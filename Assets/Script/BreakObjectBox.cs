using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObjectBox : EnemyController
{

    void Awake()
    {
        En = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        // animation = GetComponent<Animator>();
        BC = GetComponent<BoxCollider2D>();
        EnemyCurrentHp = EnemyHP;
        hp = EnemyCurrentHp;
        enemyHpSlider.gameObject.SetActive(false);

    }
    void Update()
    {
        enemyHpSlider.maxValue = EnemyHP;
        enemyHpSlider.value = EnemyCurrentHp;
        enemyHpSlider.transform.position = hudPos.position;

        if (EnemyCurrentHp != hp && EnemyCurrentHp > 0)
        {
            float dmg = EnemyCurrentHp - hp;
            GameObject hudText = Instantiate(hudDamageText);
            hudText.transform.position = hudPos.position;
            hudText.GetComponent<DamageText>().damage = (int)dmg;
            EnemyCurrentHp = hp;

            ActiveHpSlider();

            if (EnemyCurrentHp <= 0)
            {
                audio.PlaySound("OBJBREAK");
                SR.color = new Color(1, 0, 1, 0.5f);
                // SR.flipY = true;
                BC.enabled = false;
                Invoke("DeActive", 0.1f);
                Debug.Log("get");
                Invoke("UnActiveHpSlider", 1);
                Debug.Log("get1");
                StartCoroutine(DropItem(droppercent));
                Debug.Log("get2");
                Invoke("DropMoney", 0.1f); 
            }
            else
            {
                Invoke("UnActiveHpSlider", 5);
                SR.color = new Color(1, 0.9f, 0.02f, 0.5f);
                Invoke("ReturnEnemyColor", 1);
            }
        }

    }



}
