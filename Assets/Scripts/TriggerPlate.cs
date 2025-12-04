using UnityEngine;

public class TriggerPlate : MonoBehaviour
{
    public RayCastFromPlayer raycastScript;
    public Animator anim;
    public string objTag;         
    public string requiredName;

    [Header("Snapping Settings")]
    public Transform snapPoint;
    public float snapRadius = 0.5f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(objTag)) return;

        if (other.name.StartsWith(requiredName))
        {
            if (!IsPlayerHoldingThis(other.gameObject))
            {
                TrySnap(other.gameObject);
            }

            //if (requiredName == "CubeMedallion") raycastScript.Cube = true;
            //if (requiredName == "CylinderMedallion") raycastScript.Cylinder = true;
            //if (requiredName == "PrismMedallion") raycastScript.Prism = true;

            Debug.Log("placed");
        }
    }

    private void TrySnap(GameObject medallion)
    {
        if (snapPoint == null) return;

        if (medallion.transform.parent != null)
            return;

        float distance = Vector3.Distance(medallion.transform.position, snapPoint.position);

        if (distance <= snapRadius)
        {
            medallion.transform.position = snapPoint.position;
            medallion.transform.rotation = snapPoint.rotation;

            if (requiredName == "CubeMedallion") raycastScript.Cube = true;
            if (requiredName == "CylinderMedallion") raycastScript.Cylinder = true;
            if (requiredName == "PrismMedallion") raycastScript.Prism = true;

            Rigidbody rb = medallion.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
            anim.SetTrigger("MSnapped");

            Debug.Log("Medallion snapped to snap point");
        }
    }

    private bool IsPlayerHoldingThis(GameObject obj)
    {
        return raycastScript.heldObject != null && raycastScript.heldObject == obj;
    }
}
