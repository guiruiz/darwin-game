using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcesTest : MonoBehaviour
{
    void Start()
    {
        float boatAngle = 50; //0 360 
        float sailAngle = 30; // 0 360
        float wind = 10f; // 0 4

        Vector2 v1 = CalculateV1(sailAngle, wind); // force on sail
        Vector2 v2 = CalculateV2(boatAngle, sailAngle, wind); // water reaction
        Vector2 v3 = v1 + v2; // resultant

        Debug.Log("v1= " + v1 + " / mag= " + CalculateMag(v1));
        Debug.Log("v2= " + v2 + " / mag= " + CalculateMag(v2));
        Debug.Log("v3= " + v3 + " / mag= " + CalculateMag(v3));

        float yOffset = 5f;
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(v1.x, yOffset, v1.y), Color.red, 100f);
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(v2.x, yOffset, v2.y), Color.blue, 100f);
        Debug.DrawLine(new Vector3(0f, yOffset, 0f), new Vector3(v3.x, yOffset, v3.y), Color.green, 100f);
    }

    void Update()
    {


    }

    // CurlyPhi = boatAngle
    // Theta = sailAngle
    // mag = wind



    public Vector2 CalculateV1(float sAngle, float w)
    {
        float v1X = w * Mathf.Sin(sAngle);
        float v1Y = 0;

        float thetaRad = -Mathf.PI / 180 * (sAngle + 90); // Convert degrees to radians and apply transformation
        float v1XRotated = v1X * Mathf.Cos(thetaRad) - v1Y * Mathf.Sin(thetaRad); // Apply rotation
        float v1YRotated = v1X * Mathf.Sin(thetaRad) + v1Y * Mathf.Cos(thetaRad);
        float v1XTranslated = v1XRotated + 0; // Apply translation
        float v1YTranslated = v1YRotated + 0;

        return new Vector2(v1XTranslated, v1YTranslated);
    }

    public Vector2 CalculateV2(float bAngle, float sAngle, float w)
    {
        float v2X = 0;
        float v2Y = -w * Mathf.Sin(Mathf.PI / 180 * sAngle) * Mathf.Cos(Mathf.PI / 180 * (bAngle - sAngle)); // Convert degrees to radians and calculate sine and cosine

        float phiRad = Mathf.PI / 180 * bAngle; // Convert degrees to radians for rotation
        float v2XRotated = v2X * Mathf.Cos(phiRad) - v2Y * Mathf.Sin(phiRad); // Apply rotation
        float v2YRotated = v2X * Mathf.Sin(phiRad) + v2Y * Mathf.Cos(phiRad);
        float v2XTranslated = v2XRotated + 0; // Apply translation
        float v2YTranslated = v2YRotated + 0;

        return new Vector2(v2XTranslated, v2YTranslated);
    }

    public float CalculateMag(Vector2 v)
    {
        return Mathf.Sqrt(v.x * v.x + v.y * v.y); // Calculate magnitude

    }

}
