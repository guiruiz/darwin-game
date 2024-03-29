using System;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float rotation = 0f;
    public float speed = 2f;
    private float rotationSpeed = 30f;
    void Start()
    {
    }

    void Update()
    {
        float r = rotation;
        if (Input.GetKey(KeyCode.Z))
        {
            r -= rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            r += rotationSpeed * Time.deltaTime;
        }

        r = Utils.Normalize360Range(r);

        rotation = r;
    }

}
