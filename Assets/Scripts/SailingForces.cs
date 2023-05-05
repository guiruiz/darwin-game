using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailingForces : MonoBehaviour
{
    public Boat boat;
    public Wind wind;

    public Vector2 sailForce { get; private set; }
    public Vector2 waterForce { get; private set; }
    public Vector2 resultForce { get; private set; }
    public float resultDirection { get; private set; } = 0;

    void Update()
    {
        float magnitude = wind.speed;
        float hullWindDeg = GetHullWindDeg();
        float sailWindDeg = GetSailWindDeg();

        // calculate Forces
        // v1: sail, v2: water resistance, v3: resultant
        Vector2 v1 = CalculateSailForce(magnitude, sailWindDeg);
        Vector2 v2 = CalculateWaterForce(magnitude, sailWindDeg, hullWindDeg);
        Vector2 v3 = v1 + v2;

        // calculate thurst direction  
        float thrustDeg = GetThrustDeg(v3);
        float thrustToleranceDeg = 1f;
        bool withinTolerance = Mathf.Abs(hullWindDeg - thrustDeg) <= thrustToleranceDeg;

        // rotate forces relative to wind
        float windDeg = RotationToDeg(wind.rotation);
        float windRad = DegToRad(windDeg);
        Vector2 rotatedV1 = Utils.RotateVector(v1, windRad);
        Vector2 rotatedV2 = Utils.RotateVector(v2, windRad);
        Vector2 rotatedV3 = Utils.RotateVector(v3, windRad);

        sailForce = rotatedV1;
        waterForce = rotatedV2;
        resultForce = rotatedV3;
        resultDirection = withinTolerance ? 1 : -1;

        //DebugForce("v1", rotatedV1);
        //DebugForce("v2", rotatedV2);
        //DebugForce("v3", rotatedV3);
        //DrawForce(rotatedV1, Color.red);
        //DrawForce(rotatedV2, Color.blue);
        //DrawForce(rotatedV3, Color.green);
    }

    Vector2 CalculateSailForce(float mag, float sailDeg)
    {
        float sailRad = DegToRad(sailDeg);
        // define the original vector
        Vector2 vector = new Vector2(mag * Mathf.Sin(sailRad), 0);
        // rotate the vector by theta1 + 90 degrees in radians
        Vector2 rotatedVector = Utils.RotateVector(vector, sailRad + Mathf.PI / 2);
        return rotatedVector;
    }

    Vector2 CalculateWaterForce(float mag, float sailDeg, float boatDeg)
    {
        float sailRad = DegToRad(sailDeg);
        float boatRad = DegToRad(boatDeg);
        // define the original vector
        Vector2 vector = new Vector2(0, -mag * Mathf.Sin(sailRad) * Mathf.Cos(boatRad - sailRad));
        // rotate the vector by phi1 radians around the origin
        Vector2 rotatedVector = Utils.RotateVector(vector, boatRad);
        return rotatedVector;
    }

    private float GetHullWindDeg()
    {
        // 0 -> 360 anti clock
        float hullDeg = RotationToDeg(boat.hullRotation);
        float windDeg = RotationToDeg(wind.rotation);
        float hullWindDeg = hullDeg - windDeg;

        return Utils.Normalize360Range(hullWindDeg);
    }

    private float GetSailWindDeg()
    {
        // 0 -> 360 anti clock
        float sailDeg = RotationToDeg(boat.mastRotation);
        float sailWindDeg = sailDeg + GetHullWindDeg();
        return Utils.Normalize360Range(sailWindDeg);
    }

    private float GetThrustDeg(Vector2 rForce)
    {
        // 0 -> 360 anti clock
        float deg = Mathf.Atan2(rForce.y, rForce.x) * Mathf.Rad2Deg;
        return Utils.Normalize360Range(deg);
    }

    private void DebugForce(string name, Vector2 force)
    {
        float forceDeg = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
        Debug.Log($"{name} = {force} / mag= {force.magnitude} / {forceDeg} degrees");
    }

    private void DrawForce(Vector2 force, Color color)
    {
        float yOffset = 5f;
        float lineDuration = 0.1f;
        float multiplier = 10f;
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(multiplier * force.x, yOffset, multiplier * force.y), color, lineDuration);
    }

    private float RotationToDeg(float rotation)
    {
        return 360f - rotation;
    }

    private float DegToRad(float deg)
    {
        return deg * Mathf.Deg2Rad;
    }
}
