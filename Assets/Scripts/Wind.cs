using System;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float direction = 0f;
    public float speed = 2f;
    private float rotationSpeed = 30f;
    void Start()
    {
    }

    void Update()
    {
        // float r = direction;
        // if (Input.GetKey(KeyCode.Q))
        // {
        //     r -= rotationSpeed * Time.deltaTime;
        // }
        // else if (Input.GetKey(KeyCode.E))
        // {
        //     r += rotationSpeed * Time.deltaTime;
        // }

        // r = Utils.DegreesTo360Range(r);

        // direction = r;
    }

}
