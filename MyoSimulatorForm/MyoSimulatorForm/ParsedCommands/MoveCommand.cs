using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI.ParsedCommands
{
    class MoveCommand : ParsedCommand
    {
        public MoveCommand(vector3 gyroData, uint duration)
            : base(CommandType.MOVE)
        {
            this.gyroData = gyroData;
            this.duration = duration;
        }

        public vector3 getGyroData()
        {
            return gyroData;
        }

        public uint getDuration()
        {
            return duration;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format(", Gyro Data: ({0}, {1}, {2}), Duration: {3}.",
                gyroData.x, gyroData.y, gyroData.z, duration);
        }

        private vector3 gyroData;
        private uint duration;
    }
}
