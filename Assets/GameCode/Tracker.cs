using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] private Transform from, to;
    [SerializeField] private float tweenTime;

    [SerializeField] private Transform satellite;
    [SerializeField] private Transform satellite2;

    [SerializeField] private float _elapsedTime = 0f;        //  ��� �ð�


    void Update()
    {
        //  1.������ �ð� tweenTime ���� from -> to �� �������� �̵��ϴ� Tracker Ŭ������ �����ϼ���.
        if (_elapsedTime < tweenTime)
        {
            _elapsedTime += Time.deltaTime;
            //  �ð� ����ȭ
            float _time = Mathf.Clamp01(_elapsedTime / tweenTime);
            transform.position = Vector3.Lerp(from.position, to.position, _time);

            // 2. �̵��ϴ� ������ �������� �������� �����ϰ� 1��ŭ ������ ��ġ�� �ִ� satellite�� ��ġ�� ����
            //  �Ÿ� ����ȭ
            Vector3 _dir = (to.position - from.position).normalized;
            // �̵� ���⿡ ���� �̵��� ��ġ
            Vector3 _sugic = new Vector3(_dir.y, -_dir.x - 1, 0f); //   �� �κ��� �� ����
            //  �̵��ϴ� ��ġ�� ���� �̵��� ���� ������
            Vector3 _satellitePos = transform.position + _sugic;
            //  ��ġ ������Ʈ
            satellite.position = _satellitePos;
        }
        // 3. satellite2�� ȸ���ϵ��� ����
        if (_elapsedTime < tweenTime)
        {
            // 5ȸ ȸ��
            float _angle = 360f * 5f * (_elapsedTime / tweenTime);
            //  �Ÿ� 3��ŭ ������
            satellite2.position = transform.position + new Vector3(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad), 0f) * 3f;
        }
    }

    //  ���� �Լ��� �߰� ���� ���� ����Ͻ� �� �ֽ��ϴ�
    Vector3 Cross(Vector3 lhs, Vector3 rhs)
    {
        return Vector3.Cross(lhs, rhs);
    }

    Vector3 Normalize(Vector3 value)
    {
        return value.normalized;
    }

    float Sin(float f)
    {
        return Mathf.Sin(f);
    }

    float Cos(float f)
    {
        return Mathf.Cos(f);
    }
}