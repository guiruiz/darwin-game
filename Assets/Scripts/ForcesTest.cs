using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcesTest : MonoBehaviour
{
    float boatAngle = 0; //0 360 
    float sailAngle = 30; // 0 360
    float wind = 2f; // 0 4

    // CurlyPhi = boatAngle
    // Theta = sailAngle
    // mag = wind

    // v1 force on sail
    // v2 water reaction
    // v3 resultant force

    void Start()
    {


        Vector2 v1 = CalculateV1(wind, sailAngle);
        Vector2 v2 = CalculateV2(wind, sailAngle, boatAngle);
        Vector2 v3 = v1 + v2;

        float v1Angle = Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg;
        float v2Angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
        float v3Angle = Mathf.Atan2(v3.y, v3.x) * Mathf.Rad2Deg;

        Debug.Log("sailF  v1= " + v1 + " / mag= " + v1.magnitude + " //// " + v1Angle + " degrees");
        Debug.Log("waterR v2= " + v2 + " / mag= " + v2.magnitude + " //// " + v2Angle + " degrees");
        Debug.Log("Result v3= " + v3 + " / mag= " + v3.magnitude + " //// " + v3Angle + " degrees");

        float yOffset = 5f;
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(10 * v1.x, yOffset, 10 * v1.y), Color.red, 10000f);
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(10 * v2.x, yOffset, 10 * v2.y), Color.blue, 10000f);
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(10 * v3.x, yOffset, 10 * v3.y), Color.green, 10000f);

        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(0f, yOffset, 100f), Color.magenta, 10000f);
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(100f, yOffset, 0f), Color.magenta, 10000f);
    }

    void Update()
    {
    }

    void OnGUI()
    {
        GUI.Label(new Rect(25, 0, 200, 40), "Boat Angle " + boatAngle);
        GUI.Label(new Rect(25, 20, 200, 40), "Sail Angle " + sailAngle);
        GUI.Label(new Rect(25, 40, 200, 40), "Wind " + wind);
    }

    Vector2 CalculateV1(float mag, float theta1_degrees)
    {
        float theta1_radians = theta1_degrees * Mathf.Deg2Rad; // convert the angle from degrees to radians
        Vector2 vector = new Vector2(mag * Mathf.Sin(theta1_radians), 0); // define the original vector
        Vector2 rotatedVector = RotateVector(vector, theta1_radians + Mathf.PI / 2); // rotate the vector by theta1 + 90 degrees in radians
        Debug.Log($"Original vector: {vector}, Rotated vector: {rotatedVector}");
        return rotatedVector;
    }

    Vector2 CalculateV2(float mag, float theta1_degrees, float phi1_degrees)
    {
        float theta1_radians = theta1_degrees * Mathf.Deg2Rad; // convert the angle from degrees to radians
        float phi1_radians = phi1_degrees * Mathf.Deg2Rad; // convert the angle from degrees to radians

        Vector2 vector = new Vector2(0, -mag * Mathf.Sin(theta1_radians) * Mathf.Cos(phi1_radians - theta1_radians)); // define the original vector
        Vector2 rotatedVector = RotateVector(vector, phi1_radians); // rotate the vector by phi1 radians around the origin
        Debug.Log($"Original vector: {vector}, Rotated vector: {rotatedVector}");
        return rotatedVector;
    }

    public static Vector2 RotateVector(Vector2 vector, float angle)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg), Vector3.one); // create a rotation matrix
        return rotationMatrix.MultiplyVector(vector); // apply the rotation matrix to the vector and return the result
    }

}
