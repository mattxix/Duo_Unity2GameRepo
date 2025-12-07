using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using TMPro;
using static InventoryScript;
using static MedallionID;



public class RayCastFromPlayer : MonoBehaviour
{
    public Animator animAlarm;
    public InventoryScript InventoryScript;
    public float raycastDistance = 5.0f;
    bool holdingItem = false;
    public GameObject heldObject;
    public EnemySpawner EnemySpawner;
    public LayerMask medallionLayer;
    public TextMeshProUGUI helpMessage;

    [Header("Room1")]
    public GameObject doorButton1;
    //public GameObject puzzleDoor1;
    public GameObject wireBoxUncut;
    public GameObject wireBoxCut;
    public Light statusLight1;
    public Animator anim1;
    //bool wireCuttersInInventory = false;
    bool wiresCut = false;
    bool KeyCard1InInventory = false;
    bool door1Unlocked = false;

    [Header("Room2")]
    public GameObject doorButton2;
    //public GameObject puzzleDoor2;
    public Light statusLight2;
    public Animator anim2;
    //bool door2Unlocked = false;
    bool KeyCard2InInventory = false;

    [Header("Room3")]
    public Animator anim3;
    //public GameObject doorButton3;
    //public GameObject puzzleDoor3;
    public bool Cube;
    public bool Cylinder;
    public bool Prism;
    //bool door3Unlocked = false;

    [Header("Core")]
    public GameObject C4;
    public ExplosiveTimer ExplosiveTimer;
    public ElevatorScript ElevatorScript;

    [Header("Audio")]
    public AudioClip keycardSwipeClip;
    public AudioClip DoorOpenClip;

    [Range(0f, 1f)]
    public float keycardSwipeVolume = 2f;
    public float DoorOpenVolume = 2f;


    [HideInInspector]
    public bool accessGrantedPlayed = false;

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
            statusLight1.color = Color.green;
            //doorButton1.GetComponent<Renderer>().material.color = Color.green;
            door1Unlocked = true;
        }
        else
        {
            statusLight1.color = Color.red;
            //doorButton1.GetComponent<Renderer>().material.color = Color.red;
            door1Unlocked = false;
        }

        if (KeyCard2InInventory)
        {
            statusLight2.color = Color.green;
            //doorButton2.GetComponent<Renderer>().material.color = Color.green;
            //door2Unlocked = true;
        }
        else
        {
            statusLight2.color = Color.red;
            //doorButton2.GetComponent<Renderer>().material.color = Color.red;
            //door2Unlocked = false;
        }


        if (Cube && Prism && Cylinder)
        {
            //doorButton3.GetComponent<Renderer>().material.color = Color.green;
            //door3Unlocked = true;
            //puzzleDoor3.SetActive(false);
            anim3.SetTrigger("OpenDoor");
        }
        else
        {
            //doorButton3.GetComponent<Renderer>().material.color = Color.red;
            //door3Unlocked = false;
            //puzzleDoor3.SetActive(true);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, medallionLayer))
        {
            // We are looking at a medallion
            MedallionID medallion = hit.collider.GetComponentInParent<MedallionID>();
            if (medallion != null)
            {
                OnLookAtMedallion();
            }
        }
        else
        {
            helpMessage.text = "";
        }
    }
    void OnLookAtMedallion()
    {
        if(InventoryScript.currentSlot == 0 && !holdingItem)
        {
            helpMessage.text = "left click to pickup";
        }
        else if (InventoryScript.currentSlot != 0 && !holdingItem)
        {
            helpMessage.text = "switch to open hand";
        }
        

       
    }

    public void WiresAreCut()
    {
        wiresCut = true;
        wireBoxCut.SetActive(true);
        wireBoxUncut.SetActive(false);

        if (animAlarm != null)
        {
            animAlarm.SetBool("AlarmOff", true);
        }
    }

    //public void HaveWireCutters()
    //{
    //    wireCuttersInInventory = true;
    //}
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
            TryPickup();
        }

        if (ctx.canceled)
        {
            DropItem();
        }
    }

    void TryPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance)
            && InventoryScript.currentSlot == 0)
        {
            if (hit.collider.CompareTag("PickupItem"))
            {
                PickupObjectScript pickup = hit.collider.GetComponent<PickupObjectScript>();
                MedallionID medallion = hit.collider.GetComponentInParent<MedallionID>();

                if (pickup != null && medallion != null)
                {
                    pickup.PickUp();
                    heldObject = hit.collider.gameObject;
                    holdingItem = true;

                    switch (medallion.type)
                    {
                        case MedallionID.MedallionType.Cube:
                            InventoryScript.PickupMedallion("Cube");
                            break;

                        case MedallionID.MedallionType.Cylinder:
                            InventoryScript.PickupMedallion("Cylinder");
                            break;

                        case MedallionID.MedallionType.Prism:
                            InventoryScript.PickupMedallion("Prism");
                            break;
                    }
                }
            }
        }
    }



    public void DropItem()
    {
        if (holdingItem && heldObject != null)
        {
            heldObject.GetComponent<PickupObjectScript>().PickUp();
            holdingItem = false;
            heldObject = null;
            InventoryScript.PickupMedallion("None");
        }
    }



    public void interactableObject(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("DoorButton") && door1Unlocked && InventoryScript.KeyCardEquipped())
                {
                    //puzzleDoor1.SetActive(false);
                    anim1.SetTrigger("OpenDoor");
                    EnemySpawner.currentRoom = 2;
                    InventoryScript.KeyCardSwipped();

                    // Play swipe audio once for this swipe
                    if (keycardSwipeClip != null)
                    {
                        AudioSource.PlayClipAtPoint(keycardSwipeClip, transform.position, keycardSwipeVolume);
                        AudioSource.PlayClipAtPoint(DoorOpenClip, transform.position, DoorOpenVolume);

                    }

                }
                else if (hit.collider.CompareTag("WirePanel") && InventoryScript.WirecuttersEquipped())
                {
                    WiresAreCut();
                    InventoryScript.WireCuttersUsed();
                    Debug.Log("Cut");
                }
                else if (hit.collider.CompareTag("DoorButton2") && InventoryScript.KeyCardEquipped())
                {
                    //puzzleDoor2.SetActive(false);
                    anim2.SetTrigger("OpenDoor");
                    EnemySpawner.currentRoom = 3;
                    InventoryScript.KeyCardSwipped();

                    // Play swipe audio once for this swipe
                    if (keycardSwipeClip != null)
                    {
                        AudioSource.PlayClipAtPoint(keycardSwipeClip, transform.position, keycardSwipeVolume);
                        AudioSource.PlayClipAtPoint(DoorOpenClip, transform.position, DoorOpenVolume);

                    }
                }
                else if (hit.collider.CompareTag("C4Location") && InventoryScript.C4Equipped())
                {
                    C4.SetActive(true);
                    InventoryScript.C4Planted();
                    ExplosiveTimer.StartExplosionTimer();
                    Debug.Log("c4Planted");

                    if (EnemySpawner != null)
                    {
                        Debug.Log("RayCastFromPlayer: calling EnemySpawner.RespawnAllEnemies()");
                        EnemySpawner.RespawnAllEnemies();
                    }
                    else
                    {
                        Debug.LogWarning("RayCastFromPlayer: EnemySpawner reference is null. Assign it in the Inspector.");
                    }
                }
                else if (hit.collider.CompareTag("Lever"))
                {
                    ElevatorScript.ToggleElevator();
                }

            }

        }
    }

}
