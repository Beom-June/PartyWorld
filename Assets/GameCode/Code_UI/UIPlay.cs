using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    [SerializeField] private GameObject _checking;

    //  play btn¿ª ¥©∏£∏È yes btn¿Ã ∂‰
    public void PlayButton()
    {
        _checking.SetActive(true);
    }
    //  PlayButton. Loading Scene or Loading UI ∂ﬂ∞‘ «‘ (∞ÌπŒ)
    public void YesButton()
    {
        SceneManager.LoadScene("Scene_Loading");
    }
    public void NoButton()
    {
        _checking.SetActive(false);
    }
}