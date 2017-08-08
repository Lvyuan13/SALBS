using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using System.IO;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.ResourceManagers {

	public class CH4_ResourceManager : AtmosphericResourceManager {

        public override string thermoFile
        {
            get
            {
                return "Methane.csv";
            }
        }

        public CH4_ResourceManager(float initialValue): base(initialValue){

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
			double result = litres * (10^(-3)) * 0.668;
            return Convert.ToSingle(result);
        }
	}
}