using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonColor : MonoBehaviour
{
    private Button _button;
    private Color _originalColor;
    private bool _isToggled = false;

    void Start()
    {
        _button = GetComponent<Button>();
        _originalColor = _button.colors.normalColor;
        _button.onClick.AddListener(ToggleColor);
    }

    void ToggleColor()
    {
        var colorBlock = _button.colors;
        colorBlock.normalColor = _isToggled ? _originalColor : Color.gray;
        _button.colors = colorBlock;
        _isToggled = !_isToggled;
    }
}
