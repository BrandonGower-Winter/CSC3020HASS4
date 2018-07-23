using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private int deathClipId;

    public GameObject enemyModel;

	void Start ()
    {
        enemyModel.GetComponent<SkinnedMeshRenderer>().materials[0].mainTexture = RandomGenerator.instance.getRandomMaterial().mainTexture;
        deathClipId = RandomGenerator.instance.getRandomClipId();
	}

    public void playDeath()
    {
        RandomGenerator.instance.playAudioClip(deathClipId);
        Destroy(gameObject);
    }

}
