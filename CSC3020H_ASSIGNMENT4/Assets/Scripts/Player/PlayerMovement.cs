using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    [Header("Auto set at runtime")]
    public Rigidbody player_rigid;

    [Header("Speed Values")]
    public float forward_speed;
    public float forward_speed_clamp;
    public float backward_speed;
    public float backward_speed_clamp;
    public float sideways_speed;
    public float sideways_speed_clamp;

    [Header("Animation Properties")]
    public Animator player_animations;

    void Start ()
    {
        player_rigid = GetComponent<Rigidbody>();
        player_animations = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        player_animations.SetBool("IsWalking",manageMovement());
	}

    bool manageMovement()
    {
        bool isMoving = false;
        Vector3 movement_result = new Vector3();

        //Manage left and right
        if(Input.GetKey(KeyCode.A))
        {
            isMoving = true;
            movement_result += Vector3.left * sideways_speed;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            isMoving = true;
            movement_result +=  Vector3.right * sideways_speed;
        }

        //Manage Forward and Backwards
        if (Input.GetKey(KeyCode.W))
        {
            isMoving = true;
            movement_result += Vector3.forward * forward_speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            isMoving = true;
            movement_result += Vector3.back * backward_speed;
        }

        if(!isMoving)
        {
            //If the player hasn't move set their veclocity to 0. This will stop players immediately when the move their hands off the keys
            player_rigid.velocity = new Vector3(0, player_rigid.velocity.y);
            return false;
        }

        movePlayer(movement_result);
        clampSpeed();
        return true;
    }

    void movePlayer(Vector3 force)
    {
        player_rigid.AddRelativeForce(force);
    }

    void clampSpeed()
    {

        float zVal = Mathf.Clamp(player_rigid.velocity.z, -backward_speed, forward_speed_clamp);
        float xVal = Mathf.Clamp(player_rigid.velocity.x, -sideways_speed_clamp, sideways_speed_clamp);

        player_rigid.velocity = new Vector3(xVal, player_rigid.velocity.y, zVal);

    }
}
