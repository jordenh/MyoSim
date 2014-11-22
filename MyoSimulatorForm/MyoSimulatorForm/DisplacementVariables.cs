using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI
{
    class DisplacementVariables
    {
        public DisplacementVariables(float d_prev=0, float d_next=0, float accel=0) 
        {
            d = d_next - d_prev;
            v0 = 1;
            a = accel;
        }

        ~DisplacementVariables() { }

        /*
         * Calculate the next displacement vector based on the next delta of time changed
         * @param   int t   time for this displacement
         * Formula: d = v0*t + (a*t^2)/2
         */
        public void calcNextDisp(int t)
        {
            d = v0 * t + (a * t * t) / 2;
        }

        /*
         * Set the acceleration
         * @param   float a     acceleration
         */
        public void setAccel(float accel)
        {
            a = accel;
        }

        /* 
         * Get methods to return displacement
         * @return  float d
         */
        public float getDisp()
        {
            return d;
        }

        private float d;
        private float v0;
        private float a;
    }
}
