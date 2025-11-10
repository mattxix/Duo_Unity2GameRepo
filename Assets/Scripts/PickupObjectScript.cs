using UnityEngine;


public class PickupObjectScript : MonoBehaviour
{

    bool pickedUp = false;
    Rigidbody rb;
    public Transform destinationObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PickUp()
    {
        pickedUp = !pickedUp;
        if (pickedUp)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            transform.position = destinationObject.transform.position;
            transform.parent = destinationObject.parent;
        }
        else
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            transform.parent = null;
        }
    }
}
