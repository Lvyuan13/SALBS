using System.Collections;
using System.Collections.Generic;

namespace LunarParametricNumeric {
	public class H2O_ResourceManager : IResourceManager<float> {
		private float totalH2O; //[kg]

		public H2O_ResourceManager(float initialValue){
			totalH2O = initialValue;
		}

		public void addResource(float resource) {
			totalH2O += resource;
		}

		public void consumeResource(float resource) {
			totalH2O -= resource;
		}

		public float getLevel() {
			return totalH2O;
		}

		public static float LitresToKG(float litres){
			double result = litres * (10^(-3)) * 1000;
            return Convert.ToSingle(result);
        }
	}
}