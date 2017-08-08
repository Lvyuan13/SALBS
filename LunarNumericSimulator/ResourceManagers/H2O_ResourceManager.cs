using System.Collections;
using System.Collections.Generic;
using System;

namespace LunarNumericSimulator {
	public class H2O_ResourceManager : ResourceManager<float> {
		private float totalH2O; //[kg]

		public H2O_ResourceManager(float initialValue){
			totalH2O = initialValue;
		}

		public override void addResource(float resource) {
			totalH2O += resource;
		}

		public override void consumeResource(float resource) {
			totalH2O -= resource;
		}

		public override float getLevel() {
			return totalH2O;
		}

		public static float LitresToKG(float litres){
			double result = litres * (10^(-3)) * 1000;
            return Convert.ToSingle(result);
        }
	}
}