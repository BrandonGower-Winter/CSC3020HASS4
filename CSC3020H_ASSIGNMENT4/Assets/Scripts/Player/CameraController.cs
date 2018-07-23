using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [Header("Cameras")]
    public CameraMode mode;
    public Camera[] cameras;

    public GameObject ui_canvas;

    [Header("First Person Properties")]
    public Transform fp_camera_tranform;
    public Transform fp_player_tranform;
    public float fp_horizontal_rotation_speed;
    public float fp_vertical_rotation_speed;
    public Vector2 vertical_clamp;

    [Header("3rd Person Properties")]
    public Transform tp_camera_transform;
    public Transform tp_target;
    public float tp_horizontal_rotation_speed;
    public float tp_height_adjustment;
    public float tp_adjustment_speed;
    public Vector2 tp_height_clamps;

    [Header("Orbit Camera Properties")]
    public Transform orb_camera;
    public Transform orb_target;
    public Vector2 orb_offset;
    public float orb_speed;
    public float orb_scroll_speed;
    public float orb_elevation_speed;
    public Vector2 orb_camera_clamp;
    public Vector2 orb_scroll_clamp;

    private void Start()
    {
        //Disable Other Cameras
        cameras[1].enabled = false;
        cameras[2].enabled = false;


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

        if (Input.GetKey(KeyCode.G))
        {
            orb_camera.position += Vector3.down * orb_elevation_speed * Time.deltaTime;
            //Clamp values
            if (orb_camera.position.y - orb_target.position.y < orb_camera_clamp.x)
            {
                orb_camera.position = new Vector3(orb_camera.position.x, orb_target.position.y + orb_camera_clamp.x, orb_camera.position.z);
            }
        }
        else if (Input.GetKey(KeyCode.H))
        {
            orb_camera.position += Vector3.up * orb_elevation_speed * Time.deltaTime;
            //Clamp values
            if (orb_camera.position.y - orb_target.position.y > orb_camera_clamp.y)
            {
                orb_camera.position = new Vector3(orb_camera.position.x, orb_target.position.y + orb_camera_clamp.y, orb_camera.position.z);
            }
        }


        float scrollSpeed = Input.GetAxis("Mouse ScrollWheel");
        Vector3 new_scroll_pos = (orb_target.position - orb_camera.position) * orb_scroll_speed * scrollSpeed * Time.deltaTime;

        if(Vector3.Distance(new_scroll_pos + orb_camera.position, orb_target.position) > orb_scroll_clamp.x && Vector3.Distance(new_scroll_pos + orb_camera.position, orb_target.position) < orb_scroll_clamp.y)
        {
            orb_camera.position += new_scroll_pos;
        }


        orb_camera.RotateAround(orb_target.position, Vector3.up,orb_speed * Time.deltaTime);

        Vector3 newPos = orb_camera.position;

        orb_camera.LookAt(new Vector3(tp_target.position.x,tp_target.position.y,tp_target.position.z));

    }

    private void manageTP()
    {
        tp_target.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * tp_horizontal_rotation_speed));

        if (Input.GetAxis("Mouse Y") != 0)
        {
            float value = -Input.GetAxis("Mouse Y") * tp_adjustment_speed * Time.deltaTime;
            if (tp_camera_transform.position.y + value > tp_height_clamps.x && tp_camera_transform.position.y + value< tp_height_clamps.y)
                tp_camera_transform.position += Vector3.up * value;
        }

        tp_camera_transform.LookAt(new Vector3(tp_target.position.x, tp_target.position.y + tp_height_adjustment, tp_target.position.z));

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
                    ui_canvas.SetActive(true);
                    cameras[0].enabled = true;
                    cameras[2].enabled = false;
                    break;
                case CameraMode.THIRDPERSON:
                    ui_canvas.SetActive(false);
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
