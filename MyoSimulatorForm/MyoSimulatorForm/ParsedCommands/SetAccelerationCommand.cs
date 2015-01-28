using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI.ParsedCommands
{
    class SetAccelerationCommand : ParsedCommand
    {
        public SetAccelerationCommand(vector3 accelData)
            : base(CommandType.SET_ACCELERATION)
        {
            this.accelData = accelData;
        }

        public vector3 getAccelerationData()
        {
            return accelData;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format(", Accel Data: ({0}, {1}, {2}).",
                accelData.x, accelData.y, accelData.z);
        }

        private vector3 accelData;
    }
}
