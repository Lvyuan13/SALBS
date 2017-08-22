using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using System.IO;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.ResourceManagers {

	public class StoredResourceManager : ResourceManager<double> {

        protected double totalResource;

        public override Resources managedResource { get; protected set; }

        public StoredResourceManager(Resources resource, double initialValue){
            totalResource = initialValue;
            managedResource = resource;
		}
		public override void addResource(double resource) {
			totalResource += resource;
		}

		public override void consumeResource(double resource) {
			totalResource -= resource;
		}
		public override double getLevel() {
			return totalResource;
		}

	}
}