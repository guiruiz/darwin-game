using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailingForces : MonoBehaviour
{
    public Boat boat;
    public Wind wind;

    void Update()
    {
        float magnitude = wind.speed;

        float hullWindAngle = GetHullWindAngle();
        float sailWindAngle = GetSailWindAngle();

        Vector2 v1 = CalculateSailLift(magnitude, sailWindAngle);
        Vector2 v2 = CalculateWaterResistance(magnitude, sailWindAngle, hullWindAngle);
        // v3 resultant force
        Vector2 v3 = v1 + v2;

        // rotate forces relative to hull and wind
        float forcesRotationAngle = Utils.Normalize360Range(360 - hullWindAngle - boat.hullRotation);
        float forcesRotationRadians = forcesRotationAngle * Mathf.Deg2Rad;

        //Debug.Log(forcesRotationAngle + " / " + hullWindAngle + " / " + boat.hullRotation);
        //Debug.Log(GetHullCircleAngle() + " / " + GetThrustCircleAngle(v3) + " / " + GetHullWindAngle() + " / " + forcesRotationAngle);


        Vector2 sailForce = Utils.RotateVector(v1, forcesRotationRadians);
        Vector2 waterForce = Utils.RotateVector(v2, forcesRotationRadians);
        Vector2 resultantForce = Utils.RotateVector(v3, forcesRotationRadians);
        //Debug.Log("hullWindAngle= " + hullWindAngle + " | sailAngle= " + sailWindAngle + " | boatRotationAngle= " + forcesRotationAngle);

        //float sailForceAngle = Mathf.Atan2(sailForce.y, sailForce.x) * Mathf.Rad2Deg;
        //float waterForceAngle = Mathf.Atan2(waterForce.y, waterForce.x) * Mathf.Rad2Deg;
        //float resultantForceAngle = Mathf.Atan2(resultantForce.y, resultantForce.x) * Mathf.Rad2Deg;
        //Debug.Log("sailForce= " + sailForce + " / mag= " + sailForce.magnitude + " //// " + sailForceAngle + " degrees");
        //Debug.Log("waterForce= " + waterForce + " / mag= " + waterForce.magnitude + " //// " + waterForceAngle + " degrees");
        //Debug.Log("resultantForce= " + resultantForce + " / mag= " + resultantForce.magnitude + " //// " + resultantForceAngle + " degrees");

        float yOffset = 5f;
        float lineDuration = 0.1f;
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(10 * sailForce.x, yOffset, 10 * sailForce.y), Color.red, lineDuration);
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(10 * waterForce.x, yOffset, 10 * waterForce.y), Color.blue, lineDuration);
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(10 * resultantForce.x, yOffset, 10 * resultantForce.y), Color.green, lineDuration);
    }

    Vector2 CalculateSailLift(float mag, float sailDegrees)
    {
        float sailRadians = sailDegrees * Mathf.Deg2Rad;
        // define the original vector
        Vector2 vector = new Vector2(mag * Mathf.Sin(sailRadians), 0);
        // rotate the vector by theta1 + 90 degrees in radians
        Vector2 rotatedVector = Utils.RotateVector(vector, sailRadians + Mathf.PI / 2);
        //Debug.Log($"V1 Original vector: {vector}, Rotated vector: {rotatedVector}");
        return rotatedVector;
    }

    Vector2 CalculateWaterResistance(float mag, float sailDegrees, float boatDegrees)
    {
        float sailRadians = sailDegrees * Mathf.Deg2Rad;
        float boatRadians = boatDegrees * Mathf.Deg2Rad;
        // define the original vector
        Vector2 vector = new Vector2(0, -mag * Mathf.Sin(sailRadians) * Mathf.Cos(boatRadians - sailRadians));
        // rotate the vector by phi1 radians around the origin
        Vector2 rotatedVector = Utils.RotateVector(vector, boatRadians);
        //Debug.Log($"V2 Original vector: {vector}, Rotated vector: {rotatedVector}");
        return rotatedVector;
    }

    public float GetHullWindAngle()
    {
        // 0 -> 360 anti clock
        float hullAngle = Utils.ToCircleAngle(boat.hullRotation);
        float windAngle = Utils.ToCircleAngle(wind.direction);
        float hullWindAngle = hullAngle - windAngle;

        return Utils.Normalize360Range(hullWindAngle);
    }
    public float GetSailWindAngle()
    {
        // 0 -> 360 anti clock
        float sailAngle = Utils.ToCircleAngle(boat.mastRotation);
        float sailWindAngle = sailAngle + GetHullWindAngle();
        return Utils.Normalize360Range(sailWindAngle);
    }
    public float GetHullCircleAngle()
    {
        // 0 -> 360 anti clock
        return 360 - boat.hullRotation;
    }

    public float GetThrustCircleAngle(Vector2 rForce)
    {
        // 0 -> 360 anti clock
        float angle = Mathf.Atan2(rForce.y, rForce.x) * Mathf.Rad2Deg;
        //angle = (angle < 0) ? angle + 360 : angle;
        //angle = angle - wind.direction;
        angle = Utils.Normalize360Range(angle);

        return angle;
    }
}
