using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_ResourceManager : MonoBehaviour, IResourceManager<float> {
	private float totalFood; //[kg]

	public void addResource(float resource) {
		totalFood += resource;
	}

	public void consumeResource(float resource) {
		totalFood -= resource;
	}

	public float getLevel() {
		return totalFood;
	}
}