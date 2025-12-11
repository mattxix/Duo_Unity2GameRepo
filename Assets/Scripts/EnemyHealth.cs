using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public Image healthBar;
    public float enemyHealth = 100;
    public float currentHealth;
    public GameObject healthBarParent;
    public DropKeyCard dropKeyCardScript;
    public TutorialHelperScript tutorialHelperScript;
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip hitSound;
    public NavMeshAgent navMeshAgent;
    public BoxCollider boxCollider;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = enemyHealth;
    }

    public void TakeDamage(float damage)
    {

        if (currentHealth > 0)
        {
            currentHealth -= damage;
            healthBar.fillAmount = currentHealth / enemyHealth;
            Debug.Log("Play");
            anim.ResetTrigger("Hit");
            anim.SetTrigger("Hit");
            audioSource.PlayOneShot(hitSound, .18f);
        }
        if (currentHealth <= 0)
        {
            //enemy Dies
            
            Dead();
        }
    }

    void Dead()
    {
        if (gameObject.name == "RobotGuardEnemy")
        {
            dropKeyCardScript.DropKeyCard1();
        }
        if (gameObject.name == "TutRobotGuardEnemy" && tutorialHelperScript != null)
        {
            tutorialHelperScript.StartSceneFive();
        }
        Destroy(healthBarParent);
        StartCoroutine(DeathAni());
        
    }

    IEnumerator DeathAni()
    {
        navMeshAgent.enabled = false;
        boxCollider.enabled = false;
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(3.1f);
        Destroy(gameObject);
    }    
}
