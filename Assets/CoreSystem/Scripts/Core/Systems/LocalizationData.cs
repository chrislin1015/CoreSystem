using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LanguageData
{
    public string m_Key;
    public string[] m_Language;

    ~LanguageData()
    {
        m_Language = null;
    }

    public LanguageData(string[] iData)
    {
        int _Size = iData.Length;
        m_Language = new string[_Size - 1];

        for (int i = 0; i < _Size; ++i)
        {
            if (i == 0)
            {
                m_Key = iData[i];
            }
            else
            {
                m_Language[i - 1] = iData[i].Replace("\\n", "\n");
            }
        }
    }
}

public class LocalizationData : MonoBehaviour
{
    protected Dictionary<string, LanguageData> m_LanguageDataDic;

    private void Awake()
    {
        m_LanguageDataDic = new Dictionary<string, LanguageData>();
    }

    void OnDestroy()
    {
        m_LanguageDataDic.Clear();
        m_LanguageDataDic = null;
    }

    public void LoadTable(string iSourceData)
    {
        char[] _RecordSeparator = { '\n' };
        char[] _FieldSeparator = { '\t' };

        string[] _Records = iSourceData.Split(_RecordSeparator);
        List<LanguageData> _TempDataList = new List<LanguageData>();

        foreach (string _Record in _Records)
        {
            string[] _Fields = _Record.Split(_FieldSeparator);
            _TempDataList.Add(new LanguageData(_Fields));
        }

        LanguageData[] _LanguageArray = _TempDataList.ToArray();

        foreach (LanguageData _Language in _LanguageArray)
        {
            if (string.IsNullOrEmpty(_Language.m_Key))
                continue;

            if (m_LanguageDataDic.ContainsKey(_Language.m_Key))
            {
                m_LanguageDataDic[_Language.m_Key] = _Language;
            }
            else
            {
                m_LanguageDataDic.Add(_Language.m_Key, _Language);
            }
        }

        _LanguageArray = null;
        _TempDataList.Clear();
        _TempDataList = null;
    }

    public string GetText(string iKey, int iIndex)
    {
        if (m_LanguageDataDic.ContainsKey(iKey))
        {
            if (iIndex < 0 || iIndex >= m_LanguageDataDic[iKey].m_Language.Length)
            {
                return "";
            }
            return m_LanguageDataDic[iKey].m_Language[iIndex];
        }

        return "";
    }
}
