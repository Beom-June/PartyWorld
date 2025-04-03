using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : MonoBehaviour
{
    [SerializeField] private GameObject _btnSetting;

    private bool _settingBool;
    void Start()
    {

    }

    void Update()
    {

    }
    public void SettingBtn()
    {
        _btnSetting.SetActive(_settingBool = !_settingBool);
    }
}
