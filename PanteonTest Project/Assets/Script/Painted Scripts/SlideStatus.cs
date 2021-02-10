using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideStatus : MonoBehaviour
{
    public GameObject saveButton;
    Vector3[] pivotPoints;
    bool[] status;
    int index = 0;
    public Slider paintSlider;
    int maxPaintedValue = 441;
    public int paintValue = 0;
    void Start()
    {
        status = new bool[441];
        pivotPoints = new Vector3[441];
        for (float i = -5; i <= 5; i += 0.5f)
        {
            for (float j = -5; j <= 5; j += .5f)
            {
                pivotPoints[index] = new Vector3(i, j, 4.195f);
                status[index] = true;
                index++;

            }
        }

    }


    public void Update()
    {

        if (Input.GetMouseButton(0))
            IsTherePainted();
        paintSlider.value = paintValue;
        paintSlider.maxValue = maxPaintedValue;
        if (paintValue > 360)
        {
            saveButton.SetActive(true);
        }
    }
    void IsTherePainted()
    {
        for (int i = 0; i < index; i++)
        {
            float distance = Vector3.Distance(transform.position, pivotPoints[i]);
            if (distance <= 0.5f && status[i])
            {
                status[i] = false;
                paintValue++;
            }
        }

    }

}
