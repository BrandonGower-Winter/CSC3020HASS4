using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public CameraMode mode;
    [Header("First Person Properties")]
    public Transform fp_camera_tranform;
    public Transform fp_player_tranform;
    public float fp_horizontal_rotation_speed;
    public float fp_vertical_rotation_speed;
    public Vector2 vertical_clamp;

    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    switch(mode)
        {
            case CameraMode.FIRSTPERSON:
                manageFP();
                break;
            case CameraMode.THIRDPERSON:
                manageTP();
                break;
            case CameraMode.ORBIT:
                manageOrbit();
                break;
        }
	}

    private void manageOrbit()
    {
        throw new NotImplementedException();
    }

    private void manageTP()
    {
        throw new NotImplementedException();
    }

    private void manageFP()
    {
        //We want to rotate the entire players body with horizontal movement
        fp_player_tranform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * fp_horizontal_rotation_speed));
        //We want to rotate only the gun and camera with vertical movement
        fp_camera_tranform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -fp_vertical_rotation_speed,0));
        Vector3 newAngle = fp_camera_tranform.eulerAngles;
        newAngle.x = clampAngle(fp_camera_tranform.eulerAngles.x,vertical_clamp.x,vertical_clamp.y);
        fp_camera_tranform.eulerAngles = newAngle;
    }

    private float clampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}

public enum CameraMode
{
    FIRSTPERSON,
    THIRDPERSON,
    ORBIT
}
