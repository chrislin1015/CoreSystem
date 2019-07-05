using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModuleData
{
    public int ModuleID;
    public int AssetType;
    public string AssetName;
    public string AttachName;

    public ModuleData()
    {
    }
}

[CreateAssetMenu(fileName = "ModuleData", menuName = "CoreSystem/ModuleData", order = 1)]
public class ModuleDataSO : ScriptableObject
{
    public List<ModuleData> m_ModuleDatas;

    public void Awake()
    {
        Debug.Log("Awake");
    }

    public void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    public void OnDisable()
    {
        Debug.Log("OnDisable");
    }

    public void OnDestroy()
    {
        Debug.Log("OnDestroy");

        if (m_ModuleDatas != null)
        {
            m_ModuleDatas.Clear();
            m_ModuleDatas = null;
        }
    }

    public void DataFromXML(string iFileContent)
    {
        XMLLoader<ModuleData> _DataLoader = new XMLLoader<ModuleData>();
        _DataLoader.Initial(iFileContent, true);

        if (m_ModuleDatas != null)
        {
            m_ModuleDatas.Clear();
            m_ModuleDatas = null;
        }

        m_ModuleDatas = new List<ModuleData>();
        foreach (ModuleData _Data in _DataLoader.DataList)
        {
            m_ModuleDatas.Add(_Data);
        }

        _DataLoader = null;
    }
}
