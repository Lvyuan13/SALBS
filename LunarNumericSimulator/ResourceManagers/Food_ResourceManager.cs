using System.Collections;
using System.Collections.Generic;

namespace LunarNumericSimulator {
	public class Food_ResourceManager : ResourceManager<double> {
		private double totalFood; //[kg]

        public override Resources managedResource
        {
            get
            {
                return Resources.Food;
            }
        }

        public Food_ResourceManager(double initialValue){
			totalFood = initialValue;
		}

		public override void addResource(double resource) {
			totalFood += resource;
		}

		public override void consumeResource(double resource) {
			totalFood -= resource;
		}

		public override double getLevel() {
			return totalFood;
		}
	}
}