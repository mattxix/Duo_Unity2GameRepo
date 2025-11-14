using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("General")]
    public GameObject EnemyPrefab;
    public GameObject patrolScript;
    public Transform player;
    public int currentRoom = 1; // 1 = room1, 2 = room2, 3 = room3, 4 = endRoom
    private bool isChecking = true;
    [Header("Room1")]
    public Transform[] spawnLocations;
    [Header("Room2")]
    public GameObject Room2EnemyGroup;
    public Transform[] spawnLocationsRoom2;
    [Header("Room3")]
    public GameObject Room3EnemyGroup;
    public Transform[] spawnLocationsRoom3;

    bool hasClearedRoom4 = false;
    void Start()
    {
        StartCoroutine(RoomCheckLoop());
    }
    void Update()
    {
        if (currentRoom == 2)
        {
            Room2EnemyGroup.SetActive(true);
        }
        else if (currentRoom == 3)
        {
            Room3EnemyGroup.SetActive(true);
        }
    }
    IEnumerator RoomCheckLoop()
    {
        while (isChecking)
        {
            yield return new WaitForSeconds(5f); 

            switch (currentRoom)
            {
                case 1:
                    Room1Event();
                    break;
                case 2:
                    Room2Event();
                    break;
                case 3:
                    Room3Event();
                    break;
                case 4:
                    EndEvent();
                    break;
                
            }
        }
    }

    void Room1Event()
    {
        Debug.Log("Room 1 event triggered!");
        SpawnZombieRoom1();
    }

    void Room2Event()
    {
        Debug.Log("Room 2 event triggered!");
        SpawnZombieRoom2();
    }

    void Room3Event()
    {
        Debug.Log("Room 3 event triggered!");
        SpawnZombieRoom3();
    }
    void EndEvent()
    {
        if (hasClearedRoom4) return;
        hasClearedRoom4 = true;

        Debug.Log("Room 4 entered — clearing all enemies!");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
            
    }
    public void SpawnZombieRoom1()
    {
        int randomIndex = Random.Range(0, spawnLocations.Length);

        Transform spawnPoint = spawnLocations[randomIndex];
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyHealth respawnScript = newEnemy.GetComponent<EnemyHealth>();
        PatrolEnemy ai = newEnemy.GetComponent<PatrolEnemy>();
        if (ai != null)
            ai.target = player;
    }
    public void SpawnZombieRoom2()
    {
        int randomIndex = Random.Range(0, spawnLocationsRoom2.Length);

        Transform spawnPoint = spawnLocationsRoom2[randomIndex];
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyHealth respawnScript = newEnemy.GetComponent<EnemyHealth>();
        PatrolEnemy ai = newEnemy.GetComponent<PatrolEnemy>();
        if (ai != null)
            ai.target = player;
    }
    public void SpawnZombieRoom3()
    {
        int randomIndex = Random.Range(0, spawnLocationsRoom3.Length);

        Transform spawnPoint = spawnLocationsRoom3[randomIndex];
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyHealth respawnScript = newEnemy.GetComponent<EnemyHealth>();
        PatrolEnemy ai = newEnemy.GetComponent<PatrolEnemy>();
        if (ai != null)
            ai.target = player;
    }

   
    

}
