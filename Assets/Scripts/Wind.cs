using System;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float direction = 0f;
    public float rotationSpeed = 0f;

    void Start()
    {
    }

    void Update()
    {
        float r = direction;
        if (Input.GetKey(KeyCode.Q))
        {
            r -= rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            r += rotationSpeed * Time.deltaTime;
        }

        r = Utils.DegreesTo360Range(r);

        direction = r;
    }

    private int[] WindAngleArray = new int[] { 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 105, 110, 115, 120, 125, 130, 135, 140, 145, 150, 155, 160, 165, 170, 175, 180 };
    private int[] WindOptimalArray = new int[] { 35, 35, 35, 34, 33, 32, 31, 30, 29, 27, 25, 23, 21, 19, 17, 15, 13, 12, 11, 10, 9, 8, 7, 6, 5, 5, 5, 5, 5, 5, 5, 5, 5 };
    private int[] WindHeelArray = new int[] { 3, 20, 30, 45, 43, 41, 39, 37, 35, 33, 31, 29, 27, 25, 23, 21, 19, 17, 15, 13, 11, 9, 7, 5, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0 };

    public float GetWindOptimal(float windDirection)
    {
        float windRelative = direction;
        if (direction > 180)
        {
            windRelative = 360 - windRelative;
        }

        int windIndex = GetWindIndex(windRelative);
        float windOptimal = WindOptimalArray[windIndex];

        return windOptimal;
    }

    int GetWindIndex(float windDirection)
    {
        float divisor = 5;
        float quotient = windDirection / divisor;
        float roundedQuotient = Mathf.Round(quotient);
        int roundedValue = (int)(roundedQuotient * divisor);
        int index = Array.IndexOf(WindAngleArray, roundedValue);

        return index;
    }

}
