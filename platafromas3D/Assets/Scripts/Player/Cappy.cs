using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cappy : MonoBehaviour
{
    public float distance;
    public float speed;
    public float spawnTime;
    Vector3 direction;
    Vector3 startPoint;
    public Collider colliderCollision;
    public Collider colliderTrigger;
    public float jumpForce;
    PlayerController owner;
    private void Awake()
    {
        colliderCollision.enabled = false;
        colliderTrigger.enabled = false;

    }
    void Start()
    {
        
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, startPoint) >= distance) 
        {
            transform.position = startPoint + direction * distance;
            Activate();
        }
    }
    void DestroyObject() 
    {
        owner.canSpawnCappy = true;
        Destroy(gameObject);
    }
    void Activate() 
    {
        colliderCollision.enabled = true;
        colliderTrigger.enabled = true;
        Invoke("DestroyObject", spawnTime);
    }
    public void Init(Vector3 _direction,PlayerController player) 
    {
        direction = _direction;
        startPoint = transform.position;
        owner = player;
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) 
        {
              if (!player.CanUsePlatform(transform.position))
                return;
            Vector3 vel = player.GetComponent<Rigidbody>().velocity;
            vel.y = 0;
            player.GetComponent<Rigidbody>().velocity = vel;
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce * 1.55f,ForceMode.Impulse);
        }
    }
}
