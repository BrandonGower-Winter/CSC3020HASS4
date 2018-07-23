using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class RotationalLight : MonoBehaviour {


    private Rigidbody light_rigidbody;

    public Vector3 rotation;

	// Use this for initialization
	void Start ()
    {
        light_rigidbody = GetComponent<Rigidbody>();
        light_rigidbody.AddTorque(rotation);
	}
}
