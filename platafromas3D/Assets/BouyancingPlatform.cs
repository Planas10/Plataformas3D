using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouyancingPlatform : MonoBehaviour
{
    public float jumpForce;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            if (!player.CanUsePlatform(transform.position))
                return;
            Vector3 vel = player.GetComponent<Rigidbody>().velocity;
            vel.y = 0;
            player.GetComponent<Rigidbody>().velocity = vel;
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce * 1.55f, ForceMode.Impulse);
        }
    }
}
