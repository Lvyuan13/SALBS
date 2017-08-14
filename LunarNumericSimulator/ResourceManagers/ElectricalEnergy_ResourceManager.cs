using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using System.IO;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.ResourceManagers {

	public class ElectricalEnergy_ResourceManager : ResourceManager<double> {

        protected double totalResource;

        public override Resources managedResource
        {
            get
            {
                return Resources.ElecticalEnergy;
            }
        }

        public ElectricalEnergy_ResourceManager(double initialValue){
            totalResource = initialValue;

		}
		public override void addResource(double resource) {
			totalResource += resource;
		}

		public override void consumeResource(double resource) {
			totalResource -= resource;
		}
		public override double getLevel() {
			return totalResource;
		}

		public double LitresToKG(double litres){
            return 0;
        }
	}
}