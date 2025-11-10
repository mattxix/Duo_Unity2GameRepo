using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour
{
    public float weaponDamage = 10.0f;
    public float weaponRange = 200.0f;
    public float fireRate = 1.0f;
    public float nextFire = 0.0f;
    public Camera fpsCamera;
    public Animator anim;
    public AudioSource audioSource; 
    public AudioClip gunFireClip;

    [Header("Scope")]
    public Camera playerCam;
    public float aimFOV = 55f;
    public float normalFOV = 85f;
    public float fovSpeed = 10f;

    public MouseLook lookScriptCamera;
    public MouseLook lookScriptPlayer;

    private bool isAiming = false;


    [Header("Reload")]
    public Image reloadBar;
    public float fillSpeed = 0.1f;


    void Update()
    {
        float targetFOV = isAiming ? aimFOV : normalFOV;
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetFOV, Time.deltaTime * fovSpeed);
    }
    public void Shoot()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit,
            weaponRange,Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                //apply damage
                hit.collider.GetComponent<EnemyHealth>().TakeDamage(weaponDamage);
                Debug.Log(hit.collider.name);
            }
        }
        
    }

    public void FireShot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            audioSource.PlayOneShot(gunFireClip);
            anim.SetTrigger("Shoot");
            //StartCoroutine(Recoil());
            Debug.Log("shot");
            Shoot();
        }
    }

    public void AimIn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isAiming = true;
            lookScriptCamera.SetAiming(true);
            lookScriptPlayer.SetAiming(true);
        }
        else if (ctx.canceled)
        {
            isAiming = false;
            lookScriptCamera.SetAiming(false);
            lookScriptPlayer.SetAiming(false);
        }
    }

    

    IEnumerator Recoil()
    {
        reloadBar.fillAmount = 0f; 
        while (reloadBar.fillAmount < 1f)
        {
            reloadBar.fillAmount += Time.deltaTime * fillSpeed;
            reloadBar.fillAmount = Mathf.Clamp01(reloadBar.fillAmount);
            yield return null;
            
        }
        //anim.SetTrigger("Shoot");





    }


    


}
