using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialHelperScript : MonoBehaviour
{
    [Header("Enemy")]
    public NavMeshAgent robotNavAgent;

    [Header("Animation")]
    public Animator anim;
    public Animator doorAnim;

    [Header("ImageFade")]
    public Image fadeImage;
    public float fadeDuration = 1f;


    [Header("Text")]
    public TMP_Text Speak;
    public float typingSpeed = 0.05f;
    public float linePause = 1.75f;
    public string Text1;
    public string Text2;
    public string Text3;
    public string Text4;
    public string Text5;
    public string Text6;
    public AudioSource audioSource;
    public AudioClip clip1;
    public float volume = 1f;

    [Header("Triggers")]
    public BoxCollider StartTrigger;
    public BoxCollider KeyCardTrigger;
    public BoxCollider RoomTwoTrigger;
    public BoxCollider RoomThreeTrigger;

    [Header("Colliders")]
    public BoxCollider PickupsCollider;
    public BoxCollider WireBoxCollider;
    public BoxCollider C4Collider;


    bool isWireCut = false;
    bool robotReadyforSceneTwo = false;
    

    void Start()
    {
        Speak.text = "";
        robotNavAgent.enabled = false;
    }

    private void Update()
    {
        if (isWireCut && robotReadyforSceneTwo)
        {
            WireBoxCollider.enabled = false;
        }
    }

    public void StartSceneOne()
    {
        StartTrigger.enabled = false;
        StartCoroutine(SceneOne());
    }
    public void StartSceneTwo()
    {
        KeyCardTrigger.enabled = false;
        StartCoroutine(SceneTwo());
    }
    public void StartSceneThree()
    {
        RoomTwoTrigger.enabled = false;
        StartCoroutine(SceneThree());
    }
    public void StartSceneFour()
    {
        RoomThreeTrigger.enabled = false;
        StartCoroutine(SceneFour());
    }
    public void StartSceneFive()
    {
        StartCoroutine(SceneFive());
    }

    public void WiresCut()
    {
        isWireCut = true;
    }

    public void PlantedBomb()
    {
        StartCoroutine(FadeToBlack());
        
    }



    IEnumerator SceneOne()
    {
        
        StartCoroutine(Dialogue(Text1));
        yield return new WaitForSeconds(11f);
        anim.SetTrigger("Start");
        anim.SetTrigger("Back");
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Dialogue(Text2));
        StartCoroutine(PointThenWalk());
    }

    

    IEnumerator PointThenWalk()
    {

        yield return new WaitForSeconds(13f);
        anim.SetTrigger("Point");
        PickupsCollider.enabled = false;
        yield return new WaitForSeconds(6.7f);
        anim.SetTrigger("ToDoor");
        yield return new WaitForSeconds(5f);
        robotReadyforSceneTwo = true;
    }

    IEnumerator SceneTwo()
    {
        StartCoroutine(Dialogue(Text3));
        anim.SetTrigger("Look");
        yield return new WaitForSeconds(.2f);
    }
    IEnumerator SceneThree()
    {
        
        anim.SetTrigger("ToDoor");
        anim.SetTrigger("ToRoomTwo");
        yield return new WaitForSeconds(5f);
        anim.SetTrigger("Point");
        StartCoroutine(Dialogue(Text4));
    }
    IEnumerator SceneFour()
    {
        anim.SetTrigger("ToDoor2");
        anim.SetTrigger("ToRoomThree");
        yield return new WaitForSeconds(3f);
        StartCoroutine(Dialogue(Text5));
        yield return new WaitForSeconds(8f);
        doorAnim.SetTrigger("Lower");
        yield return new WaitForSeconds(.75f);
        robotNavAgent.enabled = true;

    }
    IEnumerator SceneFive()
    {
        anim.SetTrigger("ToCore");
        anim.SetTrigger("CoreWalk");
        yield return new WaitForSeconds(1.25f);
        StartCoroutine(Dialogue(Text6));
        anim.SetTrigger("Point");
        yield return new WaitForSeconds(18f);
        C4Collider.enabled = false;

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

    IEnumerator FadeToBlack()
    {
        StartCoroutine(Fade(0f, 1f));
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = endAlpha;
        fadeImage.color = c;
    }
}
