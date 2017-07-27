using System.Collections;
using System.Collections.Generic;

namespace LunarParametricNumeric {
	public class Thermal_ResourceManager : IResourceManager<float> {
		private float totalEnthalpy; //[kJ]

		public Thermal_ResourceManager(float initialValue){
			totalEnthalpy = initialValue;
		}

		public void addResource(float resource) {
			totalEnthalpy += resource;
		}

		public void consumeResource(float resource) {
			totalEnthalpy -= resource;
		}

		public float getLevel() {
			return totalEnthalpy;
		}
	}
}