using MyoSimGUI.ParsedCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyoSimGUI
{
    class CommandRunner
    {
        private const uint ORIENTATION_DELAY = 10;

        private HubCommunicator hubCommunicator;

        public CommandRunner(HubCommunicator hubCommunicator)
        {
            this.hubCommunicator = hubCommunicator;
        }

        public void runCommands(Multimap<uint, RecorderFileHandler.RecordedData> timeToRecordedData)
        {
            List<uint> timeList = timeToRecordedData.getUnderlyingDict().Keys.ToList();
            timeList.Sort();

            for (int i = 0; i < timeList.Count; i++)
            {
                uint time = timeList[i];
                uint delay = 0;
                if (i < timeList.Count - 1)
                { 
                    delay = timeList[i + 1] - timeList[i];
                }

                List<RecorderFileHandler.RecordedData> recordedDataList = timeToRecordedData[time];
                foreach (RecorderFileHandler.RecordedData recordedData in recordedDataList)
                {
                    if (recordedData.type == RecorderFileHandler.RecordedDataType.SYNC)
                    {
                        hubCommunicator.SendSyncData(recordedData.orientationQuat, recordedData.gyroDat, recordedData.accelDat);
                    }
                    else
                    {
                        switch (recordedData.asyncCommand)
                        {
                            case ParsedCommand.AsyncCommandCode.CONNECT:
                                hubCommunicator.SendConnected();
                                break;
                            case ParsedCommand.AsyncCommandCode.DISCONNECT:
                                hubCommunicator.SendDisconnected();
                                break;
                            case ParsedCommand.AsyncCommandCode.PAIR:
                                hubCommunicator.SendPaired();
                                break;
                            case ParsedCommand.AsyncCommandCode.UNPAIR:
                                hubCommunicator.SendUnpaired();
                                break;
                            case ParsedCommand.AsyncCommandCode.ARM_RECOGNIZED:
                                // Temporary
                                hubCommunicator.SendArmRecognized(HubCommunicator.Arm.RIGHT, HubCommunicator.XDirection.FACING_ELBOW);
                                break;
                            case ParsedCommand.AsyncCommandCode.ARM_LOST:
                                hubCommunicator.SendArmLost();
                                break;
                            default:
                                hubCommunicator.SendPose(HubCommunicator.asyncCommandCodeToPose(recordedData.asyncCommand));
                                break;
                        }
                    }
                }

                if (delay > 0)
                {
                    Thread.Sleep((int)delay);
                }
            }
        }

        public void runCommands(Multimap<uint, ParsedCommand> timeToParsedCommand)
        {
            runCommands(setTimeToRecordedData(timeToParsedCommand));
        }

        private uint getMaxTime(List<uint> timeList, Multimap<uint, ParsedCommand> timeToParsedCommand)
        {
            uint maxTime = 0;

            foreach (uint time in timeList)
            {
                List<ParsedCommand> commands = timeToParsedCommand[time];

                foreach (ParsedCommand command in commands)
                {
                    uint currTime = time;
                    if (command.getType() == ParsedCommand.CommandType.MOVE)
                    {
                        currTime += ((MoveCommand)command).getDuration();
                    }

                    if (currTime > maxTime)
                    {
                        maxTime = currTime;
                    }
                }
            }

            return maxTime;
        }

        private Multimap<uint, RecorderFileHandler.RecordedData> setTimeToRecordedData(Multimap<uint, ParsedCommand> timeToParsedCommand)
        {
            Multimap<uint, RecorderFileHandler.RecordedData> timeToRecordedData = new Multimap<uint, RecorderFileHandler.RecordedData>();
            List<uint> timeList = timeToParsedCommand.getUnderlyingDict().Keys.ToList();
            uint maxTime = getMaxTime(timeList, timeToParsedCommand);
            ParsedCommand.vector3 currentOrientation = new ParsedCommand.vector3(0, 0, 0);
            ParsedCommand.vector3 lastMoveDelta = new ParsedCommand.vector3(0, 0, 0);
            ParsedCommand.vector3 currentAcceleration = new ParsedCommand.vector3(0, 0, 0);
            ParsedCommand.vector3 currentGyro = new ParsedCommand.vector3(0, 0, 0);

            // Add multiples of 10 to the time list.
            for (uint i = 0; i <= maxTime; i+=10)
            {
                timeList.Add(i);
            }

            List<uint> distinctTimes = new List<uint>(timeList.Distinct());
            distinctTimes.Sort();
            uint lastMoveTime = 0;
            uint lastDuration = 0;

            foreach (uint time in distinctTimes)
            {
                if (timeToParsedCommand.getUnderlyingDict().ContainsKey(time))
                {
                    // Need to do a preliminary check to set Gyro and Acceleration information.
                    List<ParsedCommand> commands = timeToParsedCommand[time];

                    foreach (ParsedCommand command in commands)
                    {
                        if (command.getType() == ParsedCommand.CommandType.MOVE)
                        {
                            MoveCommand moveCommand = (MoveCommand)command;
                            ParsedCommand.vector3 yawPitchRoll = moveCommand.getGyroData();
                            uint duration = moveCommand.getDuration();
                            currentGyro.x = yawPitchRoll.x / duration;
                            currentGyro.y = yawPitchRoll.y / duration;
                            currentGyro.z = yawPitchRoll.z / duration;
                        }
                        else if (command.getType() == ParsedCommand.CommandType.SET_ACCELERATION)
                        {
                            SetAccelerationCommand setAccelCommand = (SetAccelerationCommand)command;
                            currentAcceleration = setAccelCommand.getAccelerationData();
                        }
                    }
                }

                if (time % 10 == 0)
                {
                    if (time - lastMoveTime <= lastDuration && lastDuration != 0)
                    {
                        currentOrientation.x = (((float)lastMoveDelta.x) / lastDuration) * 10 + currentOrientation.x;
                        currentOrientation.y = (((float)lastMoveDelta.y) / lastDuration) * 10 + currentOrientation.y;
                        currentOrientation.z = (((float)lastMoveDelta.z) / lastDuration) * 10 + currentOrientation.z;
                    }

                    ParsedCommand.vector3 convertedToMyo = new ParsedCommand.vector3(currentOrientation.z, currentOrientation.y, currentOrientation.x);
                    ParsedCommand.Quaternion newOrientation = getQuatFromAngles(convertedToMyo);
                    RecorderFileHandler.RecordedData orientationDat = new RecorderFileHandler.RecordedData(newOrientation,
                        currentGyro, currentAcceleration);
                    timeToRecordedData.Add(time, orientationDat);
                }

                if (timeToParsedCommand.getUnderlyingDict().ContainsKey(time))
                {
                    // Deal with setting a MOVE and ASYNC commands.
                    List<ParsedCommand> commands = timeToParsedCommand[time];

                    foreach (ParsedCommand command in commands)
                    {
                        if (command.getType() == ParsedCommand.CommandType.MOVE)
                        {
                            MoveCommand moveCommand = (MoveCommand)command;
                            ParsedCommand.vector3 yawPitchRoll = moveCommand.getGyroData();
                            uint duration = moveCommand.getDuration();
                            lastDuration = duration;
                            lastMoveTime = time;
                            lastMoveDelta = yawPitchRoll;
                        }
                        else if (command.getType() == ParsedCommand.CommandType.ASYNC)
                        {
                            AsyncCommand asyncCommand = (AsyncCommand)command;
                            RecorderFileHandler.RecordedData asyncDat = new RecorderFileHandler.RecordedData(asyncCommand.getAsyncCommand());
                            timeToRecordedData.Add(time, asyncDat);
                        }
                        else
                        {
                            // TODO: Deal with Expect
                        }
                    }
                }
            }

            return timeToRecordedData;
        }

        private ParsedCommand.Quaternion getQuatFromAngles(ParsedCommand.vector3 rollPitchYaw)
        {
            ParsedCommand.Quaternion orientation;
            float cyaw = (float)Math.Cos(rollPitchYaw.x / 2.0);
            float cpitch = (float)Math.Cos(rollPitchYaw.y / 2.0);
            float croll = (float)Math.Cos(rollPitchYaw.z / 2.0);

            float syaw = (float)Math.Sin(rollPitchYaw.x / 2.0);
            float spitch = (float)Math.Sin(rollPitchYaw.y / 2.0);
            float sroll = (float)Math.Sin(rollPitchYaw.z / 2.0);

            orientation.x = cyaw * cpitch * croll + syaw * spitch * sroll;
            orientation.y = cyaw * cpitch * sroll - syaw * spitch * croll;
            orientation.z = cyaw * spitch * croll + syaw * cpitch * sroll;
            orientation.w = syaw * cpitch * croll - cyaw * spitch * sroll;

            return orientation;
        }
    }
}
