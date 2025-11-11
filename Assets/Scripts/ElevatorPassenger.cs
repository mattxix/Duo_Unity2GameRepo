using UnityEngine;

public class ElevatorPassenger : MonoBehaviour
{
    private Transform player;
    private CharacterController cc;
    private Rigidbody rb;

    private Vector3 lastElevatorPos;
    private bool onElevator = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lastElevatorPos = rb.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            cc = player.GetComponent<CharacterController>();
            onElevator = true;
            lastElevatorPos = rb.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onElevator = false;
            player = null;
            cc = null;
        }
    }

    private void FixedUpdate()
    {
        if (onElevator && player != null && cc != null)
        {
            // Calculate how far the elevator moved this physics frame
            Vector3 elevatorDelta = rb.position - lastElevatorPos;

            // Move the player by the same amount
            cc.Move(elevatorDelta);

            // Update last position
            lastElevatorPos = rb.position;
        }
    }
}
