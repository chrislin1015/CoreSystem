using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LocalizationSystem : Singleton<LocalizationSystem>
{
    public enum LOAD_TYPE
    {
        REPLACE,
        APPEND,
    }

    [SerializeField]
    protected TextAsset[] m_LocalizationDatas;
    [SerializeField]
    protected int m_LanguageIndex = 0;

    protected event Action m_Event;
    protected Dictionary<string, LocalizationData> m_LocalizationDataDic;
    protected Action m_LoadedCB = null;

    private void Awake()
    {
        m_LocalizationDataDic = new Dictionary<string, LocalizationData>();

        if (m_LocalizationDatas != null)
        {
            foreach (TextAsset _TextAsset in m_LocalizationDatas)
            {
                LoadDataByText(_TextAsset.name, _TextAsset.text);
            }
        }
    }

    new protected void OnDestroy()
    {
        base.OnDestroy();

        m_LocalizationDataDic.Clear();
        m_LocalizationDataDic = null;

        m_Event = null;
    }

    public void AddData(GameObject iObj)
    {
        if (iObj == null)
            return;

        if (m_LocalizationDataDic.ContainsKey(iObj.name))
        {
            GameObject.Destroy(iObj);
            return;
        }

        LocalizationData _LD = iObj.GetComponent<LocalizationData>();
        if (_LD == null)
        {
            GameObject.Destroy(iObj);
            return;
        }

        m_LocalizationDataDic.Add(iObj.name, _LD);

        if (m_LoadedCB != null)
        {
            m_LoadedCB();
            m_LoadedCB = null;
        }
    }

    public void LoadData(string iPath, System.Action iCallback = null)
    {
        if (iCallback != null)
        {
            m_LoadedCB = iCallback;
        }
        PrefabLoader.LoadPrefabWithInstanceNOCoroutine(iPath, AddData);
    }

    public void LoadDataByText(string iName, string iText, System.Action iCallback = null)
    {
        if (iCallback != null)
        {
            m_LoadedCB = iCallback;
        }
        GameObject _Obj = new GameObject();
        _Obj.name = iName;
        LocalizationData _L = _Obj.AddComponent<LocalizationData>();
        _L.LoadTable(iText);
        AddData(_Obj);
    }

    public void RemoveData(string iCategory)
    {
        foreach (KeyValuePair<string, LocalizationData> _KV in m_LocalizationDataDic)
        {
            if (_KV.Key == null || _KV.Value == null)
                continue;

            if (_KV.Key.Equals(iCategory))
            {
                GameObject _GO = _KV.Value.gameObject;
                m_LocalizationDataDic.Remove(_KV.Key);
                GameObject.Destroy(_GO);

                return;
            }
        }
    }

    public void ChangeLanguage(int iLanguageIndex)
    {
        m_LanguageIndex = iLanguageIndex;
        m_Event();
    }

    public string GetText(string iCategory, string iKey)
    {
        if (m_LocalizationDataDic.ContainsKey(iCategory))
        {
            return m_LocalizationDataDic[iCategory].GetText(iKey, m_LanguageIndex);
        }

        return "";
    }

    public string GetText(string iKey)
    {
        foreach (KeyValuePair<string, LocalizationData> _KV in m_LocalizationDataDic)
        {
            string _TempText = _KV.Value.GetText(iKey, m_LanguageIndex);
            if (string.IsNullOrEmpty(_TempText) == false)
            {
                return _TempText;
            }
        }

        return "";
    }

    public void AddEvent(Action iAction)
    {
        m_Event += iAction;
    }

    public void RemoveEvent(Action iAction)
    {
        m_Event -= iAction;
    }
}
