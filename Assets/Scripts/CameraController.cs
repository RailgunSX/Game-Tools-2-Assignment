using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public Transform pivot;
    public Transform verticalPivot;
    public Vector3 offset;
    public bool useOffsetValues;
    public float horizontalRotateSpeed;
    public float verticalRotateSpeed;

    public float groundZoomOffset;
    public float maxCameraLimit;
    public float minCameraLimit;

    public bool invertY;

	// Use this for initialization
	void Start ()
    {
        if(!useOffsetValues)
        {
            offset = target.position - transform.position;
        }

        pivot.transform.position = target.transform.position;
        pivot.transform.parent = null;

        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        pivot.transform.position = target.transform.position;
        
        float horizontal = Input.GetAxis("Mouse X") * horizontalRotateSpeed;
        pivot.Rotate(0f, horizontal, 0f);

        float vertical = Input.GetAxis("Mouse Y") * verticalRotateSpeed;

        if(invertY)
        {
            verticalPivot.Rotate(vertical, 0f, 0f);
        }
        else
        {
            verticalPivot.Rotate(-vertical, 0f, 0f);
        }

        if(verticalPivot.rotation.eulerAngles.x > maxCameraLimit && verticalPivot.rotation.eulerAngles.x < 180f)
        {
            verticalPivot.rotation = Quaternion.Euler(maxCameraLimit, 0f, 0f);
        }

        if (verticalPivot.rotation.eulerAngles.x > 180f && verticalPivot.rotation.eulerAngles.x < 360f + minCameraLimit)
        {
            verticalPivot.rotation = Quaternion.Euler(360f + minCameraLimit, 0f, 0f);
        }

        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = verticalPivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0f);
        transform.position = target.position - (rotation * offset);

        if(transform.position.y < (target.position.y - groundZoomOffset))
        {
            transform.position = new Vector3(transform.position.x, target.position.y - groundZoomOffset, transform.position.z);
        }

        transform.LookAt(target);
	}
}
