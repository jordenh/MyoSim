using MyoSharp.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI
{
    class MyoRecorder
    {
        private Multimap<uint, RecorderFileHandler.RecordedData> timestampToData;
        ParsedCommands.ParsedCommand.Quaternion storedOrientation;
        ParsedCommands.ParsedCommand.vector3 storedAccel;
        ParsedCommands.ParsedCommand.vector3 storedGyro;
        ParsedCommands.AsyncCommand.armDirection storedArmDirection;
        long firstTime;
        MyoSharp.Device.IHub hub;
        MyoSharp.Device.IMyo connectedMyo;


        public MyoRecorder()
        {
            timestampToData = new Multimap<uint, RecorderFileHandler.RecordedData>();
            connectedMyo = null;
        }

        public Multimap<uint, RecorderFileHandler.RecordedData> StopRecording()
        {
            Multimap<uint, RecorderFileHandler.RecordedData> copyTimestampToData =
                new Multimap<uint, RecorderFileHandler.RecordedData>(timestampToData);
            unregisterMyoEvents(connectedMyo);
            connectedMyo = null;
            hub = null;
            timestampToData.getUnderlyingDict().Clear();
            return copyTimestampToData;
        }

        public void Record()
        {
            using (hub = Hub.Create())
            {
                // listen for when the Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
                    connectedMyo = e.Myo;
                    firstTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    timestampToData.Add(0, new RecorderFileHandler.RecordedData(
                        ParsedCommands.ParsedCommand.AsyncCommandCode.CONNECT));
                    e.Myo.Vibrate(VibrationType.Short);
                    e.Myo.AccelerometerDataAcquired += Myo_AccelerometerData;
                    e.Myo.ArmLost += Myo_ArmLost;
                    e.Myo.ArmRecognized += Myo_ArmRecognized;
                    e.Myo.GyroscopeDataAcquired += Myo_GyroscopeData;
                    e.Myo.OrientationDataAcquired += Myo_OrientationData;
                    e.Myo.PoseChanged += Myo_PoseChanged;

                };

                // listen for when the Myo disconnects
                hub.MyoDisconnected += (sender, e) =>
                {
                    Console.WriteLine("Myo has disconnected!", e.Myo.Arm);
                    long millis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    uint time = (uint)(millis - firstTime);
                    timestampToData.Add(time, new RecorderFileHandler.RecordedData(
                        ParsedCommands.ParsedCommand.AsyncCommandCode.DISCONNECT));
                    if (connectedMyo != null)
                    {
                        unregisterMyoEvents(connectedMyo);
                    }
                };
            }
        }

        private void unregisterMyoEvents(MyoSharp.Device.IMyo myo)
        {
            myo.AccelerometerDataAcquired -= Myo_AccelerometerData;
            myo.ArmLost -= Myo_ArmLost;
            myo.ArmRecognized -= Myo_ArmRecognized;
            myo.GyroscopeDataAcquired -= Myo_GyroscopeData;
            myo.OrientationDataAcquired -= Myo_OrientationData;
            myo.PoseChanged -= Myo_PoseChanged;
        }

        private void sendOrientationData(long timestamp)
        {
            uint time = (uint)(timestamp - firstTime);
            RecorderFileHandler.RecordedData syncDat = 
                new RecorderFileHandler.RecordedData(storedOrientation, storedGyro, storedAccel);
            timestampToData.Add(time, syncDat);
        }

        private void Myo_OrientationData(object sender, OrientationDataEventArgs e)
        {
            ParsedCommands.ParsedCommand.Quaternion orientation; 
            /* Correct for orientation of Myo */
            /* Some what of a hack. Project Midas should recieve get
             * the xDirection and Arm and correct for this. I.e sending
             * the armRecognize event should handle this
             */
            if (storedArmDirection.xDirection != HubCommunicator.XDirection.FACING_ELBOW)
            {
                orientation = CommandRunner.getQuatFromAngles(
                new ParsedCommands.ParsedCommand.vector3((float) e.Yaw, -1*(float) e.Pitch, -1*(float) e.Roll));
            }
            else
            {
                orientation = CommandRunner.getQuatFromAngles(
                    new ParsedCommands.ParsedCommand.vector3((float) e.Yaw, (float) e.Pitch, (float) e.Roll));
            }

            storedOrientation = orientation;
            sendOrientationData(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        }

        private void Myo_GyroscopeData(object sender, GyroscopeDataEventArgs e)
        {
            /* Correct for orientation of Myo */
            /* Some what of a hack. Project Midas should recieve get
             * the xDirection and Arm and correct for this. I.e sending
             * the armRecognize event should handle this
             */
            if (storedArmDirection.xDirection != HubCommunicator.XDirection.FACING_ELBOW)
            {
                storedGyro = new ParsedCommands.ParsedCommand.vector3(
                    e.Gyroscope.X,
                    -1 * e.Gyroscope.Y,
                    -1 * e.Gyroscope.Z);
            }
            else
            {
                storedGyro = new ParsedCommands.ParsedCommand.vector3(e.Gyroscope.X, e.Gyroscope.Y, e.Gyroscope.Z);
            }
        }

        private void Myo_AccelerometerData(object sender, AccelerometerDataEventArgs e)
        {
            /* Correct for orientation of Myo */
            /* Some what of a hack. Project Midas should recieve get
             * the xDirection and Arm and correct for this. I.e sending
             * the armRecognize event should handle this
             */
            if (storedArmDirection.xDirection != HubCommunicator.XDirection.FACING_ELBOW)
            {
                storedAccel = new ParsedCommands.ParsedCommand.vector3(e.Accelerometer.X,
                    -1 * e.Accelerometer.Y, -1 * e.Accelerometer.Z);
            }
            else
            {
                storedAccel = new ParsedCommands.ParsedCommand.vector3(e.Accelerometer.X,
                    e.Accelerometer.Y, e.Accelerometer.Z);
            }
        }

        private void Myo_ArmRecognized(object sender, ArmRecognizedEventArgs e)
        {
            long millis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            uint time = (uint)(millis - firstTime);
            storedArmDirection = new ParsedCommands.AsyncCommand.armDirection(
                                    (HubCommunicator.Arm)e.Arm,
                                    (HubCommunicator.XDirection)e.XDirection);
            timestampToData.Add(time, 
                new RecorderFileHandler.RecordedData(
                    ParsedCommands.ParsedCommand.AsyncCommandCode.ARM_RECOGNIZED,
                    storedArmDirection));
        }

        private void Myo_ArmLost(object sender, MyoEventArgs e)
        {
            long millis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            uint time = (uint)(millis - firstTime);
            timestampToData.Add(time,
                new RecorderFileHandler.RecordedData(
                    ParsedCommands.ParsedCommand.AsyncCommandCode.ARM_LOST));
        }

        private void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            long millis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            uint time = (uint)(millis - firstTime);
            timestampToData.Add(time,
                new RecorderFileHandler.RecordedData(
                    translateMyoSharpPoseToAsyncCommand(e.Pose)));
        }

        private ParsedCommands.ParsedCommand.AsyncCommandCode translateMyoSharpPoseToAsyncCommand(MyoSharp.Poses.Pose pose)
        {
            ParsedCommands.ParsedCommand.AsyncCommandCode code;
            switch (pose)
            {
                case MyoSharp.Poses.Pose.FingersSpread:
                    code = ParsedCommands.ParsedCommand.AsyncCommandCode.FINGERS_SPREAD;
                    break;
                case MyoSharp.Poses.Pose.Fist:
                    code = ParsedCommands.ParsedCommand.AsyncCommandCode.FIST;
                    break;
                case MyoSharp.Poses.Pose.Rest:
                    code = ParsedCommands.ParsedCommand.AsyncCommandCode.REST;
                    break;
                case MyoSharp.Poses.Pose.ThumbToPinky:
                    code = ParsedCommands.ParsedCommand.AsyncCommandCode.THUMB_TO_PINKY;
                    break;
                case MyoSharp.Poses.Pose.Unknown:
                    code = ParsedCommands.ParsedCommand.AsyncCommandCode.UNKNOWN;
                    break;
                case MyoSharp.Poses.Pose.WaveIn:
                    code = ParsedCommands.ParsedCommand.AsyncCommandCode.WAVE_IN;
                    break;
                default:
                    code = ParsedCommands.ParsedCommand.AsyncCommandCode.WAVE_OUT;
                    break;
            }

            return code;
        }

        // Device Listeners
    }
}
