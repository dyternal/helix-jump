using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLevel : MonoBehaviour
{
    private void Update()
    {
        if (Time.timeScale == 0) return;
        if (Input.touchCount > 0)
        {
            Touch Touch = Input.GetTouch(0);
            if (Touch.phase == TouchPhase.Moved)
            {

                float force = Touch.deltaPosition.x;
                force = Mathf.Clamp(force, -10.5f, 10.5f);
                transform.Rotate(Vector3.up, force);
            }
        }
    }
}
