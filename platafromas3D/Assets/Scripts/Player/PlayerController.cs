using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject PlayerBody;

    public float speed = 5;
    public float angularspeed = 5;

    public float JumpForce;
    public float maxSpeed;
    float h_mouse;

    public float h;


    public bool agachado;
    private bool canJump;
    private bool canRealyJump;

    public bool IsMoving;
    public bool IsAttached;
    public bool IsJumping;
    public bool IsFalling;
    public bool canSpawnCappy;
    private Rigidbody rb;
    public GameObject cappy;
    public Transform cameraPoint;
    public float timeBetweenJumps = 0.1f;
    public int maxJumps = 3;
    int currentJump = 0;
    public float additionalPercentageofJump = 30;
    public Transform cappySpawnPoint;
    public Transform platformMaximumjumpPoint;
    public int coins;
    Vector3 direction;
    Vector3 realDirection;
    bool canMove = false;
    float dirInterpolationAmount = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        cappySpawnPoint = transform.Find("cappySpawnPoint");
        canSpawnCappy = true;
        canRealyJump = true;
        direction = Vector3.zero;
        realDirection = Vector3.zero;
        Invoke("EnableController", 0.5f);
    }
    void EnableController() 
    {
        canMove = true;
    }
    private void Update()
    {
        if (IsAttached)
            return;
        float moveCoef = 1;
        if (IsJumping)
            moveCoef = 0.40f;
        if (agachado)
            moveCoef = 0.65f;
        if (!canMove)
            moveCoef = 0;
        if (dirInterpolationAmount < 1)
            dirInterpolationAmount += Time.deltaTime * angularspeed;
        realDirection = Vector3.Lerp(realDirection, direction,dirInterpolationAmount);
        Vector3 rotatedDirection = Quaternion.Euler(0, cameraPoint.eulerAngles.y - 3, 0) * realDirection;


        PlayerBody.transform.forward = transform.forward;

        rb.AddForce(rotatedDirection * speed * moveCoef * Time.deltaTime);
        if (!IsJumping) 
        {
            Vector3 velocityWithotY = rb.velocity;
            velocityWithotY.y = 0;
            velocityWithotY = Vector3.ClampMagnitude(velocityWithotY, maxSpeed);
            velocityWithotY.y = rb.velocity.y;
            rb.velocity = velocityWithotY;
        }

        IsMoving = rotatedDirection != Vector3.zero;
        if(rotatedDirection != Vector3.zero && !IsAttached)
            transform.LookAt(transform.position + rotatedDirection);

    }
    public void Agacharse(InputAction.CallbackContext context)
    {
        if (IsAttached)
            return;
        if (context.performed)
            agachado = true;
        if(context.canceled)
            agachado = false;
    }
    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove && !IsAttached)
            return;
        dirInterpolationAmount = 0;
        direction = context.ReadValue<Vector2>();
        direction = new Vector3(direction.x,0, direction.y).normalized;


    }
    public void SpawnCappy(InputAction.CallbackContext context) 
    {
        if (context.performed && canSpawnCappy) 
        {
            canSpawnCappy = false;
            GameObject instantiatedCappy = Instantiate(cappy);
            instantiatedCappy.transform.position = cappySpawnPoint.position;
            instantiatedCappy.GetComponent<Cappy>().Init(transform.forward,this);
        }
    }
    public IEnumerator ResetTrueJump() 
    {
        yield return new WaitForSeconds(0.25f);
        canRealyJump = true;
    }
    public void Jump(InputAction.CallbackContext context) {

        if (CanJump())
        {
            canRealyJump = false;
            StartCoroutine(ResetTrueJump());
            if (agachado)
            {
                ApplyBackWardsJump();
            }
            else if (IsAttached) 
            {
                ApplyWallJump();
            }
            else 
                ApplyForce();
        }
    }
    bool CanJump() 
    {
        return (currentJump > 0 && IsMoving && canJump && canRealyJump || currentJump == 0 && canJump && canRealyJump);
    }
    public void ResetTime() 
    {
        currentJump = 0;
        rb.drag = 1;
    }
    public void ApplyWallJump()
    {
        rb.useGravity = true;
        CancelInvoke();
        IsAttached = false;
        canMove = true;
        IsFalling = true;
        canJump = false;
        rb.AddForce((Vector3.up * 1.5f + (-transform.forward * 1.35f)) * (JumpForce * (1 + additionalPercentageofJump / 100 * currentJump)), ForceMode.Impulse);
    }
    public void ApplyBackWardsJump() 
    {
        CancelInvoke();
        canJump = false;
        if (rb.velocity.magnitude >= 0.1f) 
        {
            rb.velocity = Vector3.zero;
            rb.AddForce((transform.forward * JumpForce * 1.35f) + (transform.up * JumpForce * 0.75f), ForceMode.Impulse);
        }else
        rb.AddForce((Vector3.up  + (-transform.forward /2)) * (JumpForce * (1 + additionalPercentageofJump / 100 * currentJump) * 1.4f), ForceMode.Impulse);
    }
    public void ApplyForce() 
    {
        CancelInvoke();
        rb.AddForce(Vector3.up * (JumpForce * (1 + additionalPercentageofJump / 100 * currentJump)), ForceMode.Impulse);
        if (currentJump == 0) 
        {
            Vector3 vel = rb.velocity;
            vel.x *= 0.3f;
            vel.z *= 0.3f;
            rb.velocity = vel;
        }
        currentJump++;
        if (currentJump >= maxJumps)
            currentJump = 0;
    }
    public void OnTouchFloor() 
    {
        rb.drag = 0;
        IsJumping = false;
        canJump = true;
        Invoke("ResetTime", timeBetweenJumps);
    }
    public void OnExitFloor() 
    {
        IsJumping = true;
        canJump = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        AttacheableObject obj = collision.gameObject.GetComponent<AttacheableObject>();
        if (obj != null && IsJumping) 
        {
            AttachPlayer();
            transform.LookAt(collision.contacts[0].point);
            Vector3 angles  = transform.eulerAngles;
            angles.x += 43;
            transform.eulerAngles = angles;
        }
    }
    void AttachPlayer() 
    {
        canMove = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        IsAttached = true;
        canJump = true;
        canRealyJump = true;
        agachado = false;
        currentJump = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("collisión con enemigo");
            //transform.position = SpawnPoint.transform.position;
        }
    }
    public bool CanUsePlatform(Vector3 position)
    {
        return position.y < platformMaximumjumpPoint.position.y && !IsAttached;
    }
    public void IncreseCoins(int value)
    {
        coins += value;
        HudController.instance.UpdateCoins(coins);
    }
}
