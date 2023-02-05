using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMProForFooter : MonoBehaviour
{
    public TMP_InputField _inputfield;

    private void Start()
    {
        _inputfield.text = Application.dataPath;
    }
}
