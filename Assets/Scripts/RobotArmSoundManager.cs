using UnityEngine;

public class RobotArmSoundManager : MonoBehaviour
{
   public AudioSource AudioSource;
    public AudioClip upSound;
    public AudioClip downSound;
    public float upSoundVolume = 2f;
    public float downSoundVolume = 2f;

    public void Up()
    {
        AudioSource.PlayOneShot(upSound, upSoundVolume);
    }
    public void Down()
    {
        AudioSource.PlayOneShot(downSound, downSoundVolume);
    }
}
