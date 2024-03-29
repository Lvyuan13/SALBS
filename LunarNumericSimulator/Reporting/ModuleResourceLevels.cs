﻿using System;
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
        List<Resources> resources;
        string moduleName;

        public ModuleResourceLevels(int id, string mName, double[] receipt, List<Resources> res) 
        {
            resourceReceipt = receipt;
            moduleID = id;
            resources = res;
            moduleName = mName;
        }

        public double getResourceLevel(Resources res)
        {
            return resourceReceipt[(int)res];
        }

        public int getID()
        {
            return moduleID;
        }

        public List<Resources> getRegisteredResources()
        {
            return resources;
        }

        public string getName()
        {
            return moduleName;
        }

    }
}
