using UnityEngine;

public class SendFadeScreenCutSceneCam : MonoBehaviour
{
    public EndSceneAni EndSceneAni;
    public void Send()
    {
        EndSceneAni.FadeOut();
    }
}
