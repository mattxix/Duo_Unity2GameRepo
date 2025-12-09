using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialHelperScript : MonoBehaviour
{
    [Header("Animation")]
    public Animator anim;


    [Header("Text")]
    public TMP_Text Speak;
    public float typingSpeed = 0.05f;
    public float linePause = 1.75f;
    public string Text1;
    public string Text2;
    public AudioSource audioSource;
    public AudioClip clip1;
    public float volume = 1f;

    [Header("Boxs")]
    public BoxCollider BoxCollider1;

    

    private bool dialogueStarted = false;

    void Start()
    {
        Speak.text = "";

    }

    public void StartSceneOne()
    {
        BoxCollider1.enabled = false;
        StartCoroutine(SceneOne());
    }

    IEnumerator SceneOne()
    {
        dialogueStarted = true;
        StartCoroutine(Dialogue(Text1));
        yield return new WaitForSeconds(11f);
        anim.SetTrigger("Start");
        anim.SetTrigger("Back");
        yield return new WaitForSeconds(1f);
        StartCoroutine(Dialogue(Text2));
        StartCoroutine(Point());
    }
    

    IEnumerator Point()
    {

        yield return new WaitForSeconds(7.7f);
        anim.SetTrigger("Point");
    }
    IEnumerator Dialogue(string theText)
    {

        for (int i = 0; i < theText.Length; i++)
        {
            char c = theText[i];


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
