using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateWithAngle : MonoBehaviour
{
    void Start()
    {
        var rotation = transform.rotation.z;
        if (rotation != 0)
        {
            return;
        }
        else
        {
            var zRotation = 45f;
            transform.Rotate(0, 0, zRotation);
        }
    }

}
