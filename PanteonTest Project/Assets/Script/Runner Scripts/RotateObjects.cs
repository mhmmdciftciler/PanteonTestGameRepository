using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjects : MonoBehaviour
{
    //dönecek ojelerin yönü ve hızına bağlı çalışan kod dizini.
    public GameObject[] RotatingObjects;
    public Vector3[] rotationAxis; 
    public float[] rotationSpeed;


    void FixedUpdate()
    {
        for (int i = 0; i < RotatingObjects.Length; i++)
        {
            RotateObject(rotationAxis[i], rotationSpeed[i], RotatingObjects[i]);
        }

        
    }
    void RotateObject(Vector3 axis, float speed, GameObject RO)
    {
        Quaternion moveRot = Quaternion.Euler(axis * speed);
        RO.GetComponent<Rigidbody>().MoveRotation(moveRot*RO.GetComponent<Rigidbody>().rotation);
    }
}
