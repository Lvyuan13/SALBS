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
        public int moduleID;

        public ModuleResourceLevels(int id, double[] receipt)
        {
            resourceReceipt = receipt;
            moduleID = id;
        }

        public double getResourceLevel(Resources res)
        {
            return resourceReceipt[(int)res];
        }

        public int getID()
        {
            return moduleID;
        }

    }
}
