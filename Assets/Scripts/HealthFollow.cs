using UnityEngine;

public class HealthFollow : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    Vector3 offset;


    private void Start()
    {
        offset = transform.position - enemy.transform.position;


    }

    private void Update()
    {
        transform.LookAt(player);
        transform.position = enemy.transform.position + offset;
    }
}
