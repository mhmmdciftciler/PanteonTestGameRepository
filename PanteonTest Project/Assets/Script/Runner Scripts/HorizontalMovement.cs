using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    Vector3 target= new Vector3(4.5f,0,0);
    public float speed;
    private void FixedUpdate()
    {
        if (gameObject.CompareTag("HorizontalObstacle"))
            HorObs();
    }
    void HorObs()
    {
        if (target.x - transform.position.x > 0.2 || target.x - transform.position.x < -0.2)
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3((target.x) * speed, 0, 0);
        }
        else
            target.x = -target.x;
    }
  
}
