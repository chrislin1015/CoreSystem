using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSystem : Singleton<LogSystem>
{
    public int m_MaxLogCount = 0;
    protected List<string> m_LogList = new List<string>();
    public List<string> LogList
    {
        get { return m_LogList; }
    }
        
    protected List<string> m_WarningList = new List<string>();
    public List<string> WarningList
    {
        get { return m_WarningList; }
    }

    protected List<string> m_ErrorList = new List<string>();
    public List<string> ErrorList
    {
        get { return m_ErrorList; }
    }

    new protected void OnDestroy()
    {
        base.OnDestroy();

        m_LogList.Clear();
        m_LogList = null;

        m_WarningList.Clear();
        m_WarningList = null;

        m_ErrorList.Clear();
        m_ErrorList = null;
    }

    public void Log(string iLog)
    {
        if (string.IsNullOrEmpty(iLog))
            return;

#if UNITY_EDITOR
        Debug.Log(iLog);
#endif
        m_LogList.Add(iLog);

        RemoveLastest(m_LogList);
    }

    public void Warnning(string iWarning)
    {
        if (string.IsNullOrEmpty(iWarning))
            return;

#if UNITY_EDITOR
        Debug.LogWarning(iWarning);
#endif
        m_WarningList.Add(iWarning);

        RemoveLastest(m_WarningList);
    }

    public void Error(string iError)
    {
        if (string.IsNullOrEmpty(iError))
            return;

#if UNITY_EDITOR
        Debug.LogError(iError);
#endif
        m_ErrorList.Add(iError);

        RemoveLastest(m_ErrorList);
    }

    protected void RemoveLastest(List<string> iList)
    {
        if (m_MaxLogCount > 0)
        {
            if (iList.Count > m_MaxLogCount)
            {
                iList.RemoveAt(0);
            }
        }
    }
}
