using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryScript : MonoBehaviour
{
    public enum HotbarItemType { None, Hand, Gun, C4, WireCutters, KeyCard }

    private List<HotbarItemType> activeSlotTypes = new List<HotbarItemType>();

    public RayCastFromPlayer RayCastScript;
    public TextDirections ObjectiveController; // Add this at the top with your other public fields

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

    [HideInInspector]
    public bool keycardPickedFirst = false;

    [Header("InventoryIcons")]
    public GameObject[] Slots;

    [Header("InventoryIcons")]
    public Image[] medallionImages;

    private enum MedallionType { None, Cube, Cylinder, Prism }
    private MedallionType heldMedallion = MedallionType.None;

    public static int currentSlot = 1;

    void Start()
    {
        SelectGun(); //Start with gun
        RebuildHotbar();
        medallionImages[0].enabled = true;
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

        // Slot 2 - C4 (only if owned)
        if (hasC4)
        {
            activeSlotTypes.Add(HotbarItemType.C4);
            Slots[2].SetActive(true);
        }
        else
        {
            Slots[2].SetActive(false);
        }

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

        // Check if the item in that slot is actually available
        HotbarItemType type = activeSlotTypes[slotIndex];
        if ((type == HotbarItemType.C4 && !hasC4) ||
            (type == HotbarItemType.WireCutters && !hasWireCutters) ||
            (type == HotbarItemType.KeyCard && !(hasKeyCard1 || hasKeyCard2)))
        {
            return; // do nothing if item is gone
        }

        currentSlot = slotIndex;
        DisableAllItems();

        switch (type)
        {
            case HotbarItemType.Hand:
                // Nothing to show
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
        // Player picked wirecutters now
        hasWireCutters = true;
        RebuildHotbar();

        // If player already has a keycard:
        // - If keycard was picked first, instruct to snip wires now.
        // - If keycard was picked second (i.e. wirecutters were first) instruct unlock door immediately.
        if (hasKeyCard1 && ObjectiveController != null)
        {
            if (keycardPickedFirst)
            {
                ObjectiveController.MSG_SnipWires();
            }
            else
            {
                ObjectiveController.MSG_UnlockDoor();
            }
        }
        else
        {
            // No keycard yet — standard objective: snip wires once found
            if (ObjectiveController != null)
                ObjectiveController.MSG_SnipWires();
        }
    }

    public void PickupKeyCard1()
    {
        // Record whether keycard was picked before wirecutters
        keycardPickedFirst = !hasWireCutters;

        hasKeyCard1 = true;
        RebuildHotbar();

        // If wires are cut and the player has KeyCard1, show unlock door objective
        if (RayCastScript != null && RayCastScript.wiresCut && hasKeyCard1)
        {
            if (ObjectiveController != null)
                ObjectiveController.MSG_UnlockDoor();
        }
        else if(hasWireCutters == true && hasKeyCard1 && RayCastScript.wiresCut == false)
        {
            if (ObjectiveController != null)
                ObjectiveController.MSG_SnipWires();
        }
        else
        {
            if (ObjectiveController != null)
                ObjectiveController.MSG_FindWireCutters();
        }
    }

    public void PickupKeyCard2()
    {
        keycardPickedFirst = !hasWireCutters;

        hasKeyCard2 = true;
        RebuildHotbar();
            if (ObjectiveController != null)
            { 
                ObjectiveController.MSG_UnlockDoor();
            }
               
     
  
    }

    // MEDALLION PICKUP (ONE AT A TIME)
    public void PickupMedallion(string type)
    {
        // have Hand selected
        //if (currentSlot != 0)
        //{
        //    Debug.Log("Hand is not selected — cannot pick up medallion.");
        //    return;
        //}

        medallionImages[0].enabled = false;
        medallionImages[1].enabled = false;
        medallionImages[2].enabled = false;
        medallionImages[3].enabled = false;

        if (type == "Cube")
        {
            heldMedallion = MedallionType.Cube;
            medallionImages[1].enabled = true;
        }
        else if (type == "Cylinder")
        {
            heldMedallion = MedallionType.Cylinder;
            medallionImages[2].enabled = true;
        }
        else if (type == "Prism")
        {
            heldMedallion = MedallionType.Prism;
            medallionImages[3].enabled = true;
        }
        else
        {
            medallionImages[0].enabled = true;
        }

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

    public void WireCuttersUsed()
    {
        hasWireCutters = false;
        RebuildHotbar();
        SelectSlot(1);
    }

    public bool KeyCardEquipped()
    {
        int index = InventoryScript.currentSlot;  // current selected slot
        if (index >= 0 && index < activeSlotTypes.Count)
            return activeSlotTypes[index] == HotbarItemType.KeyCard;
        return false;
    }

    public void KeyCardSwipped()
    {
        if (hasKeyCard1)
            hasKeyCard1 = false;
        else if (hasKeyCard2)
            hasKeyCard2 = false;
        RebuildHotbar();
        SelectSlot(1);
    }

    public bool C4Equipped()
    {
        int index = InventoryScript.currentSlot;  // current selected slot
        if (index >= 0 && index < activeSlotTypes.Count)
            return activeSlotTypes[index] == HotbarItemType.C4;
        return false;
    }

    public void C4Planted()
    {
        hasC4 = false;
        Slots[2].SetActive(false);
        c4.SetActive(false);
        RebuildHotbar();
        SelectSlot(1);
    }
}