using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private List<int> _sceneIdx;  // �� �ε����� ��� ����Ʈ
    [SerializeField] private string _sceneName;        // ���� �� �̸�

    void Awake()
    {
        // ���� ������ �ִ� ��� �� �ε����� �ڵ����� ������
        int _sceneCnt = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < _sceneCnt - 1; i++)
        {
            _sceneIdx.Add(i);
        }
    }
    // ��ư UI���� ����ϱ� ���� �Լ�
    public void PlayButton()
    {
        //  Scene Null �̸� Return
        if (_sceneIdx.Count == 0)
        {
            Debug.LogError(" *** Scene Null !! *** ");
            return;
        }

        // �������� �� �ε����� ����
        int _randIdx = Random.Range(0, _sceneIdx.Count);
        int _selectedSceneIdx = _sceneIdx[_randIdx];

        // ���õ� ���� �̸��� ������
        _sceneName = SceneUtility.GetScenePathByBuildIndex(_selectedSceneIdx);

        // ���õ� �� �ε�
        SceneManager.LoadScene(_selectedSceneIdx);

        // �� �̸��� �ֿܼ� ���
        Debug.Log(" Scene Name : " + _sceneName);
    }
}