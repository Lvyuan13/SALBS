using System.Collections;
using System.Collections.Generic;
using System;

namespace LunarNumericSimulator {
	public class H2O_ResourceManager : ResourceManager<double> {
		private double totalH2O; //[kg]

        public override Resources managedResource
        {
            get
            {
                return Resources.H2O;
            }
        }


        public H2O_ResourceManager(double initialValue){
			totalH2O = initialValue;
		}

		public override void addResource(double resource) {
			totalH2O += resource;
		}

		public override void consumeResource(double resource) {
			totalH2O -= resource;
		}

		public override double getLevel() {
			return totalH2O;
		}

		public static double LitresToKG(double litres){
			double result = litres * (10^(-3)) * 1000;
            return result;
        }
	}
}