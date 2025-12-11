using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ExplosiveTimer : MonoBehaviour
{
    public TMP_Text Timer;
    public float countdownTime = 60f;
    public BoxCollider BoxCollider;
    public AudioSource AudioSource;
    public AudioSource AudioSourcePlayer;
    public AudioClip explosion;
    public AudioClip BombTicking;
    public MenuScript MenuScript;

    [Header("ImageFade")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    public float volumeTicking = 2.5f;
    public float explosionVolume = 1f;

    private Coroutine timerCoroutine;
    private bool timerStopped = false;

    // Start the timer
    public void StartExplosionTimer()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine); // stop existing timer
        timerCoroutine = StartCoroutine(Countdown());
        timerStopped = false;
        BoxCollider.enabled = true;
        AudioSource.PlayOneShot(BombTicking, volumeTicking);
    }

    IEnumerator Countdown()
    {
        float timeLeft = countdownTime;
        Timer.gameObject.SetActive(true);

        while (timeLeft > 0f)
        {
            if (timerStopped) yield break; // exit if timer is stopped

            Timer.text = Mathf.Ceil(timeLeft).ToString();
            yield return null; // waits a frame
            timeLeft -= Time.deltaTime;
        }

        Timer.text = "0";
        Explode();
    }

    private void Explode()
    {
        Debug.Log("Boom!"); // replace with lose Screen
        StartCoroutine(Restart());
        
    }

    IEnumerator Restart()
    {
        AudioSourcePlayer.PlayOneShot(explosion, explosionVolume);
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

    private void OnTriggerEnter(Collider other)
    {
        timerStopped = true;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        SceneManager.LoadScene(3);
    }
  
}
