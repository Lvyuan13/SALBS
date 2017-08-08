using System.Collections;
using System.Collections.Generic;
using System;

namespace LunarNumericSimulator {

	public class Pressure_ResourceManager{
		private float currentPressure; //[kPa]

		public Pressure_ResourceManager(float initialValue){
			currentPressure = initialValue;
		}

		public void setLevel(float level) {
			currentPressure = level;
		}

		public float getLevel() {
			return currentPressure;
		}
	}
}