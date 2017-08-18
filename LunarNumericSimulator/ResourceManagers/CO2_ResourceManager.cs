﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using System.IO;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.ResourceManagers {

	public class CO2_ResourceManager : AtmosphericResourceManager {

        public override Resources managedResource
        {
            get
            {
                return Resources.CO2;
            }
        }

        public override string fluidName
        {
            get { return "CarbonDioxide"; }
        }



        public CO2_ResourceManager()
        {

        }
		public override double getLevel() {
			return totalResource;
		}

	}
}