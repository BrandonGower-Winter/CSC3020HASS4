using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLightColor : MonoBehaviour {


    public Light light;
    public float intensity;

	void Start ()
    {
        light = GetComponent<Light>();

        light.intensity = intensity;
        light.color = Random.ColorHSV(0, 1, 0, 1, 0, 1, 1, 1);
	}
}
