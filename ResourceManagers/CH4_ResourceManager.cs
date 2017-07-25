using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH4_ResourceManager : MonoBehaviour, IResourceManager<float> {
	private float totalCH4; //[kg]

	public void addResource(float resource) {
		totalCH4 += resource;
	}

	public void consumeResource(float resource) {
		totalCH4 -= resource;
	}

	public float getLevel() {
		return totalCH4;
	}
}