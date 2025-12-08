using UnityEngine;
using TMPro;
using System.Collections;

public class TextDirections : MonoBehaviour
{
    public TMP_Text uiText;
    public float typeSpeed = 0.05f;
    public float fadeDuration = 1.0f;
    public float holdTime = 1.0f;

    public string initialMessage = "Objective: Find the wire cutters";

    private Coroutine currentRoutine;

    void Start()
    {
        if (!string.IsNullOrEmpty(initialMessage) && uiText != null)
        {
            ShowObjective(initialMessage);
        }
    }

    public void ShowObjective(string message)
    {
        if (uiText == null) return;
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(TypeAndFade(message));
    }


    public void MSG_SnipWires()
    {
        ShowObjective("Objective: snip the wires in the panel box to disable the alarm and unlock the door");
    }

    public void MSG_FindKeycard()
    {
        ShowObjective("Objective: Kill the main gaurd and use the keycard to unlock the door");
    }
    public void MSG_FindKeycard2()
    {
        ShowObjective("Objective: Parkour to the keycard located on the farthest hanging platform");
    }

    public void MSG_FindMedallions()
    {
        ShowObjective("Objective: Find the 3 medallions and place them in the door's interface to open it");
    }

    public void MSG_PlantC4()
    {
        ShowObjective("Objective: Plant the C4 and evacuate the base.");
    }

    public void MSG_RUN()
    {
        ShowObjective("Objective: Escape...                               RUN ");
    }

    public void MSG_CustomAndHold(string message, float holdSeconds)
    {
        if (uiText == null) return;
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(TypeAndFade(message, holdSeconds));
    }

    private IEnumerator TypeAndFade(string message, float overrideHold = -1f)
    {
        if (uiText == null) yield break;

        uiText.text = "";
        Color c = uiText.color;
        c.a = 1f;
        uiText.color = c;

        for (int i = 0; i <= message.Length; i++)
        {
            uiText.text = message.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }

        float hold = overrideHold >= 0f ? overrideHold : holdTime;
        yield return new WaitForSeconds(hold);

        float elapsed = 0f;
        Color original = uiText.color;
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            Color cc = original;
            cc.a = alpha;
            uiText.color = cc;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Color end = original;
        end.a = 0f;
        uiText.color = end;

        currentRoutine = null;
    }
}