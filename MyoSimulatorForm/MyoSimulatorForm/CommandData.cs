using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI
{
    class CommandData
    {
        public enum CommandType { ASYNC_COMMAND, ORIENTATION }
        public enum AsyncCommand { FIST, FINGER_SPREAD, REST } // Expand... Get from a common class.

        public CommandData(AsyncCommand asyncCommand)
        {
            type = CommandType.ASYNC_COMMAND;
            asyncDat = asyncCommand;
        }

        public CommandData(MyoSimulatorForm.Quaternion orientationDat, MyoSimulatorForm.vector3 gyroDat, MyoSimulatorForm.vector3 accelDat)
        {
            type = CommandType.ORIENTATION;
            orientation = orientationDat;
            gyro = gyroDat;
            accel = accelDat;
        }

        public AsyncCommand getAsyncCommand()
        {
            return asyncDat;
        }

        public CommandType getType()
        {
            return type;
        }

        public MyoSimulatorForm.Quaternion getOrientation()
        {
            return orientation;
        }

        public MyoSimulatorForm.vector3 getGyro()
        {
            return gyro;
        }

        public MyoSimulatorForm.vector3 getAccel()
        {
            return accel;
        }

        private CommandType type;
        private AsyncCommand asyncDat;
        private MyoSimulatorForm.Quaternion orientation;
        private MyoSimulatorForm.vector3 gyro, accel;

    }
}
