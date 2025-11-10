using UnityEngine;

public class Pickup : MonoBehaviour
{
    ItemCollector collector; //Will be used for inventory later
    public GameObject targetPickupObject;
    public RayCastFromPlayer RayCastScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //collector = GameObject.Find("CoinHUD").GetComponent<ItemCollector>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(0, 0, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (targetPickupObject.CompareTag("WireCutters"))
            {
                Destroy(targetPickupObject);
                RayCastScript.HaveWireCutters();
                Debug.Log("PickedUpWireCutters");
            }
            else if (targetPickupObject.CompareTag("KeyCard1"))
            {
                Destroy(targetPickupObject);
                RayCastScript.HaveKeyCard1();
                Debug.Log("PickedUpKeyCard1");
            }
            else if(targetPickupObject.CompareTag("KeyCard2"))
            {
                Destroy(targetPickupObject);
                RayCastScript.HaveKeyCard2();
                Debug.Log("PickedUpKeyCard2");
            }


        }
    }
}
