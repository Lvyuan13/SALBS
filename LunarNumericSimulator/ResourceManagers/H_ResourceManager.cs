using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using System.IO;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.ResourceManagers {

	public class H_ResourceManager : ResourceManager<float> {

        protected float totalResource;

        public override Resources managedResource
        {
            get
            {
                return Resources.Food;
            }
        }

        public H_ResourceManager(float initialValue){
            totalResource = initialValue;

		}
		public override void addResource(float resource) {
			totalResource += resource;
		}

		public override void consumeResource(float resource) {
			totalResource -= resource;
		}
		public override float getLevel() {
			return totalResource;
		}

		public float LitresToKG(float litres){
			double result = litres * (10^(-3)) * 0.0899;
            return Convert.ToSingle(result);
        }
	}
}