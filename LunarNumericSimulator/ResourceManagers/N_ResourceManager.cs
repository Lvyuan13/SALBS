using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using System.IO;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.ResourceManagers {

	public class N_ResourceManager : AtmosphericResourceManager {


        public override Resources managedResource
        {
            get
            {
                return Resources.N;
            }
        }

        public override string fluidName
        {
            get { return "Nitrogen"; }
        }

        public N_ResourceManager()
        {

		}

		public override double getLevel() {
			return totalResource;
		}

		public override double LitresToKG(double litres){
			double result = litres * (10^(-3)) * 1.165;
            return result;
        }
	}
}