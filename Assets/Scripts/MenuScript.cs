using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera camStart;

    [Tooltip("Interval in seconds between random camera switches.")]
    public float randomSwitchInterval = 5f;

    public Canvas menuCanvas;

    Camera[] randomCameras;
    int lastRandomIndex = -1;
    Coroutine randomCameraCoroutine;

    // Helper to enable one camera and disable the others
    private void SetActiveCamera(Camera activeCam)
    {
        if (cam1 != null) cam1.gameObject.SetActive(cam1 == activeCam);
        if (cam2 != null) cam2.gameObject.SetActive(cam2 == activeCam);
        if (cam3 != null) cam3.gameObject.SetActive(cam3 == activeCam);
        if (camStart != null) camStart.gameObject.SetActive(camStart == activeCam);
    }

    void Start()
    {
        // prepare array used by the random picker (ignore null entries)
        var list = new System.Collections.Generic.List<Camera>();
        if (cam1 != null) list.Add(cam1);
        if (cam2 != null) list.Add(cam2);
        if (cam3 != null) list.Add(cam3);
        randomCameras = list.ToArray();

        // only start switching if we have at least 2 cameras to choose from
        if (randomCameras.Length >= 2)
        {
            randomCameraCoroutine = StartCoroutine(RandomCameraLoop());
        }
    }

    void OnDestroy()
    {
        if (randomCameraCoroutine != null)
            StopCoroutine(randomCameraCoroutine);
    }

    IEnumerator RandomCameraLoop()
    {
        // choose an initial camera immediately (optional)
        lastRandomIndex = -1;
        yield return null; // allow one frame for cameras to initialize

        while (true)
        {
            yield return new WaitForSeconds(randomSwitchInterval);

            if (randomCameras == null || randomCameras.Length == 0)
                yield break;

            int nextIndex = Random.Range(0, randomCameras.Length);

            // if only one camera configured, nothing to do
            if (randomCameras.Length == 1)
            {
                SetActiveCamera(randomCameras[0]);
                lastRandomIndex = 0;
                continue;
            }

            // pick a different index than the previous one
            // use a small loop guard to avoid infinite loops in unexpected cases
            int attempts = 0;
            while (nextIndex == lastRandomIndex && attempts < 8)
            {
                nextIndex = Random.Range(0, randomCameras.Length);
                attempts++;
            }

            lastRandomIndex = nextIndex;
            SetActiveCamera(randomCameras[nextIndex]);
        }
    }

    public void StartGame()
    {
        // hide the menu canvas immediately
        if (menuCanvas != null)
            menuCanvas.gameObject.SetActive(false);

        // stop random switching while playing the start camera animation
        if (randomCameraCoroutine != null)
        {
            StopCoroutine(randomCameraCoroutine);
            randomCameraCoroutine = null;
        }

        StartCoroutine(PlayStartCamThenLoad());
    }

    IEnumerator PlayStartCamThenLoad()
    {
        if (camStart == null)
        {
            SceneManager.LoadScene(0);
            yield break;
        }

        SetActiveCamera(camStart);

        // Try Animator first (Mecanim)
        Animator animator = camStart.GetComponent<Animator>();
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            if (clips != null && clips.Length > 0)
            {
                // Play the first clip on layer 0 from the beginning
                string clipName = clips[0].name;
                animator.Play(clipName, 0, 0f);

                // Wait until the animator reports the state finished, with a timeout fallback.
                float timeout = clips[0].length + 0.5f;
                float timer = 0f;

                while (!animator.GetCurrentAnimatorStateInfo(0).IsName(clipName) && timer < timeout)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                // Now wait until the state's normalizedTime reaches 1 (clip finished) or timeout
                timer = 0f;
                while (animator.GetCurrentAnimatorStateInfo(0).IsName(clipName) &&
                       animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f &&
                       timer < timeout)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                // Disable all cameras immediately to avoid showing camStart POV, then load next frame
                SetActiveCamera(null);
                yield return null;
                SceneManager.LoadScene(0);
                yield break;
            }
        }

        // Fallback to legacy Animation component
        Animation legacy = camStart.GetComponent<Animation>();
        if (legacy != null && legacy.clip != null)
        {
            string stateName = legacy.clip.name;
            legacy.Play(stateName);
            yield return new WaitForSeconds(legacy.clip.length);

            // Disable camera and load immediately next frame
            SetActiveCamera(null);
            yield return null;
            SceneManager.LoadScene(0);
            yield break;
        }

        // No animation found — load immediately
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SetActiveCamera(cam1);
        SceneManager.LoadScene(1);
    }

    public void Instructions()
    {
        SetActiveCamera(cam2);
        StartCoroutine(DelayedLoad(2, 2));
    }

    public void Credits()
    {
        SetActiveCamera(cam3);
        StartCoroutine(DelayedLoad(3, 2));
    }

    IEnumerator DelayedLoad(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
}
