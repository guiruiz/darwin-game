using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindDirection : MonoBehaviour
{
    public Wind wind;

    void Start()
    {
    }

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0f, wind.rotation, 0f);
    }
}
