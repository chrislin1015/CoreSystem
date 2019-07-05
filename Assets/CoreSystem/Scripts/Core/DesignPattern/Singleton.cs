using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T s_Instance;

    /**
       Returns the instance of this singleton.
    */
    public static T Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = (T)FindObjectOfType(typeof(T));

                if (s_Instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
                }
            }

            return s_Instance;
        }
    }

    protected void OnDestroy()
    {
        s_Instance = null;
    }
}
