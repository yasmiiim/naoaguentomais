using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float walkTime;
    public bool walkRight;

    private int helth;
    private int vidaAtual = 200;

    public int damage = 1;


    public int health;
    
    private float timer;
    
    private Animator anim;

    private Rigidbody2D rig;

    public void DanoNoInimigo(int dano)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= walkTime)
        {
            walkRight = !walkRight;
            timer = 0f;
        }

        if (walkRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
                rig.velocity = Vector2.right * speed;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            rig.velocity = Vector2.left * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<player>().Damage(damage);
        }
    }
}
