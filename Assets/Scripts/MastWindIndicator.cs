using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastWindIndicator : MonoBehaviour
{
    public Wind wind;
    private float direction = 0f;
    void Start()
    {

    }

    void Update()
    {
        direction = Utils.DegreesTo360Range(wind.direction);
    }

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0f, direction, 0f);
    }
}
