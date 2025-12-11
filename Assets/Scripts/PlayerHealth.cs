using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;
    public float playerHealth = 100;
    public float currentHealth;
    [Header("ImageFade")]
    public Image fadeImage;
    public float fadeDuration = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = playerHealth;
    }

    public void TakeDamage(float damage)
    {

        if (currentHealth > 0)
        {
            currentHealth -= damage;
            healthBar.fillAmount = currentHealth / playerHealth;
        }
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
            
        }
    }

    IEnumerator Die()
    {
        StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    IEnumerator FadeToBlack()
    {
        StartCoroutine(Fade(0f, 1f));
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
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
