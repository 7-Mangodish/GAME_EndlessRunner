using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Moving : MonoBehaviour
{

    [Header("Movement")]
    private Rigidbody rb;
    private float horizontal, vertical;
    private int currentLane = 1;
    private float targetHorizontal;
    [SerializeField] private float speed;
    [SerializeField] private float timeSmooth;
    [SerializeField] private float distance;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    //GroundCheck
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDrag;
    private bool grounded;
    private int jumpCount;

    [Header("LandingAndSlide")]
    [SerializeField] private float landForce;
    [SerializeField] private float timeSlide;
    private CapsuleCollider capsuleCollider;
    private SphereCollider sphereCollider;
    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        sphereCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        targetHorizontal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        grounded = isGround();
        Move();
        Jump();
        LandAndSlide();
    }


    private void LandAndSlide()
    {
        if (!grounded && Input.GetKeyDown("down"))
        {
            rb.AddForce(Vector3.down * landForce, ForceMode.Impulse);
        }
        else if(grounded && Input.GetKeyDown("down"))
        {
            //Debug.Log("Slide");
            capsuleCollider.enabled = false;
            sphereCollider.enabled = true;
            Invoke(nameof(ChangeCollider), timeSlide);
        }
    }
    private void Move()
    {
        if (Input.GetKeyDown("right") && currentLane < 2)
        {
            targetHorizontal += distance;
            currentLane += 1;
        }
        if (Input.GetKeyDown("left") && currentLane >0)
        {
            targetHorizontal -= distance;
            currentLane -= 1;

        }
        //this.transform.position = Vector3.MoveTowards(this.transform.position,
        //    new Vector3(targetHorizontal, this.transform.position.y, this.transform.position.z), speed * Time.deltaTime);
        this.transform.position = Vector3.Lerp(this.transform.position,
            new Vector3(targetHorizontal, this.transform.position.y, this.transform.position.z), timeSmooth);
    }
    private void Jump()
    {
        if (grounded && Input.GetKeyDown("up") )
        {   
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private bool isGround()
    {
        return Physics.Raycast(this.transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    private void ChangeCollider()
    {
        capsuleCollider.enabled = true;
        sphereCollider.enabled = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, new Vector3(this.transform.position.x, 
            transform.position.y - groundCheckDistance, transform.position.z));
    }
}
