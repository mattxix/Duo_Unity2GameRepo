using UnityEngine;

public class DropKeyCard : MonoBehaviour
{
    public Transform KeyCard1;
    public Transform targetTransform;

    public void DropKeyCard1()
    {
        KeyCard1.position = targetTransform.position;
        KeyCard1.rotation = targetTransform.rotation;

    }

}
