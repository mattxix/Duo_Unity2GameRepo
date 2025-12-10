using UnityEngine;

public class TutorialEnterRoomTwo : MonoBehaviour
{
    public TutorialHelperScript script;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            script.StartSceneThree();


        }

    }
}
