using UnityEngine;

public class ElevatorScript : MonoBehaviour
{

    public Animator leverAnim;
    public Animator leverAnim2;
    public Animator elevatorAnim;

    [Header("Audio")]
    public AudioSource AudioSource;
    public AudioClip ElevatorSounds;
    public float SoundVolume = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleElevator()
    {
        elevatorAnim.SetTrigger("Toggle");

        leverAnim.SetTrigger("Flip");
        leverAnim2.SetTrigger("Flip");
        AudioSource.Stop();
        AudioSource.PlayOneShot(ElevatorSounds, SoundVolume);
    }
}
