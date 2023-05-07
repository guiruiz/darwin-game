using UnityEngine;

public class SpawnRopeWithCharacterJoint : MonoBehaviour
{
    public GameObject ropePrefab; // The prefab object containing the rope and CharacterJoint component
    public Transform startPos; // The starting position for the rope
    public Transform endPos; // The ending position for the rope
    public int numSegments = 10; // The number of segments to divide the rope into
    public float segmentLength = 1f; // The length of each segment
    public float jointSpring = 20f; // The spring force of each joint
    public float jointDamper = 0.5f; // The damping force of each joint

    void Start()
    {
        // Calculate the direction and distance between the start and end positions
        Vector3 direction = (endPos.position - startPos.position).normalized;
        float distance = Vector3.Distance(startPos.position, endPos.position);

        // Instantiate the rope prefab
        GameObject rope = Instantiate(ropePrefab, startPos.position, Quaternion.identity);

        // Calculate the segment size based on the desired number of segments and segment length
        float segmentSize = distance / numSegments;

        // Loop through each segment and position it correctly
        for (int i = 1; i < numSegments; i++)
        {
            // Calculate the position of the current segment based on the segment size and direction
            Vector3 segmentPos = startPos.position + (direction * (segmentSize * i));

            // Instantiate a new segment from the rope prefab and position it at the calculated position
            GameObject segment = Instantiate(ropePrefab, segmentPos, Quaternion.identity);

            // Connect the new segment to the previous segment with a CharacterJoint component
            CharacterJoint joint = segment.GetComponent<CharacterJoint>();
            joint.connectedBody = rope.GetComponent<Rigidbody>();
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = Vector3.zero;
            joint.connectedAnchor = Vector3.up * segmentLength * -0.5f;

            // Set the parent of the new segment to the previous segment
            segment.transform.SetParent(rope.transform.GetChild(i - 1), true);
        }

        // Connect the last segment to the end position with a CharacterJoint component
        CharacterJoint endJoint = rope.transform.GetChild(numSegments - 1).GetComponent<CharacterJoint>();
        endJoint.connectedBody = endPos.GetComponent<Rigidbody>();
        endJoint.autoConfigureConnectedAnchor = false;
        endJoint.anchor = Vector3.zero;
        endJoint.connectedAnchor = Vector3.up * segmentLength * -0.5f;
    }
}
