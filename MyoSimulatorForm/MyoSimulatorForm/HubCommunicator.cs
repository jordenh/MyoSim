using MyoSimGUI.ParsedCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI
{
    class HubCommunicator
    {
        public enum EventType
        {
            PAIRED,
            UNPAIRED,
            CONNECTED,
            DISCONNECTED,
            ARM_RECOGNIZED,
            ARM_LOST,
            SYNC_DAT,
            POSE
        }

        public enum Arm
        {
            RIGHT,
            LEFT,
            UNKNOWN
        }

        public enum XDirection
        {
            FACING_WRIST,
            FACING_ELBOW,
            UNKNOWN
        }

        public enum Pose
        {
            REST,
            FIST,
            WAVE_IN,
            WAVE_OUT,
            FINGERS_SPREAD,
            RESERVED1,
            THUMB_TO_PINKY,
            UNKNOWN
        }

        private NamedPipeServerStream pipeStream;

        public HubCommunicator(NamedPipeServerStream pipeStream)
        {
            this.pipeStream = pipeStream;
        }

        public void SendPaired()
        {
            SendSimpleCommand(EventType.PAIRED);
        }

        public void SendUnpaired()
        {
            SendSimpleCommand(EventType.UNPAIRED);
        }

        public void SendConnected()
        {
            SendSimpleCommand(EventType.CONNECTED);
        }

        public void SendDisconnected()
        {
            SendSimpleCommand(EventType.DISCONNECTED);
        }

        public void SendArmLost()
        {
            SendSimpleCommand(EventType.ARM_LOST);
        }

        public void SendSyncData(ParsedCommand.Quaternion orientation, ParsedCommand.vector3 gyro,
            ParsedCommand.vector3 accelData)
        {
            MemoryStream s = new MemoryStream();

            using (BinaryWriter br = new BinaryWriter(s))
            {
                br.Write((byte) 40);
                br.Write((ushort) EventType.SYNC_DAT);

                br.Write(orientation.x);
                br.Write(orientation.y);
                br.Write(orientation.z);
                br.Write(orientation.w);

                br.Write(gyro.x);
                br.Write(gyro.y);
                br.Write(gyro.z);

                br.Write(accelData.x);
                br.Write(accelData.y);
                br.Write(accelData.z);
            }

            Send(s.ToArray());
        }

        public void SendArmRecognized(Arm arm, XDirection xDirection)
        {
            MemoryStream s = new MemoryStream();

            using (BinaryWriter br = new BinaryWriter(s))
            {
                br.Write((byte) 2);
                br.Write((ushort) EventType.ARM_RECOGNIZED);
                br.Write((byte) arm);
                br.Write((byte) xDirection);
            }

            Send(s.ToArray());
        }

        public void SendPose(Pose pose)
        {
            MemoryStream s = new MemoryStream();

            using (BinaryWriter br = new BinaryWriter(s))
            {
                br.Write((byte) 2);
                br.Write((ushort) EventType.POSE);
                br.Write((ushort) pose);
            }

            Send(s.ToArray());
        }

        public bool isConnected()
        {
            return pipeStream.IsConnected;
        }

        private void SendSimpleCommand(EventType type)
        {
            MemoryStream s = new MemoryStream();
            
            using (BinaryWriter br = new BinaryWriter(s))
            {
                br.Write((byte) 0);
                br.Write((ushort) type);
            }

            Send(s.ToArray());
        }

        private void Send(byte[] data)
        {
            pipeStream.Write(data, 0, data.Length);
        }
    }
}
