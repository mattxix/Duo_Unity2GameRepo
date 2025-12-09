using System.Collections;
using TMPro;
using UnityEngine;

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

    public TMP_Text uiText;
    public float typeSpeed = 0.05f;
    public float fadeDuration = 1.0f;
    public float holdTime = 1.0f;

    private Coroutine currentRoutine;

    public void ShowObjective(string newMessage)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(TypeAndFade(newMessage));
    }

    IEnumerator TypeAndFade(string message)
    {
        if (uiText == null) yield break;

        uiText.text = "";
        Color c = uiText.color;
        c.a = 1f;
        uiText.color = c;

        // Type out the message
        for (int i = 0; i <= message.Length; i++)
        {
            uiText.text = message.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }

        // Hold the text for a moment
        yield return new WaitForSeconds(holdTime);

        // Fade out by modifying the text color alpha
        float elapsed = 0f;
        Color original = uiText.color;
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            Color fade = original;
            fade.a = alpha;
            uiText.color = fade;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Color end = original;
        end.a = 0f;
        uiText.color = end;
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
