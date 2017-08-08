using System;


namespace LunarNumericSimulator {
    public static class NormInv {

        private static float inverseErf(float x){
            double pi = 3.14159265359;
            double a = (8*(pi - 3))/(3*pi*(4-pi));
            double t1 = Math.Sign(x);
            double t2 = 2/(pi*a);
            double t3 = Math.Log(1-x*x)/2;
            double t4 = Math.Log(1-x*x)/a;

            double h1 = (t2 + t3)*(t2 + t3);
            double h2 = t4;
            double h3 = (t2+t3);

            double result = t1*Math.Sqrt(Math.Sqrt(h1-h2)-h3);
            return Convert.ToSingle(result);
        }

        public static float generate(float probability, float stdDev, float mean){
            double result = mean + stdDev*(Math.Sqrt(2))*inverseErf(2*probability-1);
            return Convert.ToSingle(result); 
        }
    }
}