using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public GameObject[] slots; 
    private int currentSlot = 0;

    void Start()
    {
        SelectSlot(0); //starts with hand
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0); //hand
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1); //Gun
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2); //C4
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3); //WireCutters
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4); //Keycards
    }

    void SelectSlot(int slotIndex)
    {
        if (slotIndex == currentSlot) return; //Ignore same slot chosen

        currentSlot = slotIndex; //Set Chosen Slot

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(i == currentSlot);
        }
    }
}
