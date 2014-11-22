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

        /*
         * Convert the stored command to a byte[]
         * @return  byte[] containing the command
         */
        public byte[] to_bytes()
        {
            byte[] ret = new byte[1];
            // TODO: convert functions
            //switch (command.First())
            //{
            //    case MOVE_KW:
            ///* Check for invalid inputs */
            //        if (command.Length != SIZEOF_MOVE_CMD ||
            //            !(int.TryParse(command.Last(), out t)) ||
            //            t < 0 ||
            //            !(float.TryParse(command[1], out gyro_data.x)) ||
            //            !(float.TryParse(command[2], out gyro_data.y)) ||
            //            !(float.TryParse(command[3], out gyro_data.z)))
            //        {
            //            Console.WriteLine("ERROR in move");
            //        }
            //        else
            //        {
            //            /* Calculate the change in angles per time tick */
            //            gyro_change_per_time.x = (gyro_dat.x - gyro_dat_prev.x) / delta_time;
            //            gyro_change_per_time.y = (gyro_dat.y - gyro_dat_prev.y) / delta_time;
            //            gyro_change_per_time.z = (gyro_dat.z - gyro_dat_prev.z) / delta_time;

            //            /* Set first bit to be 1 to signal orientation data */
            //            time_dat |= (1 << (sizeof(int) * 8));

            //            /* Make sure that the last data sent is a multiple of ms_btw_send */
            //            for (int t = 0; t <= (delta_time); t += ms_btw_send)
            //            {
            //                //dispX.calc_next_disp(t);
            //                //dispY.calc_next_disp(t);
            //                //dispZ.calc_next_disp(t);

            //                /* set time of command */
            //                time_dat &= 0x80;
            //                time_dat |= (t & 0x7F);
            //                /* Calculate and save quaterion data */

            //                /* Calculate and save gyro vector */
            //                gyro_dat_prev.x += gyro_change_per_time.x;
            //                gyro_dat_prev.y += gyro_change_per_time.y;
            //                gyro_dat_prev.z += gyro_change_per_time.z;

            //                /* Acceleration data is not changing in move command */
            //                /* change all the structs to byte[] */
            //                Marshal.StructureToPtr(time_dat, time_bytes, false);
            //                Marshal.StructureToPtr(quat_dat, quat_bytes, false);
            //                Marshal.StructureToPtr(gyro_dat_prev, gyro_bytes, false);
            //                Marshal.StructureToPtr(accel_dat, accel_bytes, false);

            //                /* Combine all the byte[] into one for saving into the dictionary */
            //                System.Buffer.BlockCopy(time_bytes, 0, current_orient_command, TIME_B_START, sizeof(int));
            //                /*  current_orient_command[TIME_B_START] |= (byte)((t >> 3) & 0x0000007F);
            //                  current_orient_command[TIME_B_START + 1] = (byte)((t >> 2) & 0x000000FF);
            //                  current_orient_command[TIME_B_START + 2] = (byte)((t >> 1) & 0x000000FF);
            //                  current_orient_command[TIME_B_START + 3] = (byte)(t & 0x000000FF);
                                
            //                  current_orient_command[QUAT_B_START] += BitConverter.GetBytes(dispX.get_disp());
            //                  current_orient_command[QUAT_B_START + 1] += (byte)dispY.get_disp();
            //                  current_orient_command[3] += (byte)dispZ.get_disp();
            //                  Console.WriteLine("X " + current_orient_command[1]);
            //                  bin_command_list.Add(t, current_orient_command);
            //            */
            //            }
            //        }

            //        break;
            //    case SET_ACCEL_KW:
            //        break;
            //    case DELAY_KW:
            //        break;
            //    case ASYNC_KW:
            //        break;
            //    case EXPECT_KW:
            //        break;
            //    default:
            //        break;
            //}
                        
            return ret;
        }

        /*
         * Convert byte[] into the command data 
         */
        public void from_byte()
        {

        }


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
    }
}
