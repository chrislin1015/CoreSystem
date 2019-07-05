using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Localization : MonoBehaviour
{
    public string m_Category;
    public string m_Key;
    protected Text m_Label;

    void Awake()
    {
        m_Label = GetComponent<Text>();
    }

    void Start()
    {
        //註冊語系轉換事件Callback
        if (LocalizationSystem.Instance != null)
        {
            LocalizationSystem.Instance.AddEvent(DoChange);
            DoChange();
        }
    }

    void DoChange()
    {
        if (m_Label != null && LocalizationSystem.Instance != null)
        {
            m_Label.text = LocalizationSystem.Instance.GetText(m_Category, m_Key);
        }
    }
}
