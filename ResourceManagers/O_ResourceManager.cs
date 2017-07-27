using System.Collections;
using System.Collections.Generic;

namespace LunarParametricNumeric {
	public class O_ResourceManager : IResourceManager<float> {
		private float totalO; //[kg]

		public O_ResourceManager(float initialValue){
			totalO = initialValue;
		}

		public void addResource(float resource) {
			totalO += resource;
		}

		public void consumeResource(float resource) {
			totalO -= resource;
		}

		public float getLevel() {
			return totalO;
		}
	}
}