using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using System.IO;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.ResourceManagers {

	public class TankResourceManager : ResourceManager<double> {

        protected double totalResource;

        public TankResourceManager(double initialValue){
            totalResource = initialValue;
		}

        public override Resources managedResource { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

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