using System.Collections;
using System.Collections.Generic;

namespace LunarParametricNumeric {
	public class CO2_ResourceManager : IResourceManager<float> {
		private float totalCO2; //[kg]

		public CO2_ResourceManager(float initialValue){
			totalCO2 = initialValue;
		}

		public void addResource(float resource) {
			totalCO2 += resource;
		}

		public void consumeResource(float resource) {
			totalCO2 -= resource;
		}

		public float getLevel() {
			return totalCO2;
		}
	}
}