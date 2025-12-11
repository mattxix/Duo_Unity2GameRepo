using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneAni : MonoBehaviour
{
    public Animator anim;
    public AudioSource audioSource;
    public AudioSource audioSourceExplo;
    public AudioClip outsideSounds;
    public AudioClip doorOpen;
    public AudioClip shutDown;
    public AudioClip explosion;
    [Header("ImageFade")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    [Header("HealthDown")]
    public Image healthBar;
    public float playerHealth = 100;
    public float currentHealth;



    public void Start()
    {
        StartCoroutine(SwitchToFall());
        currentHealth = playerHealth;
        audioSource.PlayOneShot(outsideSounds,2f);
        audioSource.PlayOneShot(doorOpen);
    }

    IEnumerator SwitchToFall()
    {
        yield return new WaitForSeconds(1.0f);
        audioSourceExplo.PlayOneShot(explosion,2f);
        yield return new WaitForSeconds(3.0f);
        
        anim.SetTrigger("Fall");
        yield return new WaitForSeconds(.25f);
        audioSource.PlayOneShot(shutDown, 2f);
        yield return new WaitForSeconds(.50f);
        StartCoroutine(DrainBar());
        anim.SetTrigger("Dim");

    }

    public void FadeOut()
    {
        StartCoroutine(FadeToBlack());
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


    IEnumerator DrainBar()
    {
        float startFill = healthBar.fillAmount;
        float targetFill = 0; 
        float duration = 1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(startFill, targetFill, t / duration);
            yield return null;
        }

        healthBar.fillAmount = targetFill;
    }

}
