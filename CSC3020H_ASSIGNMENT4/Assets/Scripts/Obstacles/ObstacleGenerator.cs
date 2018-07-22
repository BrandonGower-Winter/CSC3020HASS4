using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {


    public int num_of_obstacles;

    public List<GameObject> spawnableObstacles;

    public Transform base_platform;

    private Vector2 meshSize;

    void Start ()
    {
        Mesh mesh = base_platform.GetComponent<MeshFilter>().mesh;
        meshSize = new Vector2(mesh.bounds.size.x * transform.localScale.x/2f, mesh.bounds.size.z * transform.localScale.z/2f);

        for(int i = 0; i < num_of_obstacles; i++)
        {
            spawnRandomObstacle();
        }

    }

    void spawnRandomObstacle()
    {

        int objId = Random.Range(0, spawnableObstacles.Count);
        Instantiate(spawnableObstacles[objId], getRandomPos(), Quaternion.identity).GetComponent<RandomObstacleMovement>().level_platform = base_platform;
    }


    private Vector3 getRandomPos()
    {

        float xVal = Random.Range(base_platform.position.x - meshSize.x, base_platform.position.x + meshSize.x);
        float zVal = Random.Range(base_platform.position.z - meshSize.y, base_platform.position.z + meshSize.y);

        return new Vector3(xVal, 0.1f, zVal);

    }

}
