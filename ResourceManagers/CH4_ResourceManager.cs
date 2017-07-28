using System.Collections;
using System.Collections.Generic;

namespace LunarParametricNumeric {

	public class CH4_ResourceManager : IResourceManager<float> {
		private float totalCH4; //[kg]

		public CH4_ResourceManager(float initialValue){
			totalCH4 = initialValue;
		}

		public void addResource(float resource) {
			totalCH4 += resource;
		}

		public void consumeResource(float resource) {
			totalCH4 -= resource;
		}
		public float getLevel() {
			return totalCH4;
		}
	}
}