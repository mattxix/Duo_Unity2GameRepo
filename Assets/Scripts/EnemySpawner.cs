using System.Collections;
using System.Collections.Generic;
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
        if (spawnLocations == null || spawnLocations.Length == 0)
        {
            Debug.LogWarning("SpawnZombieRoom1: no spawnLocations assigned.");
            return;
        }

        int randomIndex = Random.Range(0, spawnLocations.Length);

        Transform spawnPoint = spawnLocations[randomIndex];
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        if (newEnemy != null && newEnemy.tag != "Enemy") newEnemy.tag = "Enemy";
        EnemyHealth respawnScript = newEnemy.GetComponent<EnemyHealth>();
        PatrolEnemy ai = newEnemy.GetComponent<PatrolEnemy>();
        if (ai != null)
            ai.target = player;
    }
    public void SpawnZombieRoom2()
    {
        if (spawnLocationsRoom2 == null || spawnLocationsRoom2.Length == 0)
        {
            Debug.LogWarning("SpawnZombieRoom2: no spawnLocationsRoom2 assigned.");
            return;
        }

        int randomIndex = Random.Range(0, spawnLocationsRoom2.Length);

        Transform spawnPoint = spawnLocationsRoom2[randomIndex];
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        if (newEnemy != null && newEnemy.tag != "Enemy") newEnemy.tag = "Enemy";
        EnemyHealth respawnScript = newEnemy.GetComponent<EnemyHealth>();
        PatrolEnemy ai = newEnemy.GetComponent<PatrolEnemy>();
        if (ai != null)
            ai.target = player;
    }
    public void SpawnZombieRoom3()
    {
        if (spawnLocationsRoom3 == null || spawnLocationsRoom3.Length == 0)
        {
            Debug.LogWarning("SpawnZombieRoom3: no spawnLocationsRoom3 assigned.");
            return;
        }

        int randomIndex = Random.Range(0, spawnLocationsRoom3.Length);

        Transform spawnPoint = spawnLocationsRoom3[randomIndex];
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        if (newEnemy != null && newEnemy.tag != "Enemy") newEnemy.tag = "Enemy";
        EnemyHealth respawnScript = newEnemy.GetComponent<EnemyHealth>();
        PatrolEnemy ai = newEnemy.GetComponent<PatrolEnemy>();
        if (ai != null)
            ai.target = player;
    } 

    public void RespawnAllEnemies()
    {

        var toDestroy = new HashSet<GameObject>();

        try
        {
            GameObject[] tagged = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var g in tagged) if (g != null) toDestroy.Add(g);
        }
        catch (UnityException)
        {
            Debug.LogWarning("RespawnAllEnemies: 'Enemy' tag lookup failed or tag doesn't exist. Falling back to component search.");
        }

        PatrolEnemy[] patrols = FindObjectsOfType<PatrolEnemy>();
        foreach (var p in patrols) if (p != null && p.gameObject != null) toDestroy.Add(p.gameObject);

        EnemyHealth[] healths = FindObjectsOfType<EnemyHealth>();
        foreach (var h in healths) if (h != null && h.gameObject != null) toDestroy.Add(h.gameObject);

        int removed = toDestroy.Count;
        foreach (var go in toDestroy)
        {
            if (go != null) Destroy(go);
        }

        Debug.Log($"RespawnAllEnemies: removed {removed} existing enemies. Spawning at every configured spawn point...");

        if (Room2EnemyGroup != null) Room2EnemyGroup.SetActive(true);
        if (Room3EnemyGroup != null) Room3EnemyGroup.SetActive(true);

        int spawned = 0;

        void SpawnAllIn(Transform[] arr)
        {
            if (arr == null || arr.Length == 0) return;
            foreach (Transform t in arr)
            {
                if (t == null) continue;
                SpawnAt(t);
                spawned++;
            }
        }

        SpawnAllIn(spawnLocations);
        SpawnAllIn(spawnLocationsRoom2);
        SpawnAllIn(spawnLocationsRoom3);

    }

    private void SpawnAt(Transform spawnPoint)
    {
        if (spawnPoint == null || EnemyPrefab == null) return;
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        if (newEnemy != null)
        {
            if (newEnemy.tag != "Enemy") newEnemy.tag = "Enemy";
            PatrolEnemy ai = newEnemy.GetComponent<PatrolEnemy>();
            if (ai != null) ai.target = player;
        }
    }

}
