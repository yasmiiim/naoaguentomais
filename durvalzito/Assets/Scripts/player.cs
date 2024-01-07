using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public int health = 2;
    public float speed;
    public float jumpForce;
    private bool isJumping;
    private bool doubleJump;
    private bool isAttacking;
    private Rigidbody2D rig;
    private Animator anim;

    public AudioSource soundAtk;
    public AudioSource soundJump;
    public AudioSource soundRun;

    public Transform ataquePoint;
    public float ataqueRanger = 0.5f;
    public LayerMask enemyLayers;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameController.instance.UpdateLives(health);
    }

    void Update()
    {
        Jump();
        AttackPlayer();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
                PlayRunSound();
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
                PlayRunSound();
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movement == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("transition", 0);
            soundRun.Stop();
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 2);
                PlayJumpSound();
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                isJumping = true;
            }
            else
            {
                if (doubleJump)
                {
                    anim.SetInteger("transition", 2);
                    anim.SetInteger("transition", 4);
                    PlayJumpSound();
                    rig.AddForce(new Vector2(0, jumpForce * 0.7f), ForceMode2D.Impulse);
                    doubleJump = false;
                }
            }
        }
    }

    void AttackPlayer()
    {
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                anim.SetInteger("transition", 3);
                soundAtk.Play();

                Collider2D[] hit = Physics2D.OverlapCircleAll(ataquePoint.position, ataqueRanger, enemyLayers);
                foreach (Collider2D enemy in hit)
                {
                    enemy.GetComponent<Enemy>().DanoNoInimigo(100);
                }

                yield return new WaitForSeconds(0.5f);
                anim.SetInteger("transition", 0);
                isAttacking = false;
            }
        }
    }

    void PlayRunSound()
    {
        if (!soundRun.isPlaying)
        {
            soundRun.Play();
        }
    }

    void PlayJumpSound()
    {
        soundJump.Play();
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        GameController.instance.UpdateLives(health);

        if (transform.rotation.y == 0)
        {
            transform.position += new Vector3(-1, 0, 0);
        }

        if (transform.rotation.y == 180)
        {
            transform.position += new Vector3(1, 0, 0);
        }

        if (health <= 0)
        {
            GameController.instance.GameOver();
        }
    }

    public void IncreaseLife(int value)
    {
        health += value;
        GameController.instance.UpdateLives(health);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8)
        {
            isJumping = false;
        }

        if (coll.gameObject.layer == 9)
        {
            GameController.instance.GameOver();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(ataquePoint.position, ataqueRanger);
    }
}
