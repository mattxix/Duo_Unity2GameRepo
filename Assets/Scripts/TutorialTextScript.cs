using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class TutorialTextScript : MonoBehaviour
{
    public TutorialHelperScript tutScript;
    public TMP_Text Speak;
    public float typingSpeed = 0.05f;
    public float linePause = 1.75f;
    public TMP_Text task;
    public TMP_Text runText;
    public string fullText;
    public AudioSource audioSource;
    public AudioClip clip1;
    public float volume = 1f;

    private bool dialogueStarted = false;

    void Start()
    {
        Speak.text = "";

    }
    public void SceneOneStart()
    {
        if (dialogueStarted) return;
        dialogueStarted = true;
        StartCoroutine(Dialogue());
    }

    IEnumerator Dialogue()
    {

        for (int i = 0; i < fullText.Length; i++)
        {
            char c = fullText[i];


            if (c == '/')
            {

                yield return new WaitForSeconds(linePause);

                Speak.text = "";

            }
            else
            {
                Speak.text += c;

                PlaySound();
                yield return new WaitForSeconds(typingSpeed);
            }



        }
        
        yield return new WaitForSeconds(typingSpeed);
        Speak.text = "";
    }

    public void PlaySound()
    {
        volume = Random.Range(.05f, .2f);
        audioSource.PlayOneShot(clip1, volume);
    }
}
