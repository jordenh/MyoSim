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
        public RecorderFileHandler(string recorderFileName)
        {
            fileName = recorderFileName;
        }

        public Dictionary<uint, CommandData> readRecorderFile()
        {
            Dictionary<uint, CommandData> timestampToCommandDict = new Dictionary<uint, CommandData>();
            using (BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    uint timestamp = br.ReadUInt32();
                    if (timestamp >> sizeof(uint)-1 == 1)
                    {
                        // Asynchronous
                        ushort action = br.ReadUInt16();
                        CommandData commandData = new CommandData((CommandData.AsyncCommand) action);
                        timestampToCommandDict.Add(timestamp, commandData);
                    }
                    else
                    {
                        MyoSimulatorForm.Quaternion orientationQuat;
                        MyoSimulatorForm.vector3 gyro;
                        MyoSimulatorForm.vector3 accel;

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

                        CommandData commandData = new CommandData(orientationQuat, gyro, accel);
                        timestampToCommandDict.Add(timestamp, commandData);
                    }
                }
            }

            return timestampToCommandDict;
        }

        public void writeRecorderFile(Dictionary<uint, CommandData> timestampToData)
        {
            using (BinaryWriter br = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                foreach (KeyValuePair<uint, CommandData> entry in timestampToData)
                {
                    uint timestamp = entry.Key;
                    CommandData commandDat = entry.Value;
                    br.Write(timestamp);

                    if (commandDat.getType() == CommandData.CommandType.ASYNC_COMMAND)
                    {
                        ushort action = (ushort) commandDat.getAsyncCommand();
                        br.Write(action);
                    }
                    else
                    {
                        br.Write(commandDat.getOrientation().x);
                        br.Write(commandDat.getOrientation().y);
                        br.Write(commandDat.getOrientation().z);
                        br.Write(commandDat.getOrientation().w);

                        br.Write(commandDat.getGyro().x);
                        br.Write(commandDat.getGyro().y);
                        br.Write(commandDat.getGyro().z);

                        br.Write(commandDat.getAccel().x);
                        br.Write(commandDat.getAccel().y);
                        br.Write(commandDat.getAccel().z);
                    }
                }
            }
        }

        private string fileName;
    }
}
