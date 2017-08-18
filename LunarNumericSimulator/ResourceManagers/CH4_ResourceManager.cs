using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using System.IO;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.ResourceManagers {

	public class CH4_ResourceManager : AtmosphericResourceManager {


        public override Resources managedResource
        {
            get
            {
                return Resources.CH4;
            }
        }

        public override string fluidName
        {
            get { return "Methane"; }
        }



        public CH4_ResourceManager()
        {

		}
		public override double getLevel() {
			return totalResource;
		}

		public override double LitresToKG(double litres){
			double result = litres * (10^(-3)) * 0.668;
            return result;
        }
	}
}