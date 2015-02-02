using MyoSimGUI.ParsedCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI
{
    class RecorderFileHandler
    {
        public enum RecordedDataType { ASYNC, SYNC }
        public const uint TIMESTAMP_MASK = 0x80000000;

        private string fileName;

        public struct RecordedData
        {
            public RecordedData(ParsedCommand.AsyncCommandCode asyncCommand,
                AsyncCommand.armDirection armDirection =
                    new AsyncCommand.armDirection())
            {
                this.type = RecordedDataType.ASYNC;
                this.asyncCommand = asyncCommand;
                this.armDirection = armDirection;

                // Only the above elements should be used.
                this.orientationQuat = new ParsedCommand.Quaternion();
                this.gyroDat = new ParsedCommand.vector3();
                this.accelDat = new ParsedCommand.vector3();
            }

            public RecordedData(ParsedCommand.Quaternion orientationQuat, ParsedCommand.vector3 gyroDat, 
                ParsedCommand.vector3 accelDat)
            {
                this.type = RecordedDataType.SYNC;
                this.orientationQuat = orientationQuat;
                this.gyroDat = gyroDat;
                this.accelDat = accelDat;

                // Only the above elements should be used.
                this.asyncCommand = new ParsedCommand.AsyncCommandCode();
                this.armDirection = new AsyncCommand.armDirection();
            }

            public override string ToString()
            {
                if (type == RecordedDataType.ASYNC)
                {
                    return "Async Command Code: " + asyncCommand.ToString();
                }
                else
                {
                    return String.Format("Orientation: {0}, Yaw/Pitch/Roll: {1}, Accel: {2}", orientationQuat.ToString(), gyroDat.ToString(), accelDat.ToString());
                }
            }

            public RecordedDataType type;
            public ParsedCommand.AsyncCommandCode asyncCommand;
            public ParsedCommand.Quaternion orientationQuat;
            public ParsedCommand.vector3 gyroDat;
            public ParsedCommand.vector3 accelDat;
            public AsyncCommand.armDirection armDirection;
        }

        public RecorderFileHandler(string recorderFileName)
        {
            fileName = recorderFileName;
        }

        public Multimap<uint, RecordedData> readRecorderFile()
        {
            Multimap<uint, RecordedData> timestampToCommandDict = new Multimap<uint, RecordedData>();
            using (BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    uint timestamp = br.ReadUInt32();
                    uint actualTime = timestamp;
                    uint mask = ~TIMESTAMP_MASK;
                    actualTime &= mask;
 
                    if (timestamp != actualTime)
                    {
                        // Asynchronous -- first bit was a 1.
                        ushort action = br.ReadUInt16();
                        RecordedData commandData;
                        /* ARM_RECOGNIZE command has two additional parameters */
                        if (action ==
                            (ushort)ParsedCommand.AsyncCommandCode.ARM_RECOGNIZED)
                        {
                            byte arm = br.ReadByte();
                            byte xDir = br.ReadByte();
                            commandData =
                                new RecordedData((ParsedCommand.AsyncCommandCode) action,
                                    new AsyncCommand.armDirection(
                                        (HubCommunicator.Arm)arm,
                                        (HubCommunicator.XDirection)xDir));
                        }
                        else
                        {
                            commandData =
                                new RecordedData((ParsedCommand.AsyncCommandCode)action);
                        }
                        timestampToCommandDict.Add(actualTime, commandData);
                    }
                    else
                    {
                        ParsedCommand.Quaternion orientationQuat;
                        ParsedCommand.vector3 gyro;
                        ParsedCommand.vector3 accel;

                        orientationQuat.x = br.ReadSingle();
                        orientationQuat.y = br.ReadSingle();
                        orientationQuat.z = br.ReadSingle();
                        orientationQuat.w = br.ReadSingle();

                        gyro.x = br.ReadSingle();
                        gyro.y = br.ReadSingle();
                        gyro.z = br.ReadSingle();

                        accel.x = br.ReadSingle();
                        accel.y = br.ReadSingle();
                        accel.z = br.ReadSingle();

                        RecordedData commandData = new RecordedData(orientationQuat, gyro, accel);
                        timestampToCommandDict.Add(timestamp, commandData);
                    }
                }
            }

            return timestampToCommandDict;
        }

        public void writeRecorderFile(Multimap<uint, RecordedData> timestampToData)
        {
            using (BinaryWriter br = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                foreach (KeyValuePair<uint, List<RecordedData>> entry in timestampToData.getUnderlyingDict())
                {
                    uint timestamp = entry.Key;
                    List<RecordedData> commandDatList = entry.Value;

                    foreach (RecordedData commandDat in commandDatList)
                    {
                        
                        if (commandDat.type == RecordedDataType.ASYNC)
                        {
                            // Set the first bit in the timestamp to 1, indicating ASYNC.
                            timestamp |= TIMESTAMP_MASK;

                            br.Write(timestamp);
                            ushort action = (ushort) commandDat.asyncCommand;
                            br.Write(action);
                            /* armRecognize (aka armSync) has extra parameters */
                            if (commandDat.asyncCommand ==
                                ParsedCommand.AsyncCommandCode.ARM_RECOGNIZED)
                            {
                                br.Write((byte)commandDat.armDirection.arm);
                                br.Write((byte)commandDat.armDirection.xDirection);
                            }
                        }
                        else
                        {
                            br.Write(timestamp);

                            br.Write(commandDat.orientationQuat.x);
                            br.Write(commandDat.orientationQuat.y);
                            br.Write(commandDat.orientationQuat.z);
                            br.Write(commandDat.orientationQuat.w);

                            br.Write(commandDat.gyroDat.x);
                            br.Write(commandDat.gyroDat.y);
                            br.Write(commandDat.gyroDat.z);

                            br.Write(commandDat.accelDat.x);
                            br.Write(commandDat.accelDat.y);
                            br.Write(commandDat.accelDat.z);
                        } /* if (commandDat.type == RecordedDataType.ASYNC) */
                    } /* foreach (RecordedData commandDat in commandDatList) */
                } /* foreach (KeyValuePair<uint, List<RecordedData> */
            } /* using (BinaryWriter br */
        } /* public void writeRecorderFile */
    } /* class RecorderFileHandler */
} /* namespace MyoSimGUI */
