using System.Collections;
using System.Collections.Generic;
using System;

namespace LunarParametricNumeric {

	public class N_ResourceManager : IResourceManager<float> {
		private float totalN; //[kg]

		public N_ResourceManager(float initialValue){
			totalN = initialValue;
		}

		public void addResource(float resource) {
			totalN += resource;
		}

		public void consumeResource(float resource) {
			totalN -= resource;
		}
		public float getLevel() {
			return totalN;
		}

		public static float LitresToKG(float litres){
			double result = litres * (10^(-3)) * 1.165;
            return Convert.ToSingle(result);
        }
	}
}