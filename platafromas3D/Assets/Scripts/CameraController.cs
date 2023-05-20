using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    PlayerController player;
    private float camSpeed = 3f;
    private float mouseHorizontal = 1f;
    public float heigthDistance = 20f;
    public float xDistance = 20;
    Camera cam;
    public float rotationSpeed = 10;
    public float followSpeed = 5;
    float rotValue;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        cam = GetComponentInChildren<Camera>();
    }
    void Start()
    {
        
    }
    public void MoveCamera(InputAction.CallbackContext context)
    {
        rotValue = context.ReadValue<Vector2>().x;
    }
    void Update()
    {
        cam.transform.LookAt(player.transform,Vector3.up);
        Vector3 direction = player.transform.position - transform.position;
        if(direction.magnitude >= 2)
            transform.position += direction.normalized * camSpeed * Time.deltaTime;

        if (rotValue != 0) 
        {
            Vector3 rot = transform.eulerAngles;
            rot.y += rotationSpeed * Time.deltaTime * rotValue;
            transform.eulerAngles = rot;
        }
    }
}
