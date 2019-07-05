using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleBase
{
    public delegate void OnDestroyDelegate(ModuleBase iModule);
    public static event OnDestroyDelegate m_OnDestroyDelegate;

    protected int m_ModuleID;
    public int ModuleID
    {
        get { return m_ModuleID; }
    }

    public ModuleBase()
    {
    }

    ~ModuleBase()
    {
        if (m_OnDestroyDelegate != null)
        {
            m_OnDestroyDelegate(this);
        }
    }

    public virtual void DoFirstRun()
    {
    }

    public virtual void DoLastRun()
    {
    }

    public virtual void DoRun()
    {
    }
}
