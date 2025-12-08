using UnityEngine;
using System.Collections;

public class CameraPanSequence : MonoBehaviour
{
    [Header("Cameras")]
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;

    [Header("Animators")]
    public Animator cam1Animator;
    public Animator cam2Animator;
    public Animator cam3Animator;

    [Header("Animation State Names")]
    public string cam1AnimationState = "Cam1Animation";
    public string cam2AnimationState = "Cam2Animation3";
    public string cam3AnimationState = "Cam3Animation";

    [Header("Animation Durations (seconds)")]
    public float cam1Duration = 3f;
    public float cam2Duration = 3f;
    public float cam3Duration = 3f;

    void Start()
    {
        StartCoroutine(PlayCameraSequence());
    }

    IEnumerator PlayCameraSequence()
    {
        while (true)
        {
            // Camera 1
            SetActiveCamera(cam1);
            cam1Animator.Play(cam1AnimationState, 0, 0f);
            yield return new WaitForSeconds(cam1Duration);

            // Camera 2
            SetActiveCamera(cam2);
            cam2Animator.Play(cam2AnimationState, 0, 0f);
            yield return new WaitForSeconds(cam2Duration);

            // Camera 3
            SetActiveCamera(cam3);
            cam3Animator.Play(cam3AnimationState, 0, 0f);
            yield return new WaitForSeconds(cam3Duration);
        }
    }

    void SetActiveCamera(Camera activeCam)
    {
        cam1.gameObject.SetActive(activeCam == cam1);
        cam2.gameObject.SetActive(activeCam == cam2);
        cam3.gameObject.SetActive(activeCam == cam3);
    }
}
