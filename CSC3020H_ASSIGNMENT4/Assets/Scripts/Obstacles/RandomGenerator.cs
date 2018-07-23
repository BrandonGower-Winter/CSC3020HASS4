using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour {


    private static RandomGenerator _instance;

    public static RandomGenerator instance
    {
        get
        {
            return _instance;
        }
        private set {}
    }

    public AudioSource audioSource;

    public List<Material> materials;
    public List<AudioClip> clips;

	void Start ()
    {
	    if(_instance == null)
        {
            _instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
	}

    public Material getRandomMaterial()
    {
        return materials[Random.Range(0, materials.Count)];
    }

    public int getRandomClipId()
    {
        return Random.Range(0, clips.Count);
    }

    public void playAudioClip(int id)
    {
        audioSource.PlayOneShot(clips[id]);
    }
	
}
