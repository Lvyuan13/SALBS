using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_ResourceManager : MonoBehaviour, IResourceManager<float> {
	private float totalH; //[kg]

	public void addResource(float resource) {
		totalH += resource;
	}

	public void consumeResource(float resource) {
		totalH -= resource;
	}

	public float getLevel() {
		return totalH;
	}
}