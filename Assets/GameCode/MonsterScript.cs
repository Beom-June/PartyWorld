using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public Animator anim;
    public GameObject player;
    public GameObject Hit_Muzzle;
    public GameObject Monsters;
    public Transform Muzzle_Pos;

    public int Health;
    public int MaxHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Monsters = GameObject.FindGameObjectWithTag("Monster");
        MaxHealth = Health;                                     // ü�� ����, �� ���� �ɶ� �ʱ�ȭ ��Ű�� ���ؼ�

        anim = GetComponent<Animator>();
        player.GetComponent<PlayerScript>().Monster = gameObject;

        //GetComponent<GameManager>();
        Regen();

    }


    public void GetDamage(int damage)
    {
        // ��ƼŬ ����
        GameObject muz = Instantiate(Hit_Muzzle, Muzzle_Pos.position, Quaternion.identity);
        Destroy(muz, 1f);

        Health -= damage;
        if (Health <= 0)
        {
            DEAD_Anim();
            player.GetComponent<PlayerScript>().Monster = null;
            //Destroy(gameObject, 2f);

            GameManager.Stage++;
            Invoke("Monster_Hide", 2f);
            Invoke("Regen", 1f);
        }
    }

    // ���� �׾��� ��, �ִϸ��̼�
    void DEAD_Anim()
    {
        anim.SetInteger("doDie", 1);
    }

    // object Hide -> ������Ʈ�� �ı� �� �� ó��
    public void Monster_Hide()
    {
        Health = MaxHealth;                   // ü�� ���� ������ �ʱ�ȭ
        gameObject.SetActive(false);            // ������Ʈ ��
    }

    // ���� ���� ���� & ���� -> NextStageMonsterMake
    public void Regen()
    {
        for (int i = 1; i < Monsters.transform.childCount; i++)
        {
            if (GameManager.Stage == i)
            {
                Monsters.transform.GetChild(GameManager.Stage - 1).gameObject.SetActive(true);
                break;
            }
        }
    }
}
