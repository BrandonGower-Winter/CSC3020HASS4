using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyOnLoad : MonoBehaviour {

    public ParticleSystem particle_system;
	
	// Update is called once per frame
	void Update ()
    {
        if(particle_system == null)
        {
            particle_system = GetComponent<ParticleSystem>();
        }
	    if(!particle_system.IsAlive())
        {
            Destroy(gameObject);
        }
	}
}
