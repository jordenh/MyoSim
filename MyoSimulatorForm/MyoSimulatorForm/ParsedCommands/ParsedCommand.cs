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
        public enum CommandType { MOVE, SET_ACCELERATION, DELAY, ASYNC, EXPECT }

        // Note: Arm Sync/Unsync is not listed here, because Arm Sync requires additional
        // parameters. It should either be its own command, or we should allow optional extra
        // parameters for some async commands.
        public enum AsyncCommandCode { PAIR, UNPAIR, CONNECT, DISCONNECT, REST, FIST, 
            WAVE_IN, WAVE_OUT, FINGERS_SPREAD, THUMB_TO_PINKY, UNKNOWN};

        public enum ExpectCommandCode { VIBRATE, RSSI };

        public static Dictionary<string, AsyncCommandCode> NameToAsyncCommand = new Dictionary<string, AsyncCommandCode>
        {
            {"pair", AsyncCommandCode.PAIR},
            {"unpair", AsyncCommandCode.UNPAIR},
            {"connect", AsyncCommandCode.CONNECT},
            {"disconnect", AsyncCommandCode.DISCONNECT},
            {"rest", AsyncCommandCode.REST},
            {"fist", AsyncCommandCode.FIST},
            {"wave_in", AsyncCommandCode.WAVE_IN},
            {"wave_out", AsyncCommandCode.WAVE_OUT},
            {"fingers_spread", AsyncCommandCode.FINGERS_SPREAD},
            {"thumb_to_pinky", AsyncCommandCode.THUMB_TO_PINKY},
            {"unknown", AsyncCommandCode.UNKNOWN}
        };

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
