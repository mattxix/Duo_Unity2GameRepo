using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public enum RotationAxis
    {
        MouseXAndMouseY = 0,
        MouseX = 1,
        MouseY = 2

    }

    public RotationAxis axis = RotationAxis.MouseXAndMouseY;
    public float sensitivityX = 10.0f;
    public float sensitivityY = 10.0f;
    public float maxVerticalRotation = 45.0f;
    public float minVerticalRotation = -45.0f;
    float mouseX;
    float mouseY;
    float verticalRotation = 0;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (axis == RotationAxis.MouseX)
        {
            // just x rot
            transform.Rotate(0, mouseX * sensitivityX * Time.deltaTime, 0);
        }
        else if (axis == RotationAxis.MouseY)
        {
            // just y rot
            verticalRotation -= mouseY * sensitivityY * Time.deltaTime;
            verticalRotation = Mathf.Clamp(verticalRotation, minVerticalRotation, maxVerticalRotation);
            float horizontalRotation = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(verticalRotation, horizontalRotation, 0);
        }
        else
        {
            // both x and y rot
            verticalRotation -= mouseY * sensitivityY * Time.deltaTime;
            verticalRotation = Mathf.Clamp(verticalRotation, minVerticalRotation, maxVerticalRotation);

            float deltaRotation = mouseX * sensitivityX * Time.deltaTime;
            float horizontalRotation = transform.localEulerAngles.y + deltaRotation;
            transform.localEulerAngles = new Vector3(verticalRotation, horizontalRotation, 0);

        }
    }

    public void LookValues(InputAction.CallbackContext ctx)
    {
        //Debug.Log(ctx.ReadValue<Vector2>());
        mouseX = ctx.ReadValue<Vector2>().x;
        mouseY = ctx.ReadValue<Vector2>().y;
    }


}

