using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI.ParsedCommands
{
    /*
     * storage for the command read in from the human readable file format
     */
    class ParsedCommand
    {
        public enum CommandType { MOVE, SET_ACCELERATION, ASYNC, EXPECT, PADDING }

        // Note: Arm Sync/Unsync is not listed here, because Arm Sync requires additional
        // parameters. It should either be its own command, or we should allow optional extra
        // parameters for some async commands.
        /* NOTE: Do not remove RESERVED1. It is needed to preserved the order of the enum */
        public enum AsyncCommandCode { PAIR, UNPAIR, CONNECT, DISCONNECT, ARM_RECOGNIZED, ARM_LOST, REST, FIST, 
            WAVE_IN, WAVE_OUT, FINGERS_SPREAD, THUMB_TO_PINKY,UNKNOWN};

        public enum ExpectCommandCode { VIBRATE, RSSI };

        public static biDirectional_Dict<string, AsyncCommandCode>
            NameToAsyncCommand = new biDirectional_Dict<string, AsyncCommandCode>
        {
            {"pair", AsyncCommandCode.PAIR},
            {"unpair", AsyncCommandCode.UNPAIR},
            {"connect", AsyncCommandCode.CONNECT},
            {"disconnect", AsyncCommandCode.DISCONNECT},
            {"arm_recognized", AsyncCommandCode.ARM_RECOGNIZED},
            {"arm_lost", AsyncCommandCode.ARM_LOST},
            {"rest", AsyncCommandCode.REST},
            {"fist", AsyncCommandCode.FIST},
            {"wave_in", AsyncCommandCode.WAVE_IN},
            {"wave_out", AsyncCommandCode.WAVE_OUT},
            {"fingers_spread", AsyncCommandCode.FINGERS_SPREAD},
            {"thumb_to_pinky", AsyncCommandCode.THUMB_TO_PINKY},
            {"unknown", AsyncCommandCode.UNKNOWN}
        };

        public static biDirectional_Dict<string, AsyncCommandCode>.reverse
            AsyncCommandToName =
            new biDirectional_Dict<string, AsyncCommandCode>.reverse(
                NameToAsyncCommand);

        public static Dictionary<string, ExpectCommandCode> NameToExpectCommand = new Dictionary<string, ExpectCommandCode>
        {
            {"vibrate", ExpectCommandCode.VIBRATE},
            {"rssi", ExpectCommandCode.RSSI}
        };

        public struct Quaternion
        {
            public Quaternion(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public override string ToString()
            {
                return String.Format("({0}, {1}, {2}, {3})", x, y, z, w);
            }
            public float x;
            public float y;
            public float z;
            public float w;
        }

        public struct vector3
        {
            public vector3(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public override string ToString()
            {
                return String.Format("({0}, {1}, {2})", x, y, z);
            }

            public float x;
            public float y;
            public float z;
        }

        public struct armDirection
        {
            public armDirection(HubCommunicator.Arm arm,
                HubCommunicator.XDirection xDirection)
            {
                this.arm = arm;
                this.xDirection = xDirection;
            }

            public String toString()
            {
                return String.Format("arm: {0} xDirection: {1}", arm,
                    xDirection);
            }

            public HubCommunicator.Arm arm;
            public HubCommunicator.XDirection xDirection;
        }

        public const int TIME_B_START = 0;
        public const int QUAT_B_START = 4;
        public const int GYRO_B_START = 19;
        public const int ACCL_B_START = 31;

        private static int ms_btw_send = 10;

        private CommandType type;

        public ParsedCommand(CommandType type)
        {
            this.type = type;
        }

        ~ParsedCommand()
        {
        }

        public CommandType getType()
        {
            return type;
        }

        public override string ToString()
        {
            return "Type: " + type.ToString();
        }
    }
}
