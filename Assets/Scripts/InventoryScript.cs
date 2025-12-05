using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryScript : MonoBehaviour
{
    public enum HotbarItemType { None, Hand, Gun, C4, WireCutters, KeyCard }

    private List<HotbarItemType> activeSlotTypes = new List<HotbarItemType>();



    public RayCastFromPlayer RayCastScript;

    [Header("Main Items")]
    public GameObject gun;
    public GameObject c4;
    public GameObject wireCutters;
    public GameObject keyCard1;
    public GameObject keyCard2;

    [Header("Medallions (Hand Slot)")]
    public GameObject medallionCube;
    public GameObject medallionCylinder;
    public GameObject medallionPrism;

    [Header("Inventory State")]
    public bool hasGun = true; 
    public bool hasC4 = true;
    public bool hasWireCutters;
    public bool hasKeyCard1;
    public bool hasKeyCard2;

    [Header("InventoryIcons")]
    public GameObject[] Slots;

    private enum MedallionType { None, Cube, Cylinder, Prism }
    private MedallionType heldMedallion = MedallionType.None;

    public static int currentSlot = 1; 

    void Start()
    {
        SelectGun(); //Start with gun
        RebuildHotbar();

    }

    void Update()
    {
        for (int i = 0; i < activeSlotTypes.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
            }
        }
    }


    void RebuildHotbar()
    {
        activeSlotTypes.Clear();

        // Slot 0 - Hand (always)
        activeSlotTypes.Add(HotbarItemType.Hand);
        Slots[0].SetActive(true);

        // Slot 1 - Gun (always)
        activeSlotTypes.Add(HotbarItemType.Gun);
        Slots[1].SetActive(true);

        // Slot 2 - C4 (always)
        activeSlotTypes.Add(HotbarItemType.C4);
        Slots[2].SetActive(true);

        // Slot 3 - Wirecutters (only if owned)
        if (hasWireCutters)
        {
            activeSlotTypes.Add(HotbarItemType.WireCutters);
            Slots[3].SetActive(true);
        }
        else Slots[3].SetActive(false);

        // Slot 4 - Keycards (if owned)
        if (hasKeyCard1 || hasKeyCard2)
        {
            activeSlotTypes.Add(HotbarItemType.KeyCard);
            Slots[4].SetActive(true);
        }
        else Slots[4].SetActive(false);
    }

    void SelectSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= activeSlotTypes.Count)
            return;

        currentSlot = slotIndex;
        DisableAllItems();

        switch (activeSlotTypes[slotIndex])
        {
            case HotbarItemType.Hand:
                // No item to show
                break;

            case HotbarItemType.Gun:
                gun.SetActive(true);
                RayCastScript.DropItem();
                break;

            case HotbarItemType.C4:
                c4.SetActive(true);
                RayCastScript.DropItem();
                break;

            case HotbarItemType.WireCutters:
                wireCutters.SetActive(true);
                break;

            case HotbarItemType.KeyCard:
                if (hasKeyCard1)
                    keyCard1.SetActive(true);
                else if (hasKeyCard2)
                    keyCard2.SetActive(true);
                break;
        }
    }




    void DisableAllItems()
    {
        gun.SetActive(false);
        c4.SetActive(false);
        wireCutters.SetActive(false);
        keyCard1.SetActive(false);
        keyCard2.SetActive(false);

        //medallionCube.SetActive(false);
        //medallionCylinder.SetActive(false);
        //medallionPrism.SetActive(false);
    }

    // ---------------- HAND SLOT (MEDALLIONS) ----------------

    void SelectHand()
    {
        currentSlot = 0;
        DisableAllItems();

    }

    // ---------------- WEAPON / TOOL SLOTS ----------------

    void SelectGun()
    {
        currentSlot = 1;
        DisableAllItems();
        gun.SetActive(true);
        RayCastScript.DropItem();
    }

    void SelectC4()
    {
        currentSlot = 2;
        DisableAllItems();
        c4.SetActive(true);
        RayCastScript.DropItem();
    }

    void SelectWireCutters()
    {
        currentSlot = 3;
        DisableAllItems();
        Slots[3].SetActive(true);
        wireCutters.SetActive(true);
        RebuildHotbar();
    }

    void SelectKeyCard()
    {
        currentSlot = 4;
        DisableAllItems();

        if (hasKeyCard1)
            keyCard1.SetActive(true);
        else if (hasKeyCard2)
            keyCard2.SetActive(true);

        RebuildHotbar();
    }

    // ---------------- PICKUPS ----------------
    public void PickupWireCutters()
    {
        hasWireCutters = true;
        RebuildHotbar(); 
    }
    public void PickupKeyCard1()
    {
        hasKeyCard1 = true;
        RebuildHotbar(); 
    }
    public void PickupKeyCard2()
    {
        hasKeyCard2 = true;
        RebuildHotbar(); 
    }

    // MEDALLION PICKUP (ONE AT A TIME)
    public void PickupMedallion(string type)
    {
        // have Hand selected
        if (currentSlot != 0)
        {
            Debug.Log("Hand is not selected — cannot pick up medallion.");
            return;
        }

        if (type == "Cube") heldMedallion = MedallionType.Cube;
        if (type == "Cylinder") heldMedallion = MedallionType.Cylinder;
        if (type == "Prism") heldMedallion = MedallionType.Prism;

        Debug.Log("Picked up " + heldMedallion + " Medallion");

     
    }


    
    public void DropMedallion()
    {
        heldMedallion = MedallionType.None;
        SelectGun();
    }

    public bool WirecuttersEquipped()
    {
        int index = InventoryScript.currentSlot;  // current selected slot
        if (index >= 0 && index < activeSlotTypes.Count)
            return activeSlotTypes[index] == HotbarItemType.WireCutters;
        return false;
    }
}
