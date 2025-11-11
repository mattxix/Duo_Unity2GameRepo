using UnityEngine;

public class ElevatorScript : MonoBehaviour
{

    public Animator leverAnim;
    public Animator elevatorAnim;


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
    }
}
