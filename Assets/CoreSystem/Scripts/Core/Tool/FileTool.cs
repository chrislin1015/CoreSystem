using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileTool
{
    static public string GetFolderPath(string iPath)
    {
        int _DotIndex = iPath.LastIndexOf('.');
        int _SlashIndex = Mathf.Max(iPath.LastIndexOf('/'), iPath.LastIndexOf('\\'));
        if (_SlashIndex > 0)
        {
            return (_DotIndex > _SlashIndex) ? iPath.Substring(0, _SlashIndex + 1) : iPath + "/";
        }
        return "Assets/";
    }
}
