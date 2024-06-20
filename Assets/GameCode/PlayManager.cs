using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private List<int> _sceneIdx;  // 씬 인덱스를 담는 리스트
    [SerializeField] private string _sceneName;        // 현재 씬 이름

    void Awake()
    {
        // 빌드 설정에 있는 모든 씬 인덱스를 자동으로 가져옴
        int _sceneCnt = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < _sceneCnt - 1; i++)
        {
            _sceneIdx.Add(i);
        }
    }
    // 버튼 UI에서 사용하기 위한 함수
    public void PlayButton()
    {
        //  Scene Null 이면 Return
        if (_sceneIdx.Count == 0)
        {
            Debug.LogError(" *** Scene Null !! *** ");
            return;
        }

        // 랜덤으로 씬 인덱스를 선택
        int _randIdx = Random.Range(0, _sceneIdx.Count);
        int _selectedSceneIdx = _sceneIdx[_randIdx];

        // 선택된 씬의 이름을 가져옴
        _sceneName = SceneUtility.GetScenePathByBuildIndex(_selectedSceneIdx);

        // 선택된 씬 로드
        SceneManager.LoadScene(_selectedSceneIdx);

        // 씬 이름을 콘솔에 출력
        Debug.Log(" Scene Name : " + _sceneName);
    }
}