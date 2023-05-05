using UnityEngine;

public class Boat : MonoBehaviour
{
    public GameObject hull;
    public GameObject mast;
    public Wind wind;
    public SailingForces sailingForces;

    public float hullRotation = 0f;
    public float mastRotation = -90;
    public float mastWinch = 90f;
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
        RotateMast();
        RotateHull();
        MastWinchControl();

        mastWinch = Mathf.Clamp(mastWinch, 0, 90);
        mastRotation = Mathf.Clamp(mastRotation, 180 - mastWinch, 180 + mastWinch);
        //if (wind.direction < 10 && wind.direction > 350) { Debug.Log("tacking"); }
        //else if (wind.direction > 175 && wind.direction < 185) { Debug.Log("gybing"); }
    }

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0f, hullRotation, 0f);
        mast.transform.localEulerAngles = new Vector3(0f, mastRotation, 0f);
        MoveBoat();
    }

    void RotateMast()
    {
        float r = mastRotation;
        if (Input.GetKey(KeyCode.Q))
        {
            r -= mastRotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
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
        Vector3 direction = (sailingForces.resultDirection > 0) ? boatFwd : -boatFwd;

        if (Input.GetKey(KeyCode.L))
        {
        }
        Debug.Log($"vel: {rb.velocity.magnitude} / rdir {sailingForces.resultDirection} / addForce {resultant} / dir {direction}");


        float maxSpeed = 2f;
        if (rb.velocity.magnitude < maxSpeed && moveBoat)
        {
            rb.AddForce(resultant);
        }


        Utils.DrawForce(transform.position, Utils.Vector3To2(resultant), Color.magenta);
    }

    public float GetSpeed()
    {
        return rb.velocity.magnitude;
    }
}
