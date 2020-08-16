﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTextUp : MonoBehaviour
{
    public float speed = 1.5f;
    public float stopHeight = 850;

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.y < stopHeight) {
            transform.Translate(Vector2.up * speed);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        
    }
}
