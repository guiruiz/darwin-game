using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public Transform mastTransform;
    public Wind wind;
    public float mastSpeed = 50f;
    public float mastRotation = 180f;
    public Vector3 velocity;

    void Start()
    {

    }

    void Update()
    {
        RotateMast();

        //if (wind.direction < 10 && wind.direction > 350) { Debug.Log("tacking"); }
        //else if (wind.direction > 175 && wind.direction < 185) { Debug.Log("gybing"); }
    }


    void FixedUpdate()
    {
        mastTransform.eulerAngles = new Vector3(0f, mastRotation, 0f);
    }

    void RotateMast()
    {
        float r = mastRotation;
        if (Input.GetKey(KeyCode.D))
        {
            r -= mastSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            r += mastSpeed * Time.deltaTime;
        }
        r = Mathf.Clamp(r, 90, 270);
        mastRotation = r;
    }


}
