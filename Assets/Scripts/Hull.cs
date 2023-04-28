using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hull : MonoBehaviour
{
    public float rotation = 0f;
    private float rotationSpeed = 30f;
    void Start()
    {

    }

    void Update()
    {
        float r = rotation;
        if (Input.GetKey(KeyCode.Q))
        {
            r -= rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            r += rotationSpeed * Time.deltaTime;
        }

        r = Utils.DegreesTo360Range(r);

        rotation = r;
    }

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0f, rotation, 0f);
    }
}
