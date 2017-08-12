using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarNumericSimulator.Reporting
{
    public class ModuleResourceLevels
    {
        float[] resourceReceipt;

        public ModuleResourceLevels(float[] receipt)
        {
            resourceReceipt = receipt;
        }

        public float getResourceLevel(Resources res)
        {
            return resourceReceipt[(int)res];
        }

    }
}
