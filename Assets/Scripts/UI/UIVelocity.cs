using UnityEngine;

public class UIVelocity : MonoBehaviour
{
    private float direction = 0f;

    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0f, direction, 0f);
    }
}
