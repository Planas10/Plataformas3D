using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFloor : MonoBehaviour
{
    PlayerController playerController;
    Collider coll;
    private void Awake()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
        coll = transform.GetComponent<Collider>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ResetCollider() 
    {
        coll.enabled = true;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("Obstacles"))
        {
            playerController.OnTouchFloor();
          //  coll.enabled = false;
           // Invoke("ResetCollider", 0.3f);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("Obstacles"))
        {
            playerController.OnExitFloor();
        }
    }
}
