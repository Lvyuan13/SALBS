using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermal_ResourceManager : MonoBehaviour, IResourceManager<float> {
	private float totalEnthalpy; //[kJ]

	public void addResource(float resource) {
		totalEnthalpy += resource;
	}

	public void consumeResource(float resource) {
		totalEnthalpy -= resource;
	}

	public float getLevel() {
		return totalEnthalpy;
	}
}