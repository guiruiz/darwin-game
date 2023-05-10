using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailSheet : MonoBehaviour
{
    public Wind wind;
    public Boat boat;
    public GameObject sheetGameObject;
    public SailingForces sailingForces;

    readonly float stretchConstant = .34f;
    float force = 0;
    float flapSpeed = 2f;
    float flapAmp = .05f;
    float flapFreq = 10f;

    Material sheetMaterial;

    float minForce = -1f;
    float maxForce = 1f;
    float minSpeed = 3f;
    float maxSpeed = 4f;
    float minAmp = 0.02f;
    float maxAmp = 0.04f;

    void Start()
    {
        sheetMaterial = sheetGameObject.GetComponent<Renderer>().sharedMaterial;
    }

    void Update()
    {
        Vector3 resultant = Utils.Vector2To3(sailingForces.resultForce);
        float magnitude = resultant.magnitude / wind.speed;

        CalculateForce(resultant, magnitude);
        CalculateFlap(magnitude);

        SetShaderProperties();
    }

    void CalculateForce(Vector3 resultant, float mag)
    {
        float realativeWindDeg = Utils.GetSailWindDeg(boat, wind);
        float relativeWindRotation = Utils.DegToRotation(realativeWindDeg);

        if (relativeWindRotation > 180)
        {
            mag *= -1;
        }

        force = Mathf.Clamp(mag, minForce, maxForce);
    }

    void CalculateFlap(float mag)
    {
        flapSpeed = Mathf.Clamp(maxSpeed - (maxSpeed * mag), minSpeed, maxSpeed);

        flapAmp = Mathf.Clamp(maxAmp - (maxAmp * mag), minAmp, maxAmp);

        flapFreq = Mathf.Clamp(10, 0, 10);
    }

    void SetShaderProperties()
    {
        sheetMaterial.SetFloat("_Stretch_Constant", stretchConstant);
        sheetMaterial.SetFloat("_Force", force);
        sheetMaterial.SetFloat("_Flap_Speed", flapSpeed);
        sheetMaterial.SetFloat("_Flap_Amp", flapAmp);
        sheetMaterial.SetFloat("_Flap_Freq", flapFreq);
    }
}
