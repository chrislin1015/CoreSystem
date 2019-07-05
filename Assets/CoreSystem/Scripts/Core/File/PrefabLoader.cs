using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 此類別用來執行Prefab物件載入的工作
 * 使用方式
 * 1. 呼叫 PrefabLoader.LoadPrefab("FileName", "FolderName", Callback); <-- 載入一個Prefab但不實體化，直接回傳載入的Prefab給Callback
 *    或   PrefabLoader.LoadPrefabWithInstance("FileName", "FolderName", Callback); <-- 載入一個Prefab並直接實體化，回傳實體化後的GameObject給Callback
 * 2. 撰寫Callback函式
 *    void Callback(GameObject iObj)
 *    {
 *       m_MyObj = iObj;
 *    }
 */
public class PrefabLoader : FileLoaderBase
{
    /*
     * 定義delegate，用來當載入完畢時的Callback
     */
    public delegate void AsyncLoadingPrefabComplete(GameObject obj);
    public event AsyncLoadingPrefabComplete m_PrefabLoadingCB;

    /*
     * 是否直接實體化
     */
    protected bool m_IsInstance = false;

    /*
     * 載入Prefab
     * @parameter 1. 檔名. 2. 資料夾或路徑. 3. Callback函式. 4. 物件名稱，用來辨識 
     */
    static public void LoadPrefab(string iFilename, AsyncLoadingPrefabComplete iCallback = null, string iObjname = "Load Prefab", bool iIsFromResources = true)
    {
        GameObject _Loader = new GameObject(iObjname);
        PrefabLoader _Script = _Loader.AddComponent<PrefabLoader>();
        if (_Script != null)
        {
            _Script.LoadPrfabByAsync(iFilename, iCallback, false, iIsFromResources);
        }
    }

    /*
     * 載入Prefab並直接實體化一份回傳
     * @parameter 1. 檔名. 2. 資料夾或路徑. 3. Callback函式. 4. 物件名稱，用來辨識
     */
    static public void LoadPrefabWithInstance(string iFilename, AsyncLoadingPrefabComplete iCallback = null, string iObjname = "Load Prefab", bool iIsFromResources = true)
    {
        GameObject _Loader = new GameObject(iObjname);
        PrefabLoader _Script = _Loader.AddComponent<PrefabLoader>();
        if (_Script != null)
        {
            _Script.LoadPrfabByAsync(iFilename, iCallback, true, iIsFromResources);
        }
    }

    static public void LoadPrefabWithInstanceNOCoroutine(string iFilename, AsyncLoadingPrefabComplete iCallback = null, string iObjname = "Load Prefab", bool iIsFromResources = true)
    {
        GameObject _Loader = new GameObject(iObjname);
        PrefabLoader _Script = _Loader.AddComponent<PrefabLoader>();
        if (_Script)
        {
            _Script.LoadPrfab(iFilename, iCallback, true, iIsFromResources);
        }
    }

    /*
     * 建立一個Coroutine來載入檔案並設定變數
     * @parameter 1. 檔名. 2. 資料夾或路徑. 3. Callback函式. 4. 是否直接實體化
     */
    protected void LoadPrfabByAsync(string iName, AsyncLoadingPrefabComplete iCallback, bool iIsInstance, bool iIsFromResources)
    {
        m_FilePath = iName;
        m_IsCompelete = false;
        m_IsInstance = iIsInstance;

        if (iCallback != null)
        {
            m_PrefabLoadingCB = iCallback;
        }

        if (iIsFromResources)
        {
            StartCoroutine(LoadPrefabAsync());
        }
        else
        {
        }
    }

    protected void LoadPrfab(string iName, AsyncLoadingPrefabComplete iCallback, bool iIsInstance, bool iIsFromResources)
    {
        m_FilePath = iName;
        m_IsCompelete = false;
        m_IsInstance = iIsInstance;

        if (iCallback != null)
        {
            m_PrefabLoadingCB = iCallback;
        }

        if (iIsFromResources)
        {
            GameObject _Obj = CoreTools.LoadResource<GameObject>(m_FilePath, "");

            if (_Obj != null)
            {
                GameObject _TempObj = _Obj;

                if (m_IsInstance)
                {
                    _TempObj = Instantiate(_Obj, Vector3.zero, Quaternion.identity) as GameObject;
                }

                if (m_PrefabLoadingCB != null)
                {
                    m_PrefabLoadingCB(_TempObj);
                }

                _TempObj = null;
            }
            else
            {
                if (m_PrefabLoadingCB != null)
                {
                    m_PrefabLoadingCB(_Obj);
                }
            }

            _Obj = null;
            Resources.UnloadUnusedAssets();
            m_IsCompelete = true;
        }
        else
        {
        }
    }

    /*
     * 載入Prefab並執行實體化和Callback
     */
    protected IEnumerator LoadPrefabAsync()
    {
        GameObject _Obj = CoreTools.LoadResource<GameObject>(m_FilePath, "");
        yield return null;

        if (_Obj != null)
        {
            GameObject _TempObj = _Obj;

            if (m_IsInstance)
            {
                _TempObj = Instantiate(_Obj, Vector3.zero, Quaternion.identity) as GameObject;
                yield return null;
            }

            if (m_PrefabLoadingCB != null)
            {
                m_PrefabLoadingCB(_TempObj);
            }

            _TempObj = null;
            yield return null;
        }
        else
        {
            if (m_PrefabLoadingCB != null)
            {
                m_PrefabLoadingCB(_Obj);
            }
        }

        _Obj = null;
        Resources.UnloadUnusedAssets();
        m_IsCompelete = true;
    }

    protected void PrefabLoaded(GameObject iGO)
    {
        if (iGO != null)
        {
            GameObject _TempObj = iGO;
            if (m_IsInstance)
            {
                _TempObj = Instantiate(iGO, Vector3.zero, Quaternion.identity) as GameObject;
            }

            if (m_PrefabLoadingCB != null)
            {
                m_PrefabLoadingCB(_TempObj);
            }

            _TempObj = null;
        }
        else
        {
            if (m_PrefabLoadingCB != null)
            {
                m_PrefabLoadingCB(iGO);
            }
        }

        m_IsCompelete = true;
    }
}
