using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePlayer : MonoBehaviour
{
    private bool isAttacking;

    public Animator anim;

    public Transform ataquePoint;

    public float ataqueRanger = 0.5f;

    public LayerMask enemyLayers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isAttacking = Input.GetKeyDown(KeyCode.E);

        if (isAttacking)
        {
            Attack();
        }
    }
    void Attack()
    {
        anim.SetInteger("transition", 3);

        Collider2D[] hit = Physics2D.OverlapCircleAll(ataquePoint.position, ataqueRanger, enemyLayers);

        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Enemy>().DanoNoInimigo(15);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(ataquePoint.position, ataqueRanger);
    }
}
