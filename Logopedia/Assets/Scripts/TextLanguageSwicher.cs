using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextLanguageSwicher : MonoBehaviour
{
    [SerializeField]
    private Text _buttonText;
    [SerializeField]
    private TMP_Text _text;

    private void Awake()
    {
        SwichLanguage();
    }

    private void Start()
    {
        SwichLanguage();
    }

    private void OnEnable()
    {
        SwichLanguage();
    }
    public void SwichLanguage()
    {
        StartCoroutine(SwichText());
    }

    IEnumerator SwichText()
    {
        yield return new WaitForEndOfFrame();
        _text.text = _buttonText.text;
    }
}
