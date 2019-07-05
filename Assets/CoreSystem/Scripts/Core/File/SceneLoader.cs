using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : FileLoaderBase
{
    public delegate void AsyncLoadingSceneComplete(Scene iScene);
    public event AsyncLoadingSceneComplete m_SceneLoadingCB;

    static public void LoadScene(string iSceneName, LoadSceneMode iMode = LoadSceneMode.Additive, AsyncLoadingSceneComplete iCallback = null, string iObjname = "Scene Loader")
    {
        GameObject _Loader = new GameObject(iObjname);
        SceneLoader _Script = _Loader.AddComponent<SceneLoader>();
        if (_Script != null)
            _Script.LoadScene(iSceneName, iMode, iCallback);
    }

    static public void LoadSceneAsync(string iSceneName, LoadSceneMode iMode = LoadSceneMode.Additive, AsyncLoadingSceneComplete iCallback = null, string iObjname = "Scene Loader")
    {
        GameObject _Loader = new GameObject(iObjname);
        SceneLoader _Script = _Loader.AddComponent<SceneLoader>();
        if (_Script != null)
            _Script.LoadSceneAsync(iSceneName, iMode, iCallback);
    }

    protected void LoadScene(string iSceneName, LoadSceneMode iMode, AsyncLoadingSceneComplete iCallback)
    {
        m_IsCompelete = false;

        if (iCallback != null)
            m_SceneLoadingCB = iCallback;

        SceneManager.LoadScene(iSceneName, iMode);

        if (m_SceneLoadingCB != null)
        {
            Scene _Scene = SceneManager.GetSceneByName(iSceneName);
            if (_Scene != null)
                m_SceneLoadingCB(_Scene);
        }

        m_IsCompelete = true;
    }

    protected void LoadSceneAsync(string iSceneName, LoadSceneMode iMode, AsyncLoadingSceneComplete iCallback)
    {
        m_IsCompelete = false;

        if (iCallback != null)
            m_SceneLoadingCB = iCallback;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(iSceneName, iMode);
    }

    void OnSceneLoaded(Scene iScene, LoadSceneMode iMode)
    {
        if (iScene == null)
            return;

        if (m_SceneLoadingCB != null)
        {
            SceneManager.SetActiveScene(iScene);
            m_SceneLoadingCB(iScene);
        }

        m_IsCompelete = true;
    }
}
