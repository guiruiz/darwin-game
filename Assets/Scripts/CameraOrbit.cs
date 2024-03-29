using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 3f;

    private float rotationY = 270;
    private float rotationX = 15;
    public Transform target;
    [SerializeField]
    private float distanceFromTarget = 150f;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;

    [SerializeField]
    private float defaultSmoothTime = 0.2f;
    [SerializeField]
    private Vector2 zoomMinMax = new Vector2(30, 300);
    [SerializeField]
    private Vector2 rotationXMinMax = new Vector2(-30, 30);
    void Start()
    {
        OrbitCamera(0);

    }
    void Update()
    {
        OrbitCamera(defaultSmoothTime);
        ZoomCamera();
    }


    void OrbitCamera(float smoothTime)
    {

        float mouseX = 0;
        float mouseY = 0;

        if (Input.GetMouseButton(1))
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        }

        rotationY += mouseX;
        rotationX += mouseY;

        //Apply clamping for x rotation
        rotationX = Mathf.Clamp(rotationX, rotationXMinMax.x, rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(rotationX, rotationY);

        // Apply damping between rotation changes
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = target.position - transform.forward * distanceFromTarget;
    }

    void ZoomCamera()
    {

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = distanceFromTarget + mouseScroll * 50 * -1;
        distanceFromTarget = Mathf.Clamp(newZoom, zoomMinMax.x, zoomMinMax.y);
    }
}