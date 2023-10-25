using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Animator animator;
    public GameObject Monster;                  // ���� �±׷� ã�� ������� ����
    public int Damage;                          // ���Ϳ��� �ִ� Damage

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Monster == null)
        {
            Idle_Anim();
        }
        else if (Monster != null)
        {
            Attack_Anim();
        }
    }

    public void SetDamage()
    {
        Monster.GetComponent<MonsterScript>().GetDamage(Damage);
    }

    // Player Idle
    void Idle_Anim()
    {
        // "" �ȿ� Animator �̸� �޾ƿ�
        animator.SetInteger("doAttack", 0);
    }

    // Player Attack
    void Attack_Anim()
    {
        animator.SetInteger("doAttack", 1);
    }
}
