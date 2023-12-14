using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player"; // �÷��̾ �ĺ��ϱ� ���� �±�
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Vector3 _offset;

    void FixedUpdate()
    {
        GameObject _playerObj = GameObject.FindGameObjectWithTag(_playerTag);
        // �÷��̾� ������Ʈ�� ã�Ҵٸ� �ش� Ʈ�������� �����մϴ�.
        if (_playerObj != null)
        {
            _playerTransform = _playerObj.transform;
        }

        // �÷��̾ ã�Ҵٸ� ��ġ�� ������Ʈ�մϴ�.
        if (_playerTransform != null)
        {
            transform.position = _playerTransform.position + _offset;
        }
    }
}
