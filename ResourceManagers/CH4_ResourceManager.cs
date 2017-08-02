using System.Collections;
using System.Collections.Generic;
using System;

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

		public static float LitresToKG(float litres){
			double result = litres * (10^(-3)) * 0.668;
            return Convert.ToSingle(result);
        }
	}
}