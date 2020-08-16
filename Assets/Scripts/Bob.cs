using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    public float frq;
    public float dist;
    private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
      pos.y = Mathf.Cos(Time.time*frq)*dist;
        transform.localPosition = pos;
    }
}
