using UnityEngine;

public class BeforeDoorTriggerScript : MonoBehaviour
{
    public TutorialHelperScript script;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            script.StartSceneTwo();


        }

    }
}
