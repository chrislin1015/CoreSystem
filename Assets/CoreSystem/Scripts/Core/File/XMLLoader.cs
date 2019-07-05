using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Text;

public class XMLLoader<T> : object
{
    protected List<T> m_DataList;
    public List<T> DataList
    {
        get { return m_DataList; }
    }

    protected bool m_IsInitial = false;
    protected string m_FilePath;

    public XMLLoader()
    {
    }

    ~XMLLoader()
    {
        if (m_DataList != null)
        {
            m_DataList.Clear();
            m_DataList = null;
        }
    }

    public void Initial(string iFilePath, bool iIsContent = false)
    {
        if (Application.isEditor && Application.isPlaying == false)
        {
            if (m_DataList != null)
            {
                m_DataList.Clear();
                m_DataList = null;
            }

            if (iIsContent)
            {
                Parse(iFilePath);
            }
            else
            {
                LoadXML(iFilePath);
            }
        }
        else
        {
            if (iIsContent)
            {
                Parse(iFilePath);
            }
            else
            {
                LoadXML(iFilePath);
            }
        }
    }

    public void LoadXML(string iFilePath)
    {
        m_FilePath = iFilePath;
        m_IsInitial = false;

        TextAsset _Text = (TextAsset)Resources.Load(m_FilePath);
        if (_Text == null)
        {
            return;
        }

        Parse(_Text.text);
        Resources.UnloadUnusedAssets();
        m_IsInitial = true;
    }

    public void Parse(string iXMLData, params Type[] iTypes)
    {
        XmlSerializer _XS = new XmlSerializer(typeof(List<T>), iTypes);
        MemoryStream _Memorystream = null;
        _Memorystream = new MemoryStream(StringToUTF8ByteArray(iXMLData));
        m_DataList = (List<T>)_XS.Deserialize(_Memorystream);
        m_IsInitial = true;
    }

    public bool IsInitial()
    {
        return m_IsInitial;
    }

    public T GetData(int iIndex)
    {
        if (m_IsInitial == false)
            return default(T);

        if (iIndex < 0 || iIndex >= m_DataList.Count)
            return default(T);

        T _Data = m_DataList[iIndex];

        return _Data;
    }

    static string UTF8ByteArrayToString(byte[] iCharacters)
    {
        UTF8Encoding _Encoding = new UTF8Encoding();
        string _ConstructedString = _Encoding.GetString(iCharacters);
        return (_ConstructedString);
    }

    static public byte[] StringToUTF8ByteArray(string iXmlstring)
    {
        UTF8Encoding _Encoding = new UTF8Encoding();
        byte[] _Bytes = _Encoding.GetBytes(iXmlstring);
        return _Bytes;
    }
}
