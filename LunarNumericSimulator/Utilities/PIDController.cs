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
        Queue<double> lastError = new Queue<double>();
        double lpfOrder;
        public PIDController(double pG, double iG, double dG, double lpf)
        {
            pGain = pG;
            iGain = iG;
            dGain = dG;
            lpfOrder = lpf;
            lastError.Enqueue(0);
        }

        public void removeWindup()
        {
            integrator = 0;
        }

        public double update(double error, double deltaT)
        {
            integrator += iGain * error * deltaT;
            var derivative = (error-weightedAverage(lastError)) / deltaT;
            lastError.Enqueue(error);
            if (lastError.Count > lpfOrder)
                lastError.Dequeue();
            return (pGain * error) + (iGain * integrator) + (dGain * derivative);
        }

        private double weightedAverage(Queue<double> list)
        {
            double sum = 0;
            for(int i = 0; i < list.Count; i++)
            {
                sum += i*list.ElementAt(i) / lpfOrder;
            }
            return sum / (0.5*lpfOrder+0.5);
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
