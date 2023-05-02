using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailingForces : MonoBehaviour
{
    public Boat boat;
    public Wind wind;

    public float sailForce { get; private set; }
    public float waterForce { get; private set; }
    public float resultForce { get; private set; }

    void Update()
    {
        float magnitude = wind.speed;

        float hullWindDeg = GetHullWindDeg();
        float sailWindDeg = GetSailWindDeg();

        // Calculate Forces
        // v1: sail, v2: water resistance, v3: resultant
        Vector2 v1 = CalculateSailForce(magnitude, sailWindDeg);
        Vector2 v2 = CalculateWaterForce(magnitude, sailWindDeg, hullWindDeg);
        Vector2 v3 = v1 + v2;

        float thrustDeg = GetThrustDeg(v3);

        float thrustToleranceDeg = 1f;
        bool withinTolerance = Mathf.Abs(hullWindDeg - thrustDeg) <= thrustToleranceDeg;

        // rotate forces relative to hull and wind
        float windDeg = ToCircleDeg(boat.hullRotation);
        float forceRelativeDeg = Utils.Normalize360Range(360 - wind.direction);
        float forceRelativeRad = DegToRad(forceRelativeDeg);
        Vector2 rotatedV1 = Utils.RotateVector(v1, forceRelativeRad);
        Vector2 rotatedV2 = Utils.RotateVector(v2, forceRelativeRad);
        Vector2 rotatedV3 = Utils.RotateVector(v3, forceRelativeRad);

        //DebugForce("v1", rotatedV1);
        //DebugForce("v2", rotatedV2);
        //DebugForce("v3", rotatedV3);

        DrawForce(rotatedV1, Color.red);
        DrawForce(rotatedV2, Color.blue);
        DrawForce(rotatedV3, Color.green);
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
        float hullDeg = ToCircleDeg(boat.hullRotation);
        float windDeg = ToCircleDeg(wind.direction);
        float hullWindDeg = hullDeg - windDeg;

        return Utils.Normalize360Range(hullWindDeg);
    }
    private float GetSailWindDeg()
    {
        // 0 -> 360 anti clock
        float sailDeg = ToCircleDeg(boat.mastRotation);
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

    private float ToCircleDeg(float rotation)
    {
        return 360f - rotation;
    }

    private float DegToRad(float deg)
    {
        return deg * Mathf.Deg2Rad;
    }
}
