using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomObstacleMovement : MonoBehaviour {


    public Transform level_platform;

    public NavMeshAgent agent;

    private Vector3 target;

    private Vector2 meshSize;

	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        target = getRandomPos();
        Mesh mesh = level_platform.GetComponent<MeshFilter>().mesh;
        meshSize = new Vector2(mesh.bounds.size.x * transform.localScale.x/2f, mesh.bounds.size.z * transform.localScale.z/2f);

        if(!agent.isOnNavMesh)
        {
            transform.position = Vector3.zero;
        }

	}
	
	void Update ()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.05f)
        {
            target = getRandomPos();
            agent.destination = target;
        }
    }

    private Vector3 getRandomPos()
    {

        float xVal = Random.Range(level_platform.position.x - meshSize.x, level_platform.position.x + meshSize.x);
        float zVal = Random.Range(level_platform.position.z - meshSize.y, level_platform.position.z + meshSize.y);

        return new Vector3(xVal, 0.1f, zVal);

    }
}
