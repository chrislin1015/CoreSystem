using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoneNameChange : EditorWindow
{
    static protected BoneNameChange s_Instance;

    protected string m_BeChangeWord = "";
    protected string m_WantChangeWord = "";
    protected TextAsset m_StringSource;
    protected string m_SourcePath;
    protected string m_SourceContent;

    [MenuItem("CoreTool/Bone Name Change")]
    static void Init()
    {
        s_Instance = (BoneNameChange)EditorWindow.GetWindow(typeof(BoneNameChange));
    }

    void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        Transform _SelectObject = null;

        if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
        {
            foreach (GameObject _Obj in Selection.gameObjects)
            {
                Transform _Transform = _Obj.transform;
                if (_Transform == null)
                    continue;

                _SelectObject = _Transform;
                break;
            }
        }

        Color _OriginalColor = GUI.contentColor;
        GUI.contentColor = Color.green;
        EditorGUILayout.LabelField("要修改的根結點");
        GUI.contentColor = _OriginalColor;

        EditorTool.BeginContents();
        if (_SelectObject == null)
        {
            EditorGUILayout.LabelField("沒選擇物件");
        }
        else
        {
            EditorGUILayout.LabelField(_SelectObject.name);
        }
        EditorTool.EndContents();
        EditorGUILayout.Separator();

        _OriginalColor = GUI.contentColor;
        GUI.contentColor = Color.red;
        EditorGUILayout.LabelField("要變更的關鍵字");
        GUI.contentColor = _OriginalColor;

        EditorTool.BeginContents();
        m_BeChangeWord = EditorGUILayout.TextField(m_BeChangeWord);
        EditorTool.EndContents();
        EditorGUILayout.Separator();

        _OriginalColor = GUI.contentColor;
        GUI.contentColor = Color.yellow;
        EditorGUILayout.LabelField("變更為....");
        GUI.contentColor = _OriginalColor;

        EditorTool.BeginContents();
        m_WantChangeWord = EditorGUILayout.TextField(m_WantChangeWord);
        EditorTool.EndContents();
        EditorGUILayout.Separator();

        if (_SelectObject == null)
        {
            CenterButton("開始轉換", Color.gray);
        }
        else
        {
            if (CenterButton("開始轉換", Color.green))
            {
                RecursiveChangeName(_SelectObject, m_BeChangeWord, m_WantChangeWord);
            }
        }
    }

    protected bool CenterButton(string iName, Color iColor)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        Color _OrgColor = GUI.backgroundColor;
        GUI.backgroundColor = iColor;
        bool _IsPress = GUILayout.Button(iName);
        GUI.backgroundColor = _OrgColor;

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        return _IsPress;
    }

    protected void RecursiveChangeName(Transform iT, string iBeChangeWord, string iWantChangeWord)
    {
        if (iT == null)
            return;

        if (iT.name.Contains(iBeChangeWord))
        {
            string _NewName = iT.name.Replace(iBeChangeWord, iWantChangeWord);
            iT.name = _NewName;
        }

        foreach (Transform iChild in iT)
        {
            RecursiveChangeName(iChild, iBeChangeWord, iWantChangeWord);
        }
    }
}
