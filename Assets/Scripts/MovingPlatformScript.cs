using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    private Vector3 lastPos;
    private Vector3 deltaPos;
    private bool playerOnPlatform = false;
    private Transform player;

    private void Start()
    {
        lastPos = transform.position;
    }

    private void Update()
    {
        // How much the platform moved this frame
        deltaPos = transform.position - lastPos;
        lastPos = transform.position;

        // Apply that movement to the player
        if (playerOnPlatform && player != null)
        {
            player.transform.position += deltaPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ON");
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = false;
            player = null;
        }
    }
}
