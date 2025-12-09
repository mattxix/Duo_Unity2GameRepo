using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Image healthBar;
    public float enemyHealth = 100;
    public float currentHealth;
    public GameObject healthBarParent;
    public DropKeyCard dropKeyCardScript;
    public TutorialHelperScript tutorialHelperScript;


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
        Destroy(gameObject);
    }
}
