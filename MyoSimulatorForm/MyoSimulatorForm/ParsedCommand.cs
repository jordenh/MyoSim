using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI
{
    /*
     * storage for the command read in from the human readable file format
     */
    class parsed_command
    {
        public parsed_command(string line)
        {
            string[] command = line.Split(command_delim);
            switch (command.First())
            {
                case MOVE_KW:
                    if (command.Length == SIZEOF_MOVE_CMD &&
                        (int.TryParse(command.Last(), out t)) &&
                        t >= 0 &&
                        (float.TryParse(command[1], out gyro_data.x)) &&
                        (float.TryParse(command[2], out gyro_data.y)) &&
                        (float.TryParse(command[3], out gyro_data.z)) )
                    {
                        keyword = MOVE_KW;
                    }
                    break;
                case SET_ACCEL_KW:
                    if (command.Length == SIZEOF_SET_ACC_CMD &&
                        (float.TryParse(command[1], out accel_data.x)) &&
                        (float.TryParse(command[2], out accel_data.y)) &&
                        (float.TryParse(command[3], out accel_data.z)) )
                    {
                        keyword = SET_ACCEL_KW;
                    }
                    break;
                case DELAY_KW:
                    if (command.Length == SIZEOF_DELAY_CMD &&
                        (int.TryParse(command[1], out delay)) &&
                        delay >= 0 )
                    {
                        keyword = DELAY_KW;
                    }
                    break;
                case ASYNC_KW:
                    if (command.Length == SIZEOF_ASYNC_CMD)
                    {
                        if (int.TryParse(command.Last(), out t) &&
                                t >= 0)
                        {
                            if ((int.TryParse(command[1], out async_cmd_num) &&
                                async_cmd_num >= 0) )
                                
                            {
                                keyword = ASYNC_KW;
                            }
                            else if (command[1] is string) 
                            {
                                keyword = ASYNC_KW;
                                ev_name = String.Copy(command[1]);
                            }
                        }
                        
                            

                    }

                    break;
                case EXPECT_KW:
                    if (command.Length == SIZEOF_EXPECT_CMD &&
                        command[1] is string &&
                        int.TryParse(command.Last(), out t))
                    {
                        keyword = EXPECT_KW;
                    }
                    break;
                default:
                    break;
            }       
            

        }

        ~parsed_command()
        {
        }

        /*
         * Set the gyroscope x,y,z values
         * @param   x   x vector of gyroscope data
         * @param   y   y vector of gyroscope data
         * @param   z   z vector of gyroscope data
         */
        public void set_gyro(float x, float y, float z)
        {
            gyro_data.x = x;
            gyro_data.y = y;
            gyro_data.z = z;
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

        /*
         * Print out the variables of this object.
         * Used for checking correctness
         */
        public void print_debug()
        {
            Console.WriteLine("kw: " + keyword + " t: " + t + " delay: " +
                delay + " async_cmd_num: " + async_cmd_num + " ev_name: " +
                ev_name + "\n");
        }

        struct Quaternion
        {
            public float x;
            public float y;
            public float z;
            public float w;

            //string to_string()
            //{

            //}
        }

        struct vector3
        {
            public float x;
            public float y;
            public float z;
        }

        /* static constants */
        public const int SIZEOF_MOVE_CMD = 5;
        public const int SIZEOF_SET_ACC_CMD = 2;
        public const int SIZEOF_ASYNC_CMD = 3;
        public const int SIZEOF_EXPECT_CMD = 3;
        public const int SIZEOF_DELAY_CMD = 2;

        public const int TIME_B_START = 0;
        public const int QUAT_B_START = 4;
        public const int GYRO_B_START = 19;
        public const int ACCL_B_START = 31;

        private static int ms_btw_send = 10;
        
        /* Keywords in our language. */
        public const string MOVE_KW = "move";
        public const string SET_ACCEL_KW = "set_accel";
        public const string DELAY_KW = "delay";
        public const string ASYNC_KW = "async";
        public const string EXPECT_KW = "expect";
        public const char command_delim = ' ';

        private string keyword;
        private int t;
        private int delay;
        private int async_cmd_num;
        string ev_name;
        private Quaternion quat_data;
        private vector3 gyro_data;
        private vector3 gyro_data_prev;
        private vector3 accel_data;
        private vector3 gyro_change_per_time;

    }
}
