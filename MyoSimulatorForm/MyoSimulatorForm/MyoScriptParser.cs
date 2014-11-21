using MyoSimGUI.ParsedCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public const int SIZEOF_EXPECT_CMD = 3;
        public const int SIZEOF_DELAY_CMD = 2;

        private string scriptFileName;

        public MyoScriptParser(string filename)
        {
            scriptFileName = filename;
        }

        public Dictionary<uint, List<ParsedCommand>> parseScript()
        {
            System.IO.StreamReader scriptFile = new System.IO.StreamReader(scriptFileName);
            Dictionary<uint, List<ParsedCommand>> timestampToCommandDict = new Dictionary<uint, List<ParsedCommand>>();
            string line;

            uint currentTime = 0;
            while ((line = scriptFile.ReadLine()) != null)
            {
                string[] command = line.Split(commandDelim);
                bool parseSuccess = true;

                switch (command.First())
                {
                    case MOVE_KW:
                        parseSuccess = parseMoveEvent(command, ref currentTime, timestampToCommandDict);
                        break;
                    case SET_ACCEL_KW:
                        parseSuccess = parseSetAccelEvent(command, currentTime, timestampToCommandDict);
                        break;
                    case DELAY_KW:
                        parseSuccess = parseDelayEvent(command, ref currentTime);
                        break;
                    case ASYNC_KW:
                        parseSuccess = parseAsyncEvent(command, currentTime, timestampToCommandDict);
                        break;
                    case EXPECT_KW:
                        parseSuccess = parseExpectEvent(command, currentTime, timestampToCommandDict);
                        break;
                    default:
                        break;
                }

                if (!parseSuccess)
                {
                    // TODO: Throw an exception.
                }
            }

            return timestampToCommandDict;
        }

        private bool parseMoveEvent(string[] command, ref uint currentTime, Dictionary<uint, List<ParsedCommand>> timestampToCommandDict)
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
                ParsedCommand parsedCommand = new MoveCommand(gyroData, duration);
                addParsedCommand(timestampToCommandDict, currentTime, parsedCommand);
                currentTime += duration;
            }

            return parseSuccess;
        }

        private bool parseSetAccelEvent(string[] command, uint currentTime, Dictionary<uint, List<ParsedCommand>> timestampToCommandDict)
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
                addParsedCommand(timestampToCommandDict, currentTime + relativeSetTime, parsedCommand);
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

        private bool parseAsyncEvent(string[] command, uint currentTime, Dictionary<uint, List<ParsedCommand>> timestampToCommandDict)
        {
            if (command.Length < SIZEOF_ASYNC_CMD) return false;

            ParsedCommand parsedCommand;
            uint asyncCommandNum;
            uint relativeSetTime;

            if (command.Length <= 2 || !uint.TryParse(command[2], out relativeSetTime))
            {
                // If the user does not supply a time to set the acceleration,
                // set it at the current time.
                relativeSetTime = 0;
            }

            if (!uint.TryParse(command[1], out asyncCommandNum))
            {
                if (!ParsedCommand.NameToAsyncCommand.ContainsKey(command[1])) return false;

                asyncCommandNum = (uint) ParsedCommand.NameToAsyncCommand[command[1]];
            }

            parsedCommand = new AsyncCommand((ParsedCommand.AsyncCommandCode) asyncCommandNum);
            addParsedCommand(timestampToCommandDict, currentTime + relativeSetTime, parsedCommand);

            return true;
        }

        private bool parseExpectEvent(string[] command, uint currentTime, Dictionary<uint, List<ParsedCommand>> timestampToCommandDict)
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
                addParsedCommand(timestampToCommandDict, currentTime, parsedCommand);
            }

            return parseSuccess;
        }

        private void addParsedCommand(Dictionary<uint, List<ParsedCommand>> timestampToCommandDict, uint time,
            ParsedCommand parsedCommand)
        {
            List<ParsedCommand> parsedCommandList;

            if (timestampToCommandDict.ContainsKey(time))
            {
                parsedCommandList = timestampToCommandDict[time];
            }
            else
            {
                parsedCommandList = new List<ParsedCommand>();
                timestampToCommandDict.Add(time, parsedCommandList);
            }

            parsedCommandList.Add(parsedCommand);
        }
    }
}
