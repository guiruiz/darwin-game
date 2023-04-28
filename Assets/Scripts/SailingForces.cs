using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailingForces : MonoBehaviour
{
    public Boat boat;
    public Wind wind;
    void Update()
    {
        float boatAngle = 360f - boat.hullRotation; // 0 -> 360 anti clock
        float sailAngle = boat.mastRotation * -1; // 0 -> 360 anti clock
        Debug.Log("boatAngle= " + boatAngle + " | sailAngle= " + sailAngle);

        // v1 sail lift
        Vector2 v1 = CalculateSailLift(wind.speed, sailAngle);
        // v2 water resitance
        Vector2 v2 = CalculateWaterResistance(wind.speed, sailAngle, boatAngle);
        // v3 resultant force
        Vector2 v3 = v1 + v2;

        float v1Angle = Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg;
        float v2Angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
        float v3Angle = Mathf.Atan2(v3.y, v3.x) * Mathf.Rad2Deg;

        Debug.Log("v1= " + v1 + " / mag= " + v1.magnitude + " //// " + v1Angle + " degrees");
        Debug.Log("v2= " + v2 + " / mag= " + v2.magnitude + " //// " + v2Angle + " degrees");
        Debug.Log("v3= " + v3 + " / mag= " + v3.magnitude + " //// " + v3Angle + " degrees");

        float yOffset = 5f;
        float lineDuration = 0.1f;
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(10 * v1.x, yOffset, 10 * v1.y), Color.red, lineDuration);
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(10 * v2.x, yOffset, 10 * v2.y), Color.blue, lineDuration);
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(10 * v3.x, yOffset, 10 * v3.y), Color.green, lineDuration);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(25, 0, 200, 40), "Wind Angle" + wind.direction);
        GUI.Label(new Rect(25, 40, 200, 40), "Wind Speed " + wind.speed);
        GUI.Label(new Rect(25, 20, 200, 40), "Sail Angle " + boat.mastRotation);
    }

    Vector2 CalculateSailLift(float mag, float sailDegrees)
    {
        float sailRadians = sailDegrees * Mathf.Deg2Rad;
        // define the original vector
        Vector2 vector = new Vector2(mag * Mathf.Sin(sailRadians), 0);
        // rotate the vector by theta1 + 90 degrees in radians
        Vector2 rotatedVector = RotateVector(vector, sailRadians + Mathf.PI / 2);
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
        Vector2 rotatedVector = RotateVector(vector, boatRadians);
        //Debug.Log($"V2 Original vector: {vector}, Rotated vector: {rotatedVector}");
        return rotatedVector;
    }

    public static Vector2 RotateVector(Vector2 vector, float angle)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg), Vector3.one); // create a rotation matrix
        return rotationMatrix.MultiplyVector(vector); // apply the rotation matrix to the vector and return the result
    }
}
