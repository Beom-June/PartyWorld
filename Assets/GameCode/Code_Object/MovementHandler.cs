using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _moventPoints;        //  �̵� ����Ʈ
    [SerializeField] private float _moveSpeed = 5f;                 //  �̵� �ӵ�
    [SerializeField] private bool _isMoving = false;                //  �̵� �� ���� üũ
    void Start()
    {
        if (_moventPoints == null || _moventPoints.Count == 0)
        {
            Debug.LogWarning("Movement points are not set.");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //  �÷��̾�� �ε�����
        if (collider.CompareTag("Player"))
        {
            StartCoroutine(MovePlayerToWaypoints(collider.transform));
        }
    }
    private IEnumerator MovePlayerToWaypoints(Transform player)
    {
        Rigidbody _playerRb = player.GetComponent<Rigidbody>();
        if (_playerRb != null)
        {
            _playerRb.useGravity = false; // �߷� ��Ȱ��ȭ
            _playerRb.velocity = Vector3.zero; // ���� �ӵ� �ʱ�ȭ
        }

        _isMoving = true; // �̵� �� ���� ����

        // �� Waypoint�� �̵�
        foreach (var point in _moventPoints)
        {
            // �÷��̾ ��ǥ ������ ������ ������ �ݺ�
            while (Vector3.Distance(player.position, point.transform.position) > 0.1f)
            {
                // �÷��̾� ��ġ�� ��ǥ �������� �̵�
                player.position = Vector3.MoveTowards(player.position, point.transform.position, _moveSpeed * Time.deltaTime);

                // �̵� �� ȸ�� ó�� (�ɼ�)
                Vector3 direction = (point.transform.position - player.position).normalized;
                if (direction != Vector3.zero)
                {
                    player.rotation = Quaternion.Slerp(player.rotation, Quaternion.LookRotation(direction), Time.deltaTime * _moveSpeed);
                }

                yield return null; // ���� �����ӱ��� ���
            }
        }

        _isMoving = false; // �̵� �Ϸ�
        if (_playerRb != null)
        {
            _playerRb.useGravity = true; // �߷� Ȱ��ȭ
        }
    }
}
