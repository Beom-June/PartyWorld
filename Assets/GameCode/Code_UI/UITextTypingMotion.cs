using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITextTypingMotion : MonoBehaviour
{
    [SerializeField] private Text _txtLoading;
    [SerializeField] private string _loadingText = "Loading....";  // �ݺ��� �ؽ�Ʈ

    void Start()
    {
        StartCoroutine(TypingLoop());
    }

    IEnumerator TypingLoop()
    {
        while (true)
        {
            _txtLoading.text = string.Empty;

            // DOTween�� ����Ͽ� �ؽ�Ʈ �ִϸ��̼�
            yield return _txtLoading.DOText(_loadingText, 2.5f).WaitForCompletion();

            // ���� ��� ������
            yield return new WaitForSeconds(1.0f);
        }
    }
}
