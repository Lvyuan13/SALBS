using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using System.IO;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.ResourceManagers {	
	
	public class O_ResourceManager : AtmosphericResourceManager {


        public override Resources managedResource
        {
            get
            {
                return Resources.O;
            }
        }

        public override string fluidName {
            get { return "Oxygen"; }
        }

        public O_ResourceManager()
        {

		}

		public override double getLevel() {
			return totalResource;
		}

	}
}