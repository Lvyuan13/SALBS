using System.Collections;
using System.Collections.Generic;

namespace LunarParametricNumeric {
	public class H_ResourceManager : IResourceManager<float> {
		private float totalH; //[kg]

		public H_ResourceManager(float initialValue){
			totalH = initialValue;
		}

		public void addResource(float resource) {
			totalH += resource;
		}

		public void consumeResource(float resource) {
			totalH -= resource;
		}

		public float getLevel() {
			return totalH;
		}
	}
}