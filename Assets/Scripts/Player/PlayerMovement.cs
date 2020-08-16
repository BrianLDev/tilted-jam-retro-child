using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4;
    public float lerpValue = 0.4f;
    public float stopLerp = 0.1f;
    public float rotationLerp = 0.4f;
    public float jumpForce1, jumpForce2, jumpForce3;
    public float tripplejumpTiming;
    private Vector3 movementDirection = Vector3.zero;
    private Vector3 groundVelocity = Vector3.zero;
    private Vector3 velocity;
    private Vector3 targetVelocity;
    private Rigidbody rb;
    public int jumpCounter = 0;
    public float lastLandTime;
    public float lastJumpTime;
    private bool airborn = false;
    private Animator animator;
    public GameObject model;
    private Quaternion targetRotation = Quaternion.identity;
    public bool grounded = true;
    public static PlayerMovement instance;
    public bool landing = false;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      float inputX = Input.GetAxisRaw("Horizontal");
      float inputY = Input.GetAxisRaw("Vertical");
      movementDirection.x = inputX;
      movementDirection.z = inputY;
      movementDirection = transform.rotation * movementDirection;
      targetVelocity = movementDirection * speed;
      animator.SetBool("MoveInput", targetVelocity!=Vector3.zero);
      if(animator.GetBool("IsLanding")||animator.GetBool("IsAttacking")) {
        targetVelocity = Vector3.zero;
      }
      // if(targetVelocity!=Vector3.zero) {
      //   groundVelocity = Vector3.Lerp(groundVelocity, targetVelocity, lerpValue);
      // } else {
      //   groundVelocity = Vector3.Lerp(groundVelocity, targetVelocity, stopLerp);
      // }
      groundVelocity = Accel(groundVelocity, targetVelocity, accel, decel);
      animator.SetFloat("Speed", groundVelocity.magnitude/speed);
      if(groundVelocity!=Vector3.zero) {
        targetRotation = Quaternion.LookRotation(groundVelocity);
      }
      model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, rotationLerp);
      velocity = groundVelocity;
      velocity.y = rb.velocity.y;
      rb.velocity = velocity;
      // rb.AddForce(groundVelocity, ForceMode.VelocityChange);
      if(airborn) {
        animator.SetBool("Falling", rb.velocity.y<0);
      }
      if(Input.GetButtonDown("Jump")) {
        if(grounded) {
          Jump();
        }
      }
      if(!IsGrounded()) {
        airborn = true;
        grounded = false;
      }
      if(Input.GetButtonDown("Fire1")) {
        // if(grounded) {
          animator.SetTrigger("Attack");
        // }
      }
      if(!grounded&&Time.time-lastJumpTime>0.2f&&IsGrounded()) {
        Land();
      }
      animator.SetBool("Grounded", grounded);
    }

    private void OnCollisionEnter(Collision other) {
      if(!grounded&&IsGrounded()) {
        Land();
      }
    }

    public LayerMask layerMask;
    public float groundCastDistance = 0.2f;
    bool IsGrounded() {
      if(rb.velocity.y>0)return false;
      RaycastHit hit;
      // layerMask = ~layerMask;
      float up = 2;
      bool isHit = Physics.Raycast(transform.position+Vector3.up*up, Vector3.down, out hit, groundCastDistance+up, layerMask);
      return isHit;
    }

    void Jump() {
      animator.SetTrigger("Jump");
      animator.SetBool("Land",false);
      if(Time.time <= lastLandTime+tripplejumpTiming) {
        jumpCounter++;
        if(jumpCounter>2) {
          jumpCounter = 0;
        }
      } else {
        jumpCounter = 0;
      }
      animator.SetFloat("JumpType", jumpCounter);
    }

    void Land() {
      animator.SetTrigger("Land");
      airborn = false;
      lastLandTime = Time.time;
      grounded = true;
      animator.SetBool("Falling", false);
    }
    
    public void DoJump() {
      float jumpForce = 0;
      switch(jumpCounter) {
        case 0: jumpForce=jumpForce1; break;
        case 1: jumpForce=jumpForce2; break;
        default: jumpForce = jumpForce3; break;
      }
      rb.AddForce(Vector3.up*jumpForce, ForceMode.VelocityChange);
      lastJumpTime = Time.time;
      grounded = false;
      airborn = true;
      groundVelocity = targetVelocity;
    }

    public float accel;
    public float decel;
    Vector3 Accel(Vector3 start, Vector3 end, float accel, float decel) {
      Vector3 diff = end-start;
      float mag = diff.magnitude;
      float a = accel;
      if(end == Vector3.zero) a = decel;
      if(mag<=a) return end;
      return start+diff/mag*a;
    }
}
