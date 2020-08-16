using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public GameObject hitEffect;
    public float squishAmount;
    public GameObject corpse;
    public GameObject model;
    public float squishTime;
    public float unSquishTime;
    private float squishTimer;
    private List<GameObject> path = new List<GameObject>();
    public GameObject pathHolder;
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;
    private int pathIndex = 0;
    bool squishing = false;
    public float speed = 4f;
    public float lerp = 0.1f;
    public float waitTime = 0.5f;
    private float timer = 1f;
    public AudioClip[] damageAudio;
    public AudioClip[] dieAudio;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        foreach(Transform t in pathHolder.transform) {
          path.Add(t.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time-squishTimer-squishTime;
        if(t>0) {
          if(t>=unSquishTime) {
            model.transform.localScale = new Vector3(1,1,1);
            squishing = false;
            if(health<=0) {
              Die();
            }
          } else {
            squishing = true;
            t = t/unSquishTime;
            float squish = t+squishAmount*(1-t);
            model.transform.localScale = new Vector3(1,squish,1);
          }
        }
        if(squishing) {
          targetVelocity = Vector3.zero;
        } else {
          PathToPath();
        }
        Animate();
        velocity = Vector3.Lerp(velocity, targetVelocity, lerp);
        rb.velocity = velocity;

    }

    Quaternion targetRotation;
    public float animYFrq = 4;
    public float animAFrq = 4;
    public float animY = 0.2f;
    public float animAngle = 10;
    void Animate() {
      Vector3 pos = model.transform.localPosition;
      if(targetVelocity != Vector3.zero) {
        targetRotation = Quaternion.LookRotation(targetVelocity);
        pos.y = (Mathf.Cos(Time.time*animYFrq)+1)*animY;
      } else {
        pos.y = 0;
      }
      model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, 0.3f);
      if(targetVelocity!=Vector3.zero) {
        model.transform.Rotate(0,0,(Mathf.Cos(Time.time*animAFrq)*animAngle));
      }
      model.transform.localPosition = pos;
    }
    public float minPathDist = 0.2f;
    void PathToPath() {
      if(timer>0) {
        targetVelocity = Vector3.zero;
        timer -= Time.deltaTime;
        return;
      }
      Vector3 target = path[pathIndex].transform.position;
      Vector3 diff = target - transform.position;
      diff.y = 0;
      float mag = diff.magnitude;
      if(mag<minPathDist) {
        pathIndex ++;
        if(pathIndex>=path.Count)pathIndex=0;
        timer = waitTime;
      } else {
        targetVelocity = diff/mag*speed;
      }
    }

    private void OnTriggerEnter(Collider other) {
      if(other.tag=="PlayerHit") {
        // Destroy(gameObject);
        // health -= 1;
        // if(hitEffect) {
        //   Instantiate(hitEffect, transform.position, transform.rotation);
        // }
        // Squish();
        Hurt(1);
      }
    }

    public void Hurt(float amount) {
      AudioManager.Instance.PlayRandomClip(damageAudio);
      health -= amount;
        if(hitEffect) {
          Instantiate(hitEffect, transform.position, transform.rotation);
        }
        Squish();
    }

    void Squish() {
      model.transform.localScale = new Vector3(1,squishAmount,1);
      squishTimer = Time.time;
      squishing = true;
    }

    void Die() {
      if(corpse) {
        Instantiate(corpse, transform.position, transform.rotation);
      }
      AudioManager.Instance.PlayRandomClip(dieAudio);
      Destroy(gameObject);
    }
}
