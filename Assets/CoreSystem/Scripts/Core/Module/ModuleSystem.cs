using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ModuleSystem : Singleton<ModuleSystem>
{
    public enum CHANGE_TYPE
    {
        NONE,
        PREPARE,
        CHANGE,
        MAX
    }
    protected const int NULLMODULEID = -1;

    /*
     * This variable is used to store all module
     */
    protected List<ModuleBase> m_ModuleLsit = new List<ModuleBase>();

    /*
     *
     */
    protected int m_CurrentModuleIndex = 0;

    protected int m_ToModuleIndex = 0;

    /*
     * 
     */
    protected ModuleBase m_CurrentModule;

    /*
     * 
     */
    protected int m_NextModule = NULLMODULEID;

    /// <summary>
    /// 在下次ChangeModule前要刪除的場景
    /// </summary>
    protected List<Scene> m_DestroyScene = new List<Scene>();

    protected CHANGE_TYPE e_ChangeType = CHANGE_TYPE.NONE;

    protected Action m_StartChangeModuleCallBack;

    public void RemoveModule(ModuleBase iModule)
    {
        foreach (ModuleBase _M in m_ModuleLsit)
        {
            if (_M == iModule)
            {
                m_ModuleLsit.Remove(_M);
                return;
            }
        }
    }

    new void OnDestroy()
    {
        base.OnDestroy();
        foreach (ModuleBase _M in m_ModuleLsit)
        {
            if (_M == null)
                continue;
        }

        m_ModuleLsit.Clear();
        m_ModuleLsit = null;
    }

    void Awake()
    {

    }

    public void RegisterModule(ModuleBase iModule)
    {
        if (iModule == null)
            return;

        foreach (ModuleBase _MB in m_ModuleLsit)
        {
            if (_MB.ModuleID == iModule.ModuleID)
            {
                return;
            }
        }

        m_ModuleLsit.Add(iModule);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrentModule != null)
        {
            m_CurrentModule.DoRun();
        }

        if (e_ChangeType != CHANGE_TYPE.NONE)
        {
            if (e_ChangeType == CHANGE_TYPE.PREPARE)
            {
                if (m_StartChangeModuleCallBack != null)
                {
                    m_StartChangeModuleCallBack();
                    m_StartChangeModuleCallBack = null;
                }
                e_ChangeType = CHANGE_TYPE.CHANGE;
            }
            else if (e_ChangeType == CHANGE_TYPE.CHANGE)
            {
                if (m_NextModule != NULLMODULEID)
                {
                    StartChangeModule(m_NextModule);
                    m_NextModule = NULLMODULEID;
                    e_ChangeType = CHANGE_TYPE.NONE;
                }
            }
        }
    }

    protected void StartChangeModule(int id)
    {
        m_ToModuleIndex = id;

        if (m_CurrentModule != null)
        {
            m_CurrentModule.DoLastRun();
        }

        DoChangeModule();
    }

    protected void DoChangeModule()
    {
        int _Index = 0;
        foreach (ModuleBase _MB in m_ModuleLsit)
        {
            if (_MB.ModuleID == m_ToModuleIndex)
            {
                m_CurrentModuleIndex = _Index;
                m_CurrentModule = _MB;
                m_CurrentModule.DoFirstRun();
                break;
            }
            _Index++;
        }
    }

    protected void ChangeModule()
    {
        if (m_NextModule != NULLMODULEID)
        {
            e_ChangeType = CHANGE_TYPE.PREPARE;
        }
    }

    public void ReadyChangeModule(int iNextID, Action iCallback)
    {
        m_NextModule = iNextID;

        if (iCallback != null)
        {
            m_StartChangeModuleCallBack = iCallback;
        }

        FadeSystem.Instance.FadeOut(ChangeModule);
    }

    public ModuleBase CurrentModule()
    {
        return m_CurrentModule;
    }

    public int CurrentMoudleIndex()
    {
        return m_CurrentModuleIndex;
    }

    /// <summary>
    /// 將場景列入即將刪除的場景清單中
    /// </summary>
    /// <param name="iScene"></param>
    public void AddDestroyScene(Scene iScene)
    {
        if (iScene == null)
            return;

        if (m_DestroyScene.Contains(iScene))
            return;

        m_DestroyScene.Add(iScene);
    }

    /// <summary>
    /// 將場景從即將刪除的場景清單中移除
    /// </summary>
    /// <param name="iScene"></param>
    public void RemoveDestroyScene(Scene iScene)
    {
        if (iScene == null)
            return;

        if (m_DestroyScene.Contains(iScene) == false)
            return;

        m_DestroyScene.Remove(iScene);
    }

    /// <summary>
    /// 移除清單中所有資料
    /// </summary>
    public void ClearAllDestroyScenes()
    {
        m_DestroyScene.Clear();
    }

    public void StartUnloadScene()
    {
        if (m_DestroyScene.Count > 1)
        {
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
            SceneManager.UnloadSceneAsync(m_DestroyScene[0]);
        }
    }

    void OnSceneUnLoaded(Scene iScene)
    {
        if (m_DestroyScene.Contains(iScene))
        {
            m_DestroyScene.Remove(iScene);
        }

        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
    }
}
