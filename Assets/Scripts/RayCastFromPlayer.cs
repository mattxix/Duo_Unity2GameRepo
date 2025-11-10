using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;



public class RayCastFromPlayer : MonoBehaviour
{

    public float raycastDistance = 5.0f;
    bool holdingItem = false;
    GameObject heldObject;

    [Header("Room1")]
    public GameObject doorButton1;
    public GameObject puzzleDoor1;
    bool wireCuttersInInventory = false;
    bool wiresCut = false;
    bool KeyCard1InInventory = false;
    bool door1Unlocked = false;

    [Header("Room2")]
    public GameObject doorButton2;
    public GameObject puzzleDoor2;
    //bool door2Unlocked = false;
    bool KeyCard2InInventory = false;

    [Header("Room3")]
    //public GameObject doorButton3;
    public GameObject puzzleDoor3;
    public bool Cube;
    public bool Cylinder;
    public bool Prism;
    //bool door3Unlocked = false;

    [Header("Core")]
    public GameObject C4;
    public ExplosiveTimer ExplosiveTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green);
        if (wiresCut && KeyCard1InInventory)
        {
            doorButton1.GetComponent<Renderer>().material.color = Color.green;
            door1Unlocked = true;
        }
        else
        {
            doorButton1.GetComponent<Renderer>().material.color = Color.red;
            door1Unlocked = false;
        }

        if (KeyCard2InInventory)
        {
            doorButton2.GetComponent<Renderer>().material.color = Color.green;
            //door2Unlocked = true;
        }
        else
        {
            doorButton2.GetComponent<Renderer>().material.color = Color.red;
            //door2Unlocked = false;
        }


        if (Cube && Prism && Cylinder)
        {
            //doorButton3.GetComponent<Renderer>().material.color = Color.green;
            //door3Unlocked = true;
            puzzleDoor3.SetActive(false);
        }
        else
        {
            //doorButton3.GetComponent<Renderer>().material.color = Color.red;
            //door3Unlocked = false;
            puzzleDoor3.SetActive(true);
        }
    }

    public void WiresAreCut()
    {
        wiresCut = true;
    }

    public void HaveWireCutters()
    {
        wireCuttersInInventory = true;
    }
    public void HaveKeyCard1()
    {
        KeyCard1InInventory = true;
    }
    public void HaveKeyCard2()
    {
        KeyCard2InInventory = true;
    }

    public void PickUpItem(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("PickupItem"))
                {
                    PickupObjectScript pickup = hit.collider.GetComponent<PickupObjectScript>();

                    if (pickup != null)
                    {
                        pickup.PickUp();
                        heldObject = hit.collider.gameObject;
                        holdingItem = true;
                    }
                }
            }
        }

        // DROP
        if (ctx.canceled)
        {
            if (holdingItem && heldObject != null)
            {
                heldObject.GetComponent<PickupObjectScript>().PickUp();
                holdingItem = false;
                heldObject = null;
            }
        }
    }


    public void interactableObject(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("DoorButton") && door1Unlocked && KeyCard1InInventory)
                {
                    puzzleDoor1.SetActive(false);

                }
                else if (hit.collider.CompareTag("WirePanel") && wireCuttersInInventory )
                {
                    WiresAreCut();

                }
                else if (hit.collider.CompareTag("DoorButton2") && KeyCard2InInventory)
                {
                    puzzleDoor2.SetActive(false);
                }
                else if(hit.collider.CompareTag("C4Location"))
                {
                    C4.SetActive(true);
                    ExplosiveTimer.StartExplosionTimer();
                    Debug.Log("c4Planted");
                }

            }

        }
    }

}
