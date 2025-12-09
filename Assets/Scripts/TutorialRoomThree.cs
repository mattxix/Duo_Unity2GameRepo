using UnityEngine;

public class TutorialRoomThree : MonoBehaviour
{
    public TutorialHelperScript script;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            script.StartSceneFour();


        }

    }
}
