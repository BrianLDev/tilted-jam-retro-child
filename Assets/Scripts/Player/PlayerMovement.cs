using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4;
    public float lerpValue = 0.4f;
    public float jumpForce1, jumpForce2, jumpForce3;
    public float tripplejumpTiming;
    private Vector3 movementDirection = Vector3.zero;
    private Vector3 groundVelocity = Vector3.zero;
    private Vector3 velocity;
    private Vector3 targetVelocity;
    private Rigidbody rb;
    public int jumpCounter = 0;
    public float lastLandTime;
    private bool airborn = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
      groundVelocity = Vector3.Lerp(groundVelocity, targetVelocity, lerpValue);
      velocity = groundVelocity;
      velocity.y = rb.velocity.y;
      rb.velocity = velocity;
      if(airborn&&rb.velocity.y<0&&IsGrounded()) {
        Land();
      } 
      if(Input.GetButtonDown("Jump")) {
        if(IsGrounded()) {
          Jump();
        }
      }
      
    }

    // private void OnCollisionEnter(Collision other) {
    //   if(IsGrounded()&&airborn) {
    //     Land();
    //   }
    // }

    public LayerMask layerMask;
    public float groundCastDistance = 0.2f;
    bool IsGrounded() {
      if(rb.velocity.y>0)return false;
      RaycastHit hit;
      // layerMask = ~layerMask;
      bool isHit = Physics.Raycast(transform.position, Vector3.down, out hit, groundCastDistance, layerMask);
      return isHit;
    }

    void Jump() {
      airborn = true;
      float jumpForce = jumpForce1;
      if(Time.time <= lastLandTime+tripplejumpTiming) {
        jumpCounter++;
        if(jumpCounter>2) {
          jumpCounter = 0;
        } else if(jumpCounter==1) {
          jumpForce = jumpForce2;
        } else { 
          jumpForce=jumpForce3;
        }
      } else {
        jumpCounter = 0;
      }
      // switch(jumpCounter) {
      //   case 0: jumpForce=jumpForce1; break;
      //   case 1: jumpForce=jumpForce2; break;
      //   default: jumpForce = jumpForce3; break;
      // }
      rb.AddForce(Vector3.up*jumpForce, ForceMode.VelocityChange);
    }

    void Land() {
      airborn = false;
      lastLandTime = Time.time;
    }
}
