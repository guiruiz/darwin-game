using UnityEngine;

public class Boat : MonoBehaviour
{
    public GameObject hull;
    public GameObject mast;
    public Wind wind;

    public float hullRotation = 0f;
    public float mastRotation = -90;
    public float mastWinch = 90f;

    private float hullRotationSpeed = 30f;
    private float mastRotationSpeed = 50f;
    private float mastWinchSpeed = 50f;

    // @todo add Mast Winch

    public Vector3 velocity;

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
    }

    void RotateMast()
    {
        float r = mastRotation;
        if (Input.GetKey(KeyCode.D))
        {
            r -= mastRotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            r += mastRotationSpeed * Time.deltaTime;
        }
        mastRotation = r;
    }

    void RotateHull()
    {
        float r = hullRotation;
        if (Input.GetKey(KeyCode.Q))
        {
            r -= hullRotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
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
}
