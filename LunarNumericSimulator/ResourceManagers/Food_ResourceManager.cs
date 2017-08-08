using System.Collections;
using System.Collections.Generic;

namespace LunarNumericSimulator {
	public class Food_ResourceManager : ResourceManager<float> {
		private float totalFood; //[kg]

		public Food_ResourceManager(float initialValue){
			totalFood = initialValue;
		}

		public override void addResource(float resource) {
			totalFood += resource;
		}

		public override void consumeResource(float resource) {
			totalFood -= resource;
		}

		public override float getLevel() {
			return totalFood;
		}
	}
}