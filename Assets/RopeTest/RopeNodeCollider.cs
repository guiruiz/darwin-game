using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeNodeCollider : MonoBehaviour
{
    void Update()
    {
    }

    void OnTriggerEnter(Collider collider)
    {
        // Check if the collision involves a rigidbody
        Rigidbody otherRigidbody = collider.attachedRigidbody;
        if (otherRigidbody != null)
        {
            // Move this rigidbody to a non-overlapping position
            Vector3 nonOverlappingPosition = CalculateNonOverlappingPosition(otherRigidbody);
            GetComponent<Rigidbody>().MovePosition(nonOverlappingPosition);
        }
    }


    // void OnTriggerStay(Collider collider)
    // {
    //     // Check if the collision involves a rigidbody
    //     Rigidbody otherRigidbody = collider.attachedRigidbody;
    //     if (otherRigidbody != null)
    //     {
    //         // Move this rigidbody to a non-overlapping position
    //         Vector3 nonOverlappingPosition = CalculateNonOverlappingPosition(otherRigidbody);
    //         GetComponent<Rigidbody>().MovePosition(nonOverlappingPosition);
    //     }
    // }

    Vector3 CalculateNonOverlappingPosition(Rigidbody otherRigidbody)
    {
        // Calculate the direction to move this rigidbody away from the other rigidbody
        Vector3 direction = Vector3.up;
        direction.Normalize();

        // Calculate the minimum distance required to move this rigidbody away from the other rigidbody
        float distance = (GetComponent<Collider>().bounds.extents.magnitude + otherRigidbody.GetComponent<Collider>().bounds.extents.magnitude) * 1.1f;
        //float distance = 0.01f;
        // Calculate the non-overlapping position
        Vector3 nonOverlappingPosition = otherRigidbody.transform.position + (direction * distance);

        return nonOverlappingPosition;
    }
}
