using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private List<Scene> _sceneList;            //  씬 담는 리스트
    [SerializeField] private string _sceneName;                 //  현재 씬 이름

    public void PlayButton()
    {
        for (int i = 0; i < _sceneList.Count; i++)
        {

        }
    }
}
