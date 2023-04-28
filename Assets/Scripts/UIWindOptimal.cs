using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindOptimal : MonoBehaviour
{
    public Wind wind;
    public GameObject uiIndicator;
    private float direction = 0f;

    void Start()
    {

    }
    //@todo obsolete
    void Update()
    {
        if (wind.direction < 20 || wind.direction > 340)
        {
            uiIndicator.SetActive(false);
            direction = -1;
            return;
        }

        //float windOptimal = wind.GetWindOptimal(wind.direction);

        float windOptimal = 0f;
        float sailAngle = windOptimal;
        if (wind.direction <= 180)
        {
            sailAngle = 270 - sailAngle;
        }
        else
        {
            sailAngle = 90 + sailAngle;
        }

        direction = sailAngle;
        uiIndicator.SetActive(true);
    }

    void FixedUpdate()
    {
        if (direction != -1)
        {
            transform.localEulerAngles = new Vector3(0f, direction, 0f);
        }
    }
}
