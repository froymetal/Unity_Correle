﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float velocity = 2f;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.left * velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Funcion para detectar cuando el enemigo sale de escena y se autodestruye para liberar RAM
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Destroyer")
        { Destroy(gameObject);  }
        
    }
}