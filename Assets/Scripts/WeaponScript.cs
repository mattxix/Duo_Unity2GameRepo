using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    public float weaponDamage = 100.0f;
    public float weaponRange = 200.0f;
    public float fireRate = 20.0f;
    public float nextFire = 0.0f;
    public Camera fpsCamera;

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, transform.forward, out hit, weaponRange))
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
        if(ctx.performed && Time.time >= nextFire)
        {
            nextFire = Time.time + 1.0f / fireRate;
            Shoot();
        }
    }

}
