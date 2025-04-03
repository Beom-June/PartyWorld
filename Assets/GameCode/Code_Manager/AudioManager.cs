using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  �ν��Ͻ��� ���������� Ÿ ��ũ��Ʈ���� �ʿ��� ���� ȣ���
//  AudioManager.Instance.�Լ���~
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
        // AudioManager -> Music SOurce�� ���� �뷡
        PlayMusic("BGM_01");
    }
    //  Game BGM ��� �޼ҵ�
    public void PlayMusic(string name)
    {
        Sound _sound = Array.Find(_musicSounds, x => x._name == name);

        if (_sound == null)
        {
            Debug.Log(" ***** Sound Not Found ***** ");
        }
        else
        {
            //  ���� �뷡 �߰��ϰ� �������� ����
            _musicSource.clip = _sound._clip;
            _musicSource.Play();
        }
    }

    //  Game SFX ��� �޼ҵ�
    public void PlaySFX(string name)
    {
        Sound _sound = Array.Find(_sfxSounds, x => x._name == name);

        if (_sound == null)
        {
            Debug.Log(" ***** SFXSounds Not Found ***** ");
        }
        else
        {
            //  ���� �뷡 �߰��ϰ� �������� ����
            _sfxSource.PlayOneShot(_sound._clip);
        }
    }

    //  �ش� �̹��� ��ư ������ �Ҹ� ��
    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
    }
    public void ToggleSFX()
    {
        _sfxSource.mute = !_sfxSource.mute;
    }

    //  �Ҹ� ����
    public void MusicVolume(float volume)
    {
        _musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        _sfxSource.volume = volume;
    }
}
