using UnityEngine;

public class MedallionSnapping : MonoBehaviour
{
    [SerializeField] private Transform snapPoint;
    [SerializeField] private float snapDistance = 0.75f;

    [SerializeField] private string requiredTag = "Medallion";
    [SerializeField] private string requiredName = "PrismMedallion";

    public RayCastFromPlayer raycastScript;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(requiredTag))
            return;

        if (!other.name.StartsWith(requiredName))
            return;

        UpdateRaycastFlag(requiredName);

        float distance = Vector3.Distance(other.transform.position, snapPoint.position);

        if (distance <= snapDistance)
        {
            SnapObject(other.transform);
        }
    }

    private void SnapObject(Transform target)
    {
        target.SetPositionAndRotation(snapPoint.position, snapPoint.rotation);
        Debug.Log($"Snapped {target.name} to {snapPoint.name}");
    }

    private void UpdateRaycastFlag(string name)
    {
        if (name == "CubeMedallion") raycastScript.Cube = true;
        else if (name == "CylinderMedallion") raycastScript.Cylinder = true;
        else if (name == "PrismMedallion") raycastScript.Prism = true;
    }
}
