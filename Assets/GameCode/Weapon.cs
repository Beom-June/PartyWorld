using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    meleeAttack,                                    // ���� ����
    rangeAttack                                     // ���Ÿ� ����
};
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private int _damage;                                  // ���� ������
    [SerializeField] private float _attackDelay;                           // ���� ������
    [SerializeField] private BoxCollider _attackArea;                      // ���� ����
    [SerializeField] private TrailRenderer _trailRenderer;                 // ���� ȿ��

    #region Property
    public int Damage
    {
        get { return _damage; }
    }

    public float AttackDelay
    {
        get { return _attackDelay; }
    }
    #endregion

    public void UseWeapon()
    {
        if (_weaponType == WeaponType.meleeAttack)
        {
            StopCoroutine(Swing());
            StartCoroutine(Swing());
        }
    }

    // ���� ���� �ڷ�ƾ
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        _attackArea.enabled = true;
        _trailRenderer.enabled = true;

        yield return new WaitForSeconds(0.3f);
        _attackArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        _trailRenderer.enabled = false;
    }

    // ���Ÿ� ���� �ڷ�ƾ
}
