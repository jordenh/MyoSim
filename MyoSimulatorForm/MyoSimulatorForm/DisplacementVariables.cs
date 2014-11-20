﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI
{
    class displacement_variables
    {
        public displacement_variables(float d_prev=0, float d_next=0, float accel=0) 
        {
            d = d_next - d_prev;
            v0 = 1;
            a = accel;
        }

        ~displacement_variables() { }

        /*
         * Calculate the next displacement vector based on the next delta of time changed
         * @param   int t   time for this displacement
         * Formula: d = v0*t + (a*t^2)/2
         */
        public void calc_next_disp(int t)
        {
            d = v0 * t + (a * t * t) / 2;
        }

        /*
         * Set the acceleration
         * @param   float a     acceleration
         */
        public void set_accel(float accel)
        {
            a = accel;
        }

        /* 
         * Get methods to return displacement
         * @return  float d
         */
        public float get_disp()
        {
            return d;
        }

        private float d;
        private float v0;
        private float a;
    }
}