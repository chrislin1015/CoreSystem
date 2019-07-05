using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileLoaderBase : MonoBehaviour
{

    /*
     * 檔案名稱
     */
    [HideInInspector]
    public string m_FilePath;

    /*
     * 是否載入完成
     */
    protected bool m_IsCompelete = false;

    void Update()
    {
        if (m_IsCompelete)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
