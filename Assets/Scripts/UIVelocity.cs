using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVelocity : MonoBehaviour
{
    public Boat boat;
    public Wind wind;
    private float direction = 0f;

    void Start()
    {

    }

    void Update()
    {

        // all WIP
        float angleRad = Mathf.Atan2(boat.velocity.x, boat.velocity.z);

        float angleDeg = angleRad * Mathf.Rad2Deg;

        if (angleDeg < 0)
        {
            angleDeg += 360;
        }


        //direction = angleDeg;

        SailForces();
        Foo();
    }

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0f, direction, 0f);
    }

    float Foo()
    {
        float mastAngleRad = boat.mastRotation * (Mathf.PI / 180.0f);
        float windDirectionRad = wind.direction * (Mathf.PI / 180.0f);

        // Calculate vector components of wind force
        float windForceX = Mathf.Cos(windDirectionRad);
        float windForceY = Mathf.Sin(windDirectionRad);

        // Calculate vector components of sail force
        float sailForceX = Mathf.Sin(mastAngleRad);
        float sailForceY = Mathf.Cos(mastAngleRad);

        // Calculate resultant force components
        float resultantForceX = windForceX + sailForceX;
        float resultantForceY = windForceY + sailForceY;

        // Calculate resultant force direction in radians
        float resultantForceDirectionRad = Mathf.Atan2(resultantForceX, resultantForceY);

        // Convert resultant force direction from radians to degrees
        float resultantForceDirectionDeg = resultantForceDirectionRad * (180.0f / Mathf.PI);


        return resultantForceDirectionDeg;
    }
    public float sailForce = 10f; // Magnitude of the sail force applied to the sailboat
    public float sailForceMultiplier = 1f; // Multiplier to adjust the sail force

    void SailForces()
    {
        // Calculate the sail force based on the mast rotation angle and wind direction
        float angleDifference = wind.direction - boat.mastRotation;
        float horizontalForce = sailForce * sailForceMultiplier * Mathf.Cos(angleDifference * Mathf.Deg2Rad);
        float verticalForce = sailForce * sailForceMultiplier * Mathf.Sin(angleDifference * Mathf.Deg2Rad);

        boat.velocity = new Vector3(horizontalForce, 0f, verticalForce);

        // Apply the calculated sail force to the sailboat's rigidbody
        //sailboatRigidbody.AddForce(new Vector2(horizontalForce, verticalForce));
    }
}
