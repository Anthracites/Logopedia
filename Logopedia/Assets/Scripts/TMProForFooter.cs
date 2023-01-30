using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMProForFooter : MonoBehaviour
{
    public TMP_Text _pathTMPro;

    private void Start()
    {
        _pathTMPro.text = Application.dataPath;
    }
}
