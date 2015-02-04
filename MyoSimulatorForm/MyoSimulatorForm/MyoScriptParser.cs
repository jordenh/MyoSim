using MyoSimGUI.ParsedCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyoSimGUI
{
    class MyoScriptParser
    {
        public const char commandDelim = ' ';

        /* Keywords in our language. */
        public const string MOVE_KW = "move";
        public const string SET_ACCEL_KW = "set_accel";
        public const string DELAY_KW = "delay";
        public const string ASYNC_KW = "async";
        public const string EXPECT_KW = "expect";

        /* static constants */
        public const int SIZEOF_MOVE_CMD = 5;
        public const int SIZEOF_SET_ACC_CMD = 4;
        public const int SIZEOF_ASYNC_CMD = 2;
        public const int SIZEOF_ASYNC_ARMRECOG_CMD = 4;
        public const int SIZEOF_EXPECT_CMD = 3;
        public const int SIZEOF_DELAY_CMD = 2;

        private string scriptFileName;
        private string scriptText;
        private bool fromFile;

        public MyoScriptParser(string filename, bool isFilename)
        {
            if (isFilename)
            {
                scriptFileName = filename;
                fromFile = true;
            }
            else
            {
                /* actual script text. I.e. what's written in the file */
                scriptText = filename;
                fromFile = false;
            }
        }

        public Multimap<uint, ParsedCommand> parseScript()
        {
            if (fromFile)
            {
                return parseScriptFile();
            }
            else
            {
                return parseScriptText();
            }
        }

        private Multimap<uint, ParsedCommand> parseScriptFile()
        {
            System.IO.StreamReader scriptFile = new System.IO.StreamReader(scriptFileName);
            Multimap<uint, ParsedCommand> timestampToCommandDict = new Multimap<uint, ParsedCommand>();
            string line;

            uint currentTime = 0;
            bool lastCmd = false;
            while ((line = scriptFile.ReadLine()) != null)
            {
                unsafe
                {
                    parseLine(line, &lastCmd, &currentTime, timestampToCommandDict);
                }
            }

            if (lastCmd)
            {
                // If the last seen command is a delay, add a PADDING 
                // event at the end to ensure that the script runs until the end of this delay.
                timestampToCommandDict.Add(currentTime, new ParsedCommand(ParsedCommand.CommandType.PADDING));
            }

            return timestampToCommandDict;
        }

        private Multimap<uint, ParsedCommand> parseScriptText()
        {
            System.IO.StringReader scriptFile = new System.IO.StringReader(scriptText);
            Multimap<uint, ParsedCommand> timestampToCommandDict = new Multimap<uint, ParsedCommand>();
            string line;

            uint currentTime = 0;
            bool lastCmd = false;
            while ((line = scriptFile.ReadLine()) != null)
            {
                unsafe
                {
                    parseLine(line, &lastCmd, &currentTime, timestampToCommandDict);
                }
            }

            if (lastCmd)
            {
                // If the last seen command is a delay, add a PADDING 
                // event at the end to ensure that the script runs until the end of this delay.
                timestampToCommandDict.Add(currentTime, new ParsedCommand(ParsedCommand.CommandType.PADDING));
            }

            return timestampToCommandDict;
        }

        unsafe private void parseLine(string line, bool *lastCmd, uint* currentTime,
            Multimap<uint, ParsedCommand> timestampToCommandDict)
        {
            string[] command = line.Split(commandDelim);
            bool parseSuccess = true;

            switch (command.First())
            {
                case MOVE_KW:
                    parseSuccess = parseMoveEvent(command, ref *currentTime, timestampToCommandDict);
                    break;
                case SET_ACCEL_KW:
                    parseSuccess = parseSetAccelEvent(command, *currentTime, timestampToCommandDict);
                    break;
                case DELAY_KW:
                    parseSuccess = parseDelayEvent(command, ref *currentTime);
                    break;
                case ASYNC_KW:
                    parseSuccess = parseAsyncEvent(command, *currentTime, timestampToCommandDict);
                    break;
                case EXPECT_KW:
                    parseSuccess = parseExpectEvent(command, *currentTime, timestampToCommandDict);
                    break;
                default:
                    break;
            }

            if (!parseSuccess)
            {
                // TODO: Throw an exception.
            }

            if (command.First() == DELAY_KW)
            {
                *lastCmd = true;
            }
            else
            {
                *lastCmd = false;
            }
        }
        
        private bool parseMoveEvent(string[] command, ref uint currentTime, Multimap<uint, ParsedCommand> timestampToCommandDict)
        {
            ParsedCommand.vector3 gyroData;
            bool parseSuccess = true;
            uint duration;

            if (command.Length != SIZEOF_MOVE_CMD) return false;

            parseSuccess |= float.TryParse(command[1], out gyroData.x);
            parseSuccess |= float.TryParse(command[2], out gyroData.y);
            parseSuccess |= float.TryParse(command[3], out gyroData.z);
            parseSuccess |= uint.TryParse(command[4], out duration);

            if (parseSuccess)
            {
                gyroData.x = -degreesToRadians(gyroData.x);
                gyroData.y = degreesToRadians(gyroData.y);
                gyroData.z = degreesToRadians(gyroData.z);
                ParsedCommand parsedCommand = new MoveCommand(gyroData, duration);
                timestampToCommandDict.Add(currentTime, parsedCommand);
                currentTime += duration;
            }

            return parseSuccess;
        }

        private float degreesToRadians(float angle)
        {
            return (float) (Math.PI / 180.0) * angle;
        }

        private bool parseSetAccelEvent(string[] command, uint currentTime, Multimap<uint, ParsedCommand> timestampToCommandDict)
        {
            ParsedCommand.vector3 accelData;
            bool parseSuccess = true;
            uint relativeSetTime;

            if (command.Length < SIZEOF_SET_ACC_CMD) return false;

            parseSuccess |= float.TryParse(command[1], out accelData.x);
            parseSuccess |= float.TryParse(command[2], out accelData.y);
            parseSuccess |= float.TryParse(command[3], out accelData.z);

            if (command.Length <= 4 || !uint.TryParse(command[4], out relativeSetTime))
            {
                // If the user does not supply a time to set the acceleration,
                // set it at the current time.
                relativeSetTime = 0;
            }

            if (parseSuccess)
            {
                ParsedCommand parsedCommand = new SetAccelerationCommand(accelData);
                timestampToCommandDict.Add(currentTime + relativeSetTime, parsedCommand);
            }

            return parseSuccess;
        }
        
        private bool parseDelayEvent(string[] command, ref uint currentTime)
        {
            uint delay;

            if (command.Length != SIZEOF_DELAY_CMD) return false;

            if (uint.TryParse(command[1], out delay))
            {
                currentTime += delay;
                return true;
            }

            return false;
        }

        private bool parseAsyncEvent(string[] command, uint currentTime, Multimap<uint, ParsedCommand> timestampToCommandDict)
        {
            if (command.Length < SIZEOF_ASYNC_CMD) return false;

            ParsedCommand parsedCommand;
            uint asyncCommandNum;
            uint relativeSetTime;
            string arm_string;
            string xDirection_string;
            uint arm = (uint)HubCommunicator.Arm.RIGHT;
            uint xDirection = (uint)HubCommunicator.XDirection.FACING_ELBOW;
            
            if (command.Length <= SIZEOF_ASYNC_CMD ||
                !uint.TryParse(command[SIZEOF_ASYNC_CMD], out relativeSetTime))
            {
                // If the user does not supply a time to set the async event,
                // set it at the current time.
                relativeSetTime = 0;
            }
            else if (command.Length <= SIZEOF_ASYNC_ARMRECOG_CMD ||
                !uint.TryParse(command[SIZEOF_ASYNC_ARMRECOG_CMD], out relativeSetTime))
            {
                relativeSetTime = 0;
            }

            if (!uint.TryParse(command[1], out asyncCommandNum))
            {
                if (!ParsedCommand.NameToAsyncCommand.ContainsKey(command[1])) return false;

                asyncCommandNum = (uint) ParsedCommand.NameToAsyncCommand[command[1]];
            }

            if (asyncCommandNum == (uint) ParsedCommand.AsyncCommandCode.ARM_RECOGNIZED)
            {
                arm_string = command[2];
                xDirection_string = command[3];
                if (AsyncCommand.stringToArm.ContainsKey(arm_string))
                {
                    arm = (uint)AsyncCommand.stringToArm[arm_string];
                }
                else
                {
                    MessageBox.Show(
                        "Invalid Arm value: " + arm_string /* Text */,
                        "Invalid Arm" /* Title */,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                }

                if (AsyncCommand.stringToxDir.ContainsKey(xDirection_string))
                {
                    xDirection = (uint)AsyncCommand.stringToxDir[xDirection_string];
                }
                else
                {
                    MessageBox.Show(
                        "Invalid xDirection value: " + xDirection_string /* Text */,
                        "Invalid xDirection" /* Title */,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                }

                parsedCommand = new AsyncCommand((ParsedCommand.AsyncCommandCode) asyncCommandNum,
                    (HubCommunicator.Arm)arm,
                    (HubCommunicator.XDirection)xDirection);
            }
            else
            {
                parsedCommand = new AsyncCommand((ParsedCommand.AsyncCommandCode) asyncCommandNum);
            }

            timestampToCommandDict.Add(currentTime + relativeSetTime, parsedCommand);

            return true;
        }

        private bool parseExpectEvent(string[] command, uint currentTime, Multimap<uint, ParsedCommand> timestampToCommandDict)
        {
            ParsedCommand parsedCommand;
            bool parseSuccess = true;
            uint waitTime;
            if (command.Length != SIZEOF_EXPECT_CMD) return false;

            parseSuccess |= ParsedCommand.NameToExpectCommand.ContainsKey(command[1]);
            parseSuccess |= uint.TryParse(command[2], out waitTime);

            if (parseSuccess)
            {
                ParsedCommand.ExpectCommandCode commandCode = ParsedCommand.NameToExpectCommand[command[1]];
                parsedCommand = new ExpectCommand(commandCode, waitTime);
                timestampToCommandDict.Add(currentTime, parsedCommand);
            }

            return parseSuccess;
        }
    }
}
