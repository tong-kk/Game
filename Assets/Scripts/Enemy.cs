﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int HP = 2;
    public Sprite deadEnemy;            // 死亡后的灰色图片
    public Sprite hurtedEnemy;          // 受伤后的图片
    public GameObject UI_100Points;     // 怪物死后的得分
    public float deathSpinMin = -100f;  // 怪物死后的旋转量
    public float deathSpinMax = 100f;

    private Transform frontCheck;
    private SpriteRenderer ren;          // 负责更好对应图片的
    private Rigidbody2D enemyBody;       // 敌人的2D刚体
    private bool bDeath = false;         // 标识敌人已经死亡

    // Start is called before the first frame update
    void Start()
    {
        frontCheck = transform.Find("frontCheck");
        ren = transform.Find("alienShip").GetComponent<SpriteRenderer>();
        enemyBody = GetComponent<Rigidbody2D>();
    }
    void filp()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    public void Hurt()
    {
        HP--;
    }

    private void FixedUpdate()
    {
        int nLayer = 1 << LayerMask.NameToLayer("Wall");
        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, nLayer);
        foreach(Collider2D c in frontHits)
        {
            if(c.tag == "wall")
            {
                filp();
                break;
            }
        }
        enemyBody.velocity = new Vector2(moveSpeed * transform.localScale.x, enemyBody.velocity.y);

        if(HP == 1 && hurtedEnemy != null)
        {
            ren.sprite = hurtedEnemy;
        }
        if(HP == 0 && !bDeath)
        {
            Death();
        }
    }
    void Death()
    { 
        bDeath = true;
        SpriteRenderer[] renders = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer s in renders)
        {
            s.enabled = false;
        }

        ren.enabled = true;

        if (deadEnemy != null)
            ren.sprite = deadEnemy;

        enemyBody.AddTorque(UnityEngine.Random.Range(deathSpinMin, deathSpinMax));

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach(Collider2D c in colliders)
        {
            c.isTrigger = true;
        }     

        if(UI_100Points != null)
        {
            Instantiate(UI_100Points, transform.position + new Vector3(-2f, 0.1f, 0.0f), Quaternion.identity);
        }
    }
}
