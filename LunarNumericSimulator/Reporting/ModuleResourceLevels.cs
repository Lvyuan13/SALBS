using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarNumericSimulator.Reporting
{
    public class ModuleResourceLevels
    {
        double[] resourceReceipt;

        public ModuleResourceLevels(double[] receipt)
        {
            resourceReceipt = receipt;
        }

        public double getResourceLevel(Resources res)
        {
            return resourceReceipt[(int)res];
        }

    }
}
