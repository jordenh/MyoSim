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
        public void Record()
        {
            using (var hub = Hub.Create())
            {
                // listen for when the Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
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
                    e.Myo.AccelerometerDataAcquired -= Myo_AccelerometerData;
                    e.Myo.ArmLost -= Myo_ArmLost;
                    e.Myo.ArmRecognized -= Myo_ArmRecognized;
                    e.Myo.GyroscopeDataAcquired -= Myo_GyroscopeData;
                    e.Myo.OrientationDataAcquired -= Myo_OrientationData;
                    e.Myo.PoseChanged -= Myo_PoseChanged;
                };
            }
        }

        private void Myo_OrientationData(object sender, OrientationDataEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Myo_GyroscopeData(object sender, GyroscopeDataEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Myo_ArmRecognized(object sender, ArmRecognizedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Myo_ArmLost(object sender, MyoEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Myo_AccelerometerData(object sender, AccelerometerDataEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            throw new NotImplementedException();
        }

        // Device Listeners
    }
}
