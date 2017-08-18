using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarNumericSimulator.Utilities
{
    public class PIDController
    {
        double pGain, iGain, dGain;
        double integrator = 0;
        double lastError = 0;
        public PIDController(double pG, double iG, double dG)
        {
            pGain = pG;
            iGain = iG;
            dGain = dG;
        }

        public double update(double error, double deltaT)
        {
            integrator += error * deltaT;
            var derivative = (error-lastError) / deltaT;
            lastError = error;
            return (pGain * error) + (iGain * integrator) + (dGain * derivative);
        }

    }

    public class PIDRelay
    {
        int cycle = 0;
        int sign = 1;
        double h;
        public PIDRelay(double h_value)
        {
            h = h_value;
        }

        public double update(double clock)
        {
            if (cycle % 5 == 0)
                sign = -sign;
            cycle++;
            return h * sign;
        }


        public double getUltimateGain(double a)
        {
            if (cycle < 10)
                throw new Exception("Allow the Relay to run for 10 seconds or more!");
            return 4 * h / (Math.PI * a);
        }

        public double[] getZieglerNichols(double Ku)
        {
            double Tu = 5;

            double Kp = 0.6 * Ku;
            double Ti = 0.5 * Tu;
            double Td = 0.125 * Tu;

            double Ki = Kp / Ti;
            double Kd = Kp * Td;

            return new double[] { Kp, Ki, Kd };
        }
    }
}
