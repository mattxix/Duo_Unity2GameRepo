using UnityEngine;
using TMPro;
using System.Collections;

public class TextDirections : MonoBehaviour
{
    public TMP_Text uiText;
    public float typeSpeed = 0.05f;
    public float fadeDuration = 1.0f;
    public float holdTime = 1.0f;

    private string message = "Objective: " +
                             "                                                               Find the wire cutters to turn off the alarm ";


    void Start()
    {
        if (uiText != null)
        {
            uiText.text = "";
            Color c = uiText.color;
            c.a = 1f;
            uiText.color = c;
            StartCoroutine(TypeAndFade());
        }
    }

    IEnumerator TypeAndFade()
    {
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
            Color c = original;
            c.a = alpha;
            uiText.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Color end = original;
        end.a = 0f;
        uiText.color = end;
    }
}
