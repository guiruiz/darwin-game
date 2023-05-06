using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastBoom : MonoBehaviour
{
    public Wind wind;
    public Boat boat;


    private float rotationSpeed = 100f;
    private float rotationDelta = 1;
    void Start()
    {

    }

    void Update()
    {
        float realativeWindDeg = Utils.GetSailWindDeg(boat, wind);
        float relativeWindRotation = Utils.DegToRotation(realativeWindDeg);

        if (relativeWindRotation > 180)
        {
            boat.RotateMastToAngle(boat.mastRotation - rotationDelta, rotationSpeed);
        }
        if (relativeWindRotation < 180)
        {
            boat.RotateMastToAngle(boat.mastRotation + rotationDelta, rotationSpeed);

        }
    }
}
