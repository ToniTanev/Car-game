using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform lookTarget;
    public float smoothSpeed = 0.125f;

    private void FixedUpdate()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, smoothSpeed);

            if (lookTarget)
            {
                transform.LookAt(lookTarget);
            }
            else
            {
                transform.LookAt(target.parent.position);
            }

            //transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
        }
    }
}
