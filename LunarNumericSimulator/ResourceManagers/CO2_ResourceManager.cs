using System.Collections;
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

        public override void addResource(float resource) {
			totalResource += resource;
		}

		public override void consumeResource(float resource) {
			totalResource -= resource;
		}
		public override float getLevel() {
			return totalResource;
		}

		public override float LitresToKG(float litres){
			double result = litres * (10^(-3)) * 1.842;
            return Convert.ToSingle(result);
        }
	}
}