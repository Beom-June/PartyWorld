using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITextTypingMotion : MonoBehaviour
{
    [SerializeField] private Text _txtLoading;
    [SerializeField] private string _loadingText = "Loading....";  // 반복할 텍스트

    void Start()
    {
        StartCoroutine(TypingLoop());
    }

    IEnumerator TypingLoop()
    {
        while (true)
        {
            _txtLoading.text = string.Empty;

            // DOTween을 사용하여 텍스트 애니메이션
            yield return _txtLoading.DOText(_loadingText, 2.5f).WaitForCompletion();

            // 다음 대사 딜레이
            yield return new WaitForSeconds(1.0f);
        }
    }
}
