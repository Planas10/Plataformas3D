using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = HudController.instance.player.transform;
    }
    void Start()
    {
        
    }

    void Update()
    {
        SetTarget();
    }
    void SetTarget()
    {
        agent.SetDestination(target.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null) 
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
