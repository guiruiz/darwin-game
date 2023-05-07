using UnityEngine;

public class Boat : MonoBehaviour
{
    public GameObject hull;
    public GameObject mast;
    public Wind wind;
    public SailingForces sailingForces;

    public float hullRotation = 0f;
    public float mastRotation = 180f;
    public float mastWinch = 90f;
    public float maxSpeed = 2f;
    public float speedMultiplier = 2f;
    public float sideDragFactor = 0.1f;

    public bool moveBoat = false;
    private float hullRotationSpeed = 30f;
    private float mastRotationSpeed = 50f;
    private float mastWinchSpeed = 50f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // RotateMast();
        RotateHull();
        MastWinchControl();

        mastWinch = Mathf.Clamp(mastWinch, 0, 90);
        mastRotation = Mathf.Clamp(mastRotation, 180 - mastWinch, 180 + mastWinch);

        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;

        //if (wind.direction < 10 && wind.direction > 350) { Debug.Log("tacking"); }
        //else if (wind.direction > 175 && wind.direction < 185) { Debug.Log("gybing"); }
    }

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0f, hullRotation, 0f);
        mast.transform.localEulerAngles = new Vector3(0f, mastRotation, 0f);
        MoveBoat();
        SideDrag();
    }

    void RotateMast()
    {
        float r = mastRotation;
        if (Input.GetKey(KeyCode.E))
        {
            r -= mastRotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            r += mastRotationSpeed * Time.deltaTime;
        }
        mastRotation = r;
    }

    void RotateHull()
    {
        float r = hullRotation;
        if (Input.GetKey(KeyCode.A))
        {
            r -= hullRotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            r += hullRotationSpeed * Time.deltaTime;
        }

        r = Utils.Normalize360Range(r);

        hullRotation = r;
    }

    void MastWinchControl()
    {
        float w = mastWinch;
        if (Input.GetKey(KeyCode.S))
        {
            w -= mastWinchSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            w += mastWinchSpeed * Time.deltaTime;
        }
        mastWinch = w;
    }

    void MoveBoat()
    {
        Vector3 boatFwd = transform.forward;
        Vector3 resultant = Utils.Vector2To3(sailingForces.resultForce);

        if (rb.velocity.magnitude < maxSpeed && moveBoat)
        {
            float m = (sailingForces.resultDirection > 0) ? speedMultiplier : speedMultiplier / 2;
            Vector3 f = resultant * m;
            rb.AddForce(f, ForceMode.Acceleration);
        }

        Utils.DrawForce(transform.position, Utils.Vector3To2(resultant), Color.green);
        //Utils.DrawForce(transform.position, Utils.Vector3To2(rb.velocity), Color.gray);
    }

    void SideDrag()
    {
        // Calculate the dot product of the velocity and the forward vector of the transform
        float dotProduct = Vector3.Dot(rb.velocity, transform.forward);

        // If the dot product is greater than zero, the Rigidbody is moving forward, so decrease its forward velocity
        if (Mathf.Abs(dotProduct) > 0.2f)
        {
            Vector3 forwardVelocity = transform.forward * dotProduct;
            Vector3 oppositeForce = -forwardVelocity * sideDragFactor;
            rb.AddForce(oppositeForce, ForceMode.Impulse);

            Utils.DrawForce(transform.position, Utils.Vector3To2(oppositeForce * 10), Color.red);
        }
    }

    public float GetSpeed()
    {
        return rb.velocity.magnitude;
    }

    public void RotateMastToAngle(float targetRotation, float rotationSpeed)
    {
        mastRotation = Mathf.Lerp(mastRotation, targetRotation, rotationSpeed * Time.deltaTime); ;
    }
}
