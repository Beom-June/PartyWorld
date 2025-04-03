using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  인스턴스르 선언했으니 타 스크립트에서 필요한 사운드 호출시
//  AudioManager.Instance.함수명~
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Sound")]
    [SerializeField] private Sound[] _musicSounds;
    [SerializeField] private Sound[] _sfxSounds;

    [Header("AudioSource")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // AudioManager -> Music SOurce에 넣은 노래
        PlayMusic("BGM_01");
    }
    //  Game BGM 재생 메소드
    public void PlayMusic(string name)
    {
        Sound _sound = Array.Find(_musicSounds, x => x._name == name);

        if (_sound == null)
        {
            Debug.Log(" ***** Sound Not Found ***** ");
        }
        else
        {
            //  추후 노래 추가하고 랜덤으로 수정
            _musicSource.clip = _sound._clip;
            _musicSource.Play();
        }
    }

    //  Game SFX 재생 메소드
    public void PlaySFX(string name)
    {
        Sound _sound = Array.Find(_sfxSounds, x => x._name == name);

        if (_sound == null)
        {
            Debug.Log(" ***** SFXSounds Not Found ***** ");
        }
        else
        {
            //  추후 노래 추가하고 랜덤으로 수정
            _sfxSource.PlayOneShot(_sound._clip);
        }
    }

    //  해당 이미지 버튼 누르면 소리 끔
    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
    }
    public void ToggleSFX()
    {
        _sfxSource.mute = !_sfxSource.mute;
    }

    //  소리 조절
    public void MusicVolume(float volume)
    {
        _musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        _sfxSource.volume = volume;
    }
}
