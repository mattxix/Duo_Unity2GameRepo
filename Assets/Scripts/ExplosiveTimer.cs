using UnityEngine;
using TMPro;
using System.Collections;

public class ExplosiveTimer : MonoBehaviour
{
    public TMP_Text Timer;
    public float countdownTime = 60f;
    public BoxCollider BoxCollider;

    private Coroutine timerCoroutine;
    private bool timerStopped = false;

    // Start the timer
    public void StartExplosionTimer()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine); // stop existing timer
        timerCoroutine = StartCoroutine(Countdown());
        timerStopped = false;
        BoxCollider.enabled = true;

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
    }


    private void OnTriggerEnter(Collider other)
    {
        timerStopped = true;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        Timer.text = "Stopped";
    }
  
}
