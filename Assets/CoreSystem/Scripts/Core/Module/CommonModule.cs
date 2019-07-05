using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommonModule : ModuleBase
{
    public enum ASSET_TYPE
    {
        PREFABE,
        SCENE,
        MAX,
    }

    static public bool s_IsReady = false;

    public ASSET_TYPE e_AssetType;
    public string m_AssetName;
    //public string m_AttachName;
    protected GameObject m_MainObject;

    /*
     * Load a xml file to create modules
     */
    static public void CreateModules()
    {
        s_IsReady = false;
        //PrefabLoader.LoadPrefab("ModuleData", LoadModueleDataFinish);
    }

    public CommonModule()
    {
    }

    ~CommonModule()
    {
    }

    public override void DoFirstRun()
    {
        if (e_AssetType == ASSET_TYPE.PREFABE)
        {
            PrefabLoader.LoadPrefabWithInstance(m_AssetName, LoadPrefabComplete);
        }
        else if (e_AssetType == ASSET_TYPE.SCENE)
        {
            SceneLoader.LoadSceneAsync(m_AssetName, LoadSceneMode.Additive, LoadSceneComplete);
        }
    }

    public override void DoLastRun()
    {
        if (m_MainObject != null)
        {
            GameObject.Destroy(m_MainObject);
            m_MainObject = null;
        }
    }

    public override void DoRun()
    {
    }

    /*
     * This method is call back of load prefab
     * @param 1. loaded prefab object
     */
    protected void LoadPrefabComplete(GameObject iObj)
    {
        if (iObj == null)
        {
            FadeSystem.Instance.FadeIn(FadeInFinish);
            return;
        }

        m_MainObject = iObj;
        if (MainSystem.Instance != null)
        {
            GameObject _Root = MainSystem.Instance.gameObject;
            m_MainObject.transform.SetParent(_Root.transform);
        }

        m_MainObject.transform.localScale = Vector3.one;
        m_MainObject.transform.localPosition = Vector3.zero;
        m_MainObject.transform.localRotation = Quaternion.identity;

        FadeSystem.Instance.FadeIn(FadeInFinish);

        if (m_MainObject != null)
        {
            m_MainObject.SendMessage("Loaded", SendMessageOptions.DontRequireReceiver);
        }
    }

    protected void LoadSceneComplete(Scene iScene)
    {
        GameObject[] _Roots = iScene.GetRootGameObjects();
        foreach (GameObject _Object in _Roots)
        {
            if (_Object.tag.Equals("Module"))
            {
                m_MainObject = _Object;
                m_MainObject.SendMessage("Loaded", SendMessageOptions.DontRequireReceiver);
                break;
            }
        }

        ModuleSystem.Instance.AddDestroyScene(iScene);
        FadeSystem.Instance.FadeIn(FadeInFinish);
    }

    protected void FadeInFinish()
    {
        if (m_MainObject != null)
        {
            m_MainObject.SendMessage("Initial", SendMessageOptions.DontRequireReceiver);
        }
    }

    /*protected static void LoadModueleDataFinish(GameObject iObj)
    {
        if (iObj == null)
            return;

        GameObject _New = GameObject.Instantiate(iObj);

        foreach (ModuleData _Moduledata in ModuleDataXML.Instance.m_ModuleDataList)
        {
            CommonModule _Newmodule = new CommonModule();
            if (_Newmodule == null)
                continue;

            _Newmodule.m_ModuleID = _Moduledata.ModuleID;
            _Newmodule.e_AssetType = (ASSET_TYPE)_Moduledata.AssetType;
            _Newmodule.m_AssetName = _Moduledata.AssetName;
            //_Newmodule.m_AttachName = _Moduledata.AttachName;
            ModuleSystem.Instance.RegisterModule(_Newmodule);
        }

        s_IsReady = true;

        GameObject.Destroy(_New);
    }*/
}
