using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailingForces : MonoBehaviour
{
    public Boat boat;
    public Wind wind;
    public bool logForces = true;
    public bool drawForces = true;

    public Vector2 sailForce { get; private set; }
    public Vector2 waterForce { get; private set; }
    public Vector2 resultForce { get; private set; }
    public float resultDirection { get; private set; } = 0;

    void Update()
    {
        float magnitude = wind.speed;
        float hullWindDeg = Utils.GetHullWindDeg(boat, wind);
        float sailWindDeg = Utils.GetSailWindDeg(boat, wind);

        // calculate Forces
        // v1: sail, v2: water resistance, v3: resultant
        Vector2 v1 = CalculateSailForce(magnitude, sailWindDeg);
        Vector2 v2 = CalculateWaterForce(magnitude, sailWindDeg, hullWindDeg);
        Vector2 v3 = v1 + v2;

        // calculate thurst direction  
        float thrustDeg = GetThrustDeg(v3);
        float thrustToleranceDeg = 1f;
        bool withinTolerance = Mathf.Abs(hullWindDeg - thrustDeg) <= thrustToleranceDeg;

        // rotate coords from unit circle to world
        float windDeg = Utils.RotationToDeg(wind.rotation);
        float windRad = Utils.DegToRad(windDeg);
        Vector2 rotatedV1 = Utils.RotateVector(v1, windRad);
        Vector2 rotatedV2 = Utils.RotateVector(v2, windRad);
        Vector2 rotatedV3 = Utils.RotateVector(v3, windRad);

        sailForce = rotatedV1;
        waterForce = rotatedV2;
        resultForce = rotatedV3;
        resultDirection = withinTolerance ? 1 : -1;

        if (logForces)
        {
            Utils.LogForce("v1", rotatedV1);
            Utils.LogForce("v2", rotatedV2);
            Utils.LogForce("v3", rotatedV3);
        }

        if (drawForces)
        {
            Vector3 boatPos = boat.transform.position;
            Utils.DrawForce(boatPos, rotatedV1, Color.red);
            Utils.DrawForce(boatPos, rotatedV2, Color.blue);
            Utils.DrawForce(boatPos, rotatedV3, Color.green);
        }
    }

    Vector2 CalculateSailForce(float mag, float sailDeg)
    {
        float sailRad = Utils.DegToRad(sailDeg);
        // define the original vector
        Vector2 vector = new Vector2(mag * Mathf.Sin(sailRad), 0);
        // rotate the vector by theta1 + 90 degrees in radians
        Vector2 rotatedVector = Utils.RotateVector(vector, sailRad + Mathf.PI / 2);
        return rotatedVector;
    }

    Vector2 CalculateWaterForce(float mag, float sailDeg, float boatDeg)
    {
        float sailRad = Utils.DegToRad(sailDeg);
        float boatRad = Utils.DegToRad(boatDeg);
        // define the original vector
        Vector2 vector = new Vector2(0, -mag * Mathf.Sin(sailRad) * Mathf.Cos(boatRad - sailRad));
        // rotate the vector by phi1 radians around the origin
        Vector2 rotatedVector = Utils.RotateVector(vector, boatRad);
        return rotatedVector;
    }

    private float GetThrustDeg(Vector2 rForce)
    {
        // 0 -> 360 anti clock
        float deg = Mathf.Atan2(rForce.y, rForce.x) * Mathf.Rad2Deg;
        return Utils.Normalize360Range(deg);
    }
}
