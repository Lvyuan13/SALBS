using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using System.IO;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.ResourceManagers {

	public class H_ResourceManager : ResourceManager<double> {

        protected double totalResource;

        public override Resources managedResource
        {
            get
            {
                return Resources.Food;
            }
        }

        public H_ResourceManager(double initialValue){
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
			double result = litres * (10^(-3)) * 0.0899;
            return result;
        }
	}
}