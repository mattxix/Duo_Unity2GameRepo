using UnityEngine;

public class TutorialStartBoxOne : MonoBehaviour
{
    public TutorialHelperScript script;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            script.StartSceneOne();
            

        }
       
    }
}
