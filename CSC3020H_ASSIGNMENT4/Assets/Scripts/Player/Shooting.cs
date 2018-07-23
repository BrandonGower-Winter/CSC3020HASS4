using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {


    public float rate_of_fire;
    public float bullet_range;
    private float time_since_last_shot = 0;
    public GameObject particleEffect;
    public Transform barrelPos;
    public AudioSource gunSource;
    public AudioClip gunshot;



	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.DrawRay(transform.position, barrelPos.forward * bullet_range, Color.green);
        if (Input.GetButton("Fire1"))
        {
            if(Time.time - time_since_last_shot > rate_of_fire)
            {
                GameObject ps = Instantiate(particleEffect, barrelPos.position, barrelPos.rotation) as GameObject;
                ps.AddComponent<ParticleDestroyOnLoad>().particle_system  = ps.GetComponent<ParticleSystem>();
                gunSource.PlayOneShot(gunshot);
                
                RaycastHit raycast_hit = new RaycastHit();

                if(Physics.Raycast(barrelPos.position, barrelPos.forward, out raycast_hit, bullet_range))
                {
                    if(raycast_hit.collider.tag == "Enemy")
                    {
                        raycast_hit.transform.GetComponent<Enemy>().playDeath();
                    }
                }
                

                time_since_last_shot = Time.time;
            }
        }
	}
}
