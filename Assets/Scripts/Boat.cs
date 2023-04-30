using UnityEngine;

public class Boat : MonoBehaviour
{
    public GameObject hull;
    public GameObject mast;
    public Wind wind;

    public float hullRotation = 0f;
    public float mastRotation = -90;

    private float hullRotationSpeed = 30f;
    private float mastRotationSpeed = 50f;

    // @todo add Mast Winch

    public Vector3 velocity;

    void Update()
    {
        RotateMast();
        RotateHull();
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
        r = Mathf.Clamp(r, 90, 270);
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

        r = Utils.DegreesTo360Range(r);

        hullRotation = r;
    }
}
