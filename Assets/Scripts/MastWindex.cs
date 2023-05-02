using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastWindex : MonoBehaviour
{
    public Wind wind;
    private float direction = 0f;
    void Start()
    {

    }

    void Update()
    {
        direction = Utils.Normalize360Range(wind.direction);
    }

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0f, direction, 0f);
    }
}
