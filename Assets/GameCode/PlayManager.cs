using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private List<Scene> _sceneList;            //  �� ��� ����Ʈ
    [SerializeField] private string _sceneName;                 //  ���� �� �̸�

    public void PlayButton()
    {
        for (int i = 0; i < _sceneList.Count; i++)
        {

        }
    }
}
