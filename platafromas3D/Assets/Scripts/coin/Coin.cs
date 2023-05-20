using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed;
    public int value;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) 
        {
            Destroy(gameObject);
            player.IncreseCoins(value);
        }
    }
}
