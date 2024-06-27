using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UITextEffect : MonoBehaviour
{
    [SerializeField] private LoopType _lootType;
    [SerializeField] private Text _txt;
    void Start()
    {
        StartBlinking();
    }
    void StartBlinking()
    {
        // �ؽ�Ʈ ���İ��� 0���� �ٿ��ٰ� 1�� ���ƿ��� �ִϸ��̼��� ���� �ݺ�
        _txt.DOFade(0, 1f ).SetLoops(-1, _lootType) 
            .SetEase(Ease.InOutQuad); // �ε巴�� �����ϰ� ������ ��¡ ����
    }
}
