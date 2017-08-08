using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarNumericSimulator.Utilities
{
    public class RBF2D_Single
    {

        protected alglib.rbfmodel rbf;
        protected alglib.rbfreport report;
        public RBF2D_Single(float[][] data)
        {
            var rbfData = new double[data.Length, 3];
            for (int i = 0; i < data.Length; i++)
            {
                rbfData[i, 0] = data[i][0];
                rbfData[i, 1] = data[i][1];
                rbfData[i, 2] = data[i][2];
            }

            alglib.rbfcreate(2, 1, out rbf);
            alglib.rbfsetpoints(rbf, rbfData);
            alglib.rbfsetalgoqnn(rbf);
            alglib.rbfbuildmodel(rbf, out report);
        }

        public float queryPoint(float temp, float pressure)
        {
            double result = alglib.rbfcalc2(rbf, temp, pressure);
            return Convert.ToSingle(result);
        }
    }

    public class RBF2D_Double
    {

        protected alglib.rbfmodel rbf;
        protected alglib.rbfreport report;
        public RBF2D_Double(float[][] data)
        {
            var rbfData = new double[data.Length, 4];
            for (int i = 0; i < data.Length; i++)
            {
                rbfData[i, 0] = data[i][0];
                rbfData[i, 1] = data[i][1];
                rbfData[i, 2] = data[i][2];
                rbfData[i, 2] = data[i][3];
            }

            alglib.rbfcreate(2, 2, out rbf);
            alglib.rbfsetpoints(rbf, rbfData);
            alglib.rbfsetalgoqnn(rbf);
            alglib.rbfbuildmodel(rbf, out report);
        }

        public double[] queryPoint(float x, float y)
        {
            double[] result;
            alglib.rbfcalc(rbf ,new double[] { x, y }, out result);
            return result;
        }
    }
}
