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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time-squishTimer-squishTime;
        if(t>0) {
          if(t>=unSquishTime) {
            model.transform.localScale = new Vector3(1,1,1);
            if(health<=0) {
              Die();
            }
          } else {
            t = t/unSquishTime;
            float squish = t+squishAmount*(1-t);
            model.transform.localScale = new Vector3(1,squish,1);
          }
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
      health -= amount;
        if(hitEffect) {
          Instantiate(hitEffect, transform.position, transform.rotation);
        }
        Squish();
    }

    void Squish() {
      model.transform.localScale = new Vector3(1,squishAmount,1);
      squishTimer = Time.time;
    }

    void Die() {
      if(corpse) {
        Instantiate(corpse, transform.position, transform.rotation);
      }
      Destroy(gameObject);
    }
}
