using System.Collections;
using System.Collections.Generic;

namespace LunarParametricNumeric {
	public class Food_ResourceManager : IResourceManager<float> {
		private float totalFood; //[kg]

		public Food_ResourceManager(float initialValue){
			totalFood = initialValue;
		}

		public void addResource(float resource) {
			totalFood += resource;
		}

		public void consumeResource(float resource) {
			totalFood -= resource;
		}

		public float getLevel() {
			return totalFood;
		}
	}
}