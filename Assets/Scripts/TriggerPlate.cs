using UnityEngine;

public class TriggerPlate : MonoBehaviour
{
    public RayCastFromPlayer raycastScript;
    public string objTag;         
    public string requiredName;   

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(objTag))
        {
            Debug.Log($"Hit {other.name} with tag {other.tag}");

            if (other.name.StartsWith(requiredName))
            {
                if (requiredName == "CubeMedallion") raycastScript.Cube = true;
                if (requiredName == "CylinderMedallion") raycastScript.Cylinder = true;
                if (requiredName == "PrismMedallion") raycastScript.Prism = true;
                Debug.Log("placed");
                

            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(objTag))
        {
            if (other.name.StartsWith(requiredName))
            {
                if (requiredName == "CubeMedallion") raycastScript.Cube = false;
                if (requiredName == "CylinderMedallion") raycastScript.Cylinder = false;
                if (requiredName == "PrismMedallion") raycastScript.Prism = false;
                Debug.Log("removed");
            }
        }
    }
}
