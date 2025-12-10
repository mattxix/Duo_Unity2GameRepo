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
    public LayerMask wireBoxLayer;
    public TextMeshProUGUI helpMessage;
    public bool noMessageState = false;

    public TextDirections ObjectiveController;

    [Header("Room1")]
    public GameObject doorButton1;
    public GameObject wireBoxUncut;
    public GameObject wireBoxCut;
    public Light statusLight1;
    public Animator anim1;
    public bool wiresCut = false;
    bool KeyCard1InInventory = false;
    bool door1Unlocked = false;
    public AudioClip AlarmSound;

    [Header("Room2")]
    public GameObject doorButton2;
    public Light statusLight2;
    public Animator anim2;
    bool KeyCard2InInventory = false;

    [Header("Room3")]
    public Animator anim3;
    public bool Cube;
    public bool Cylinder;
    public bool Prism;

    [Header("Core")]
    public GameObject C4;
    public ExplosiveTimer ExplosiveTimer;
    public ElevatorScript ElevatorScript;

    [Header("Audio")]
    public AudioClip AlarmClip;
    public AudioClip keycardSwipeClip;
    public AudioClip DoorOpenClip;
    public AudioClip SnipWiresClip;

    [Range(0f, 1f)]
    public float AlarmVolume = 1f;
    public float keycardSwipeVolume = 2f;
    public float DoorOpenVolume = 2f;
    public float SnipWiresVolume = 2f;


    [HideInInspector]
    public bool accessGrantedPlayed = false;
    

    // Internal reference to the alarm AudioSource
    private AudioSource alarmAudioSource;

    void Start()
    {
        // Play the alarm sound on loop at game start
        if (AlarmClip != null)
        {
            alarmAudioSource = gameObject.AddComponent<AudioSource>();
            alarmAudioSource.clip = AlarmClip;
            alarmAudioSource.loop = true;
            alarmAudioSource.volume = AlarmVolume;
            alarmAudioSource.playOnAwake = false;
            alarmAudioSource.spatialBlend = 0f; // 2D sound
            alarmAudioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green);
        if (wiresCut && KeyCard1InInventory)
        {
            statusLight1.color = Color.green;
            door1Unlocked = true;
        }
        else
        {
            statusLight1.color = Color.red;
            door1Unlocked = false;
        }

        if (KeyCard2InInventory)
        {
            statusLight2.color = Color.green;
        }
        else
        {
            statusLight2.color = Color.red;
        }


        if (Cube && Prism && Cylinder)
        {
            anim3.SetTrigger("OpenDoor");
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
        if(InventoryScript.currentSlot == 0 && !holdingItem && !noMessageState)
        {
            helpMessage.text = "left click to pickup";
        }
        else if (InventoryScript.currentSlot != 0 && !holdingItem && !noMessageState)
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

        // Stop the alarm sound
        if (alarmAudioSource != null && alarmAudioSource.isPlaying)
        {
            alarmAudioSource.Stop();
        }

        TextDirections controller = ObjectiveController;

        if (controller != null)
        {
            if (InventoryScript != null && InventoryScript.keycardPickedFirst
                && (InventoryScript.hasKeyCard1 || InventoryScript.hasKeyCard2))
            {
                controller.MSG_UnlockDoor();
            }
            else
            {
                controller.MSG_FindKeycard();
            }
        }
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
                noMessageState = true;

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
            noMessageState = false;
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
                    anim1.SetTrigger("OpenDoor");
                    EnemySpawner.currentRoom = 2;
                    InventoryScript.KeyCardSwipped();
                    ObjectiveController.MSG_FindKeycard2();
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
                    AudioSource.PlayClipAtPoint(SnipWiresClip, wireBoxCut.transform.position, SnipWiresVolume);

                }
                else if (hit.collider.CompareTag("DoorButton2") && InventoryScript.KeyCardEquipped())
                {
                    anim2.SetTrigger("OpenDoor");
                    EnemySpawner.currentRoom = 3;
                    InventoryScript.KeyCardSwipped();
                    ObjectiveController.MSG_FindMedallions();
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
                    ObjectiveController.MSG_RUN();

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
