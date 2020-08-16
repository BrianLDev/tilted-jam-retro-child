using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnAnimatorBool : MonoBehaviour
{
    public GameObject toActivate;
    public Animator animator;
    public string boolName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      toActivate.SetActive(animator.GetBool(boolName));
    }
}
