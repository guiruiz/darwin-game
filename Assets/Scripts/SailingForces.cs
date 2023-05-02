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

        // Calculate Forces
        // v1: sail, v2: water resistance, v3: resultant
        Vector2 v1 = CalculateSailLift(magnitude, sailWindAngle);
        Vector2 v2 = CalculateWaterResistance(magnitude, sailWindAngle, hullWindAngle);
        Vector2 v3 = v1 + v2;

        float thrustAngle = GetThrustCircleAngle(v3);

        // rotate forces relative to hull and wind
        float forceRelativeAngle = Utils.Normalize360Range(360 - hullWindAngle - boat.hullRotation);
        float forceRelativeRadians = forceRelativeAngle * Mathf.Deg2Rad;

        Vector2 rotatedV1 = Utils.RotateVector(v1, forceRelativeRadians);
        Vector2 rotatedV2 = Utils.RotateVector(v2, forceRelativeRadians);
        Vector2 rotatedV3 = Utils.RotateVector(v3, forceRelativeRadians);

        float tolerance = 1f;
        bool withinTolerance = Mathf.Abs(hullWindAngle - thrustAngle) <= tolerance;

        //DebugForce(rotatedV1);
        //DebugForce(rotatedV2);
        //DebugForce(rotatedV3);

        DrawForce(rotatedV1, Color.red);
        DrawForce(rotatedV2, Color.blue);
        DrawForce(rotatedV3, Color.green);
    }

    Vector2 CalculateSailLift(float mag, float sailDegrees)
    {
        float sailRadians = sailDegrees * Mathf.Deg2Rad;
        // define the original vector
        Vector2 vector = new Vector2(mag * Mathf.Sin(sailRadians), 0);
        // rotate the vector by theta1 + 90 degrees in radians
        Vector2 rotatedVector = Utils.RotateVector(vector, sailRadians + Mathf.PI / 2);
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
        return Utils.Normalize360Range(angle);
    }

    public void DebugForce(Vector2 force)
    {
        float forceAngle = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
        Debug.Log($"sailForce= {force} / mag= {force.magnitude} / {forceAngle} degrees");
    }

    public void DrawForce(Vector2 force, Color color)
    {
        float yOffset = 5f;
        float lineDuration = 0.1f;
        float multiplier = 10f;
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(multiplier * force.x, yOffset, multiplier * force.y), color, lineDuration);

    }
}
