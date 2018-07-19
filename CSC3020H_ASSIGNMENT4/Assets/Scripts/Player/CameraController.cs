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
    public Vector2 tp_vertical_rotation_clamp;
    public float tp_horizontal_rotation_speed;
    public float tp_vertical_rotation_speed;
    public float tp_distance_modifyer;
    public Vector2 tp_distance_modifyers;
    public float tp_zoom_rate;
    public float tp_smooth_rate;
    private float tp_smooth_collide;
    private float tp_smooth_free;
    public Vector2 tp_distance_clamp;
    public LayerMask tp_layermask;
    private Vector3 tp_offset;

    [Header("Orbit Camera Properties")]
    public Transform orb_camera;
    public Transform orb_target;
    public Vector2 orb_offset;
    public float orb_speed;

    private void Start()
    {
        //Disable Other Cameras
        cameras[1].enabled = false;
        cameras[2].enabled = false;

        //Set 3rd person offset
        tp_offset = tp_target.position - tp_camera_transform.position;

        tp_smooth_free = tp_smooth_rate;
        tp_smooth_collide = tp_smooth_rate * 4;

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
        orb_camera.RotateAround(orb_target.position, Vector3.up,orb_speed * Input.GetAxis("Mouse X"));
        orb_camera.LookAt(orb_target);
    }

    private void manageTP()
    {
        tp_target.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * tp_horizontal_rotation_speed));

        tp_target_rotation_reference.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -tp_vertical_rotation_speed, 0));
        Vector3 newAngle = tp_target_rotation_reference.eulerAngles;
        newAngle.x = clampAngle(newAngle.x, tp_vertical_rotation_clamp.x, tp_vertical_rotation_clamp.y);
        tp_target_rotation_reference.eulerAngles = newAngle;

        float scrollSpeed = Input.GetAxis("Mouse ScrollWheel");
        if (scrollSpeed < 0)
        {
            tp_distance_modifyer += tp_zoom_rate;
        }
        else if(scrollSpeed > 0)
        {
            tp_distance_modifyer -= tp_zoom_rate;
        }

        tp_distance_modifyer = Mathf.Clamp(tp_distance_modifyer, tp_distance_clamp.x, tp_distance_clamp.y);


        float current_y_angle = tp_camera_transform.eulerAngles.y;
        float desired_y_angle = tp_target.transform.eulerAngles.y;
        float y_angle = Mathf.LerpAngle(current_y_angle, desired_y_angle, Time.deltaTime * tp_smooth_rate);

        Quaternion rotation = Quaternion.Euler(tp_target_rotation_reference.eulerAngles.x, y_angle, 0);

        Vector3 newPos = tp_target.position - (rotation * tp_offset * tp_distance_modifyer);
          
        applyCollisionDetection(ref newPos);

        tp_camera_transform.position = Vector3.Lerp(tp_camera_transform.position, newPos, tp_smooth_rate);

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

    private void applyCollisionDetection(ref Vector3 newPos)
    {
        RaycastHit collisionDetect = new RaycastHit();
        
        if(Physics.Linecast(tp_target.position,newPos,out collisionDetect,tp_layermask))
        {
            tp_smooth_rate = tp_smooth_collide;
            newPos = new Vector3(collisionDetect.point.x + collisionDetect.normal.x * 0.1f, newPos.y, collisionDetect.point.z + collisionDetect.normal.z * 0.1f);
            
        }
        else
        {
            tp_smooth_rate = tp_smooth_free;
        }

    }

}

public enum CameraMode
{
    FIRSTPERSON,
    THIRDPERSON,
    ORBIT
}
