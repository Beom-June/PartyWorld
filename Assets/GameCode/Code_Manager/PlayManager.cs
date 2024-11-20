using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private List<int> _sceneIdx;  // �� �ε����� ��� ����Ʈ
    [SerializeField] private string _sceneName;        // ���� �� �̸�

    [SerializeField] private GameObject _checking;
    void Awake()
    {
        // ���� ������ �ִ� ��� �� �ε����� �ڵ����� ������
        int _sceneCnt = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < _sceneCnt - 1; i++)
        {
            _sceneIdx.Add(i);
        }
    }

    public void PlayButton()
    {
        _checking.SetActive(true);
    }
    //  PlayButton. Loading Scene or Loading UI �߰� �� (���)
    public void YesButton()
    {
        SceneManager.LoadScene("Scene_Loading");
    }
    public void NoButton()
    {
        _checking.SetActive(false);
    }
    // Play Button. �� �������� ����
    public void GoToRandomScene()
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