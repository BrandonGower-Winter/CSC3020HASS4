using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [Header("Cameras")]
    public CameraMode mode;
    public Camera[] cameras;
    [Header("First Person Properties")]
    public Transform fp_camera_tranform;
    public Transform fp_player_tranform;
    public float fp_horizontal_rotation_speed;
    public float fp_vertical_rotation_speed;
    public Vector2 vertical_clamp;

    [Header("3rd Person Properties")]
    public Transform tp_camera_transform;
    public Transform tp_target;
    public Transform tp_target_rotation_reference;
    public float tp_horizontal_rotation_speed;
    public float tp_vertical_rotation_speed;
    public Vector3 feel_nice_offset;

    private Vector3 offset;


    private void Start()
    {
        //Disable Other Cameras
        cameras[1].enabled = false;
        cameras[2].enabled = false;

        //Set 3rd person offset
        offset = tp_target.position - tp_camera_transform.position + feel_nice_offset;

    }

    void Update ()
    {
        detectCameraChange();
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
        
    }

    private void manageTP()
    {

        tp_target.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * tp_horizontal_rotation_speed));

        tp_target_rotation_reference.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -tp_vertical_rotation_speed, 0));

        Quaternion rotation = Quaternion.Euler(tp_target_rotation_reference.eulerAngles.x, tp_target.eulerAngles.y, 0);

        //Calculate the position that the 3rd person camera should be at.
        tp_camera_transform.position = tp_target.position - (rotation * offset);

        tp_camera_transform.LookAt(tp_target);

    }

    private void manageFP()
    {
        //We want to rotate the entire players body with horizontal movement
        fp_player_tranform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * fp_horizontal_rotation_speed));
        //We want to rotate only the gun and camera with vertical movement
        fp_camera_tranform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -fp_vertical_rotation_speed,0));
        Vector3 newAngle = fp_camera_tranform.eulerAngles;
        //Clamp Angle of camera
        newAngle.x = clampAngle(fp_camera_tranform.eulerAngles.x,vertical_clamp.x,vertical_clamp.y);
        fp_camera_tranform.eulerAngles = newAngle;
    }

    private void detectCameraChange()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            mode += 1;
            if(mode > CameraMode.ORBIT)
            {
                mode = 0;
            }

            switch (mode)
            {
                case CameraMode.FIRSTPERSON:
                    cameras[0].enabled = true;
                    cameras[2].enabled = false;
                    break;
                case CameraMode.THIRDPERSON:
                    cameras[1].enabled = true;
                    cameras[0].enabled = false;
                    break;
                case CameraMode.ORBIT:
                    cameras[2].enabled = true;
                    cameras[1].enabled = false;
                    break;
            }

        }
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
