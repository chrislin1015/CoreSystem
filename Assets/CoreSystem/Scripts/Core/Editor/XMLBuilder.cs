using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class XMLBuilder : EditorWindow
{
    static protected XMLBuilder s_Instance;

    protected TextAsset m_StringSource;
    protected string m_SourcePath;
    protected string m_SourceContent;

    [MenuItem("CoreTool/XML Builder")]
    static void Init()
    {
        s_Instance = (XMLBuilder)EditorWindow.GetWindow(typeof(XMLBuilder));
    }

    void OnEnable()
    {
        RefreshSource();
    }

    void OnDestroy()
    {
    }

    void Update()
    {
        Repaint();
    }

    protected void RefreshSource()
    {
        if (m_StringSource)
        {
            m_SourcePath = AssetDatabase.GetAssetPath(m_StringSource.GetInstanceID());
            m_SourceContent = m_StringSource.text;
        }
        else
        {
            m_SourcePath = "";
            m_SourceContent = "";
        }
    }

    void OnGUI()
    {
        GUIViewByMultiSelect();
    }

    protected void GUIViewByMultiSelect()
    {
        List<TextAsset> _TextAssets = new List<TextAsset>();

        if (Selection.objects != null && Selection.objects.Length > 0)
        {
            Object[] _Objects = EditorUtility.CollectDependencies(Selection.objects);

            foreach (Object _Obj in _Objects)
            {
                if (_Obj is TextAsset == false)
                    continue;

                TextAsset _Text = _Obj as TextAsset;
                if (_Text == null)
                    continue;

                _TextAssets.Add(_Text);
            }
        }

        Color _OrgColor = GUI.contentColor;
        GUI.contentColor = Color.green;
        EditorGUILayout.LabelField("檔案清單");
        GUI.contentColor = _OrgColor;

        EditorTool.BeginContents();
        int _Count = 0;
        foreach (TextAsset _Text in _TextAssets)
        {
            _Count++;
            EditorGUILayout.LabelField(_Count.ToString() + "    " + _Text.name);
        }
        EditorTool.EndContents();
        EditorGUILayout.Separator();

        if (_TextAssets.Count == 0)
        {
            CenterButton("Create Scriptable Object", Color.gray);
        }
        else
        {
            if (CenterButton("Create Scriptable Object", Color.green))
            {
                ShowNotification(new GUIContent("Making string table..."));

                foreach (TextAsset _Text in _TextAssets)
                {
                    m_SourcePath = AssetDatabase.GetAssetPath(_Text.GetInstanceID());
                    m_SourceContent = _Text.text;

                    string _Path = GetFolderPath(m_SourcePath);

                    string _SOPath = _Path + _Text.name + ".asset";
                    ScriptableObject _SO2 = ScriptableObject.CreateInstance(_Text.name + "SO");
                    Object _Temp = AssetDatabase.LoadAssetAtPath(_SOPath, _SO2.GetType());
                    ScriptableObject _SO = _Temp as ScriptableObject;//AssetDatabase.LoadAssetAtPath(_SOPath, System.Type.GetType(_Text.name + "SO")) as ScriptableObject;

                    if (_SO == null)
                    {
                        _SO = ScriptableObject.CreateInstance(_Text.name + "SO");
                        System.Reflection.MethodInfo _M = _SO.GetType().GetMethod("DataFromXML");
                        _M.Invoke(_SO, new object[] { m_SourceContent });
                        AssetDatabase.CreateAsset(_SO, _SOPath);
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        System.Reflection.MethodInfo _M = _SO.GetType().GetMethod("DataFromXML");
                        _M.Invoke(_SO, new object[] { m_SourceContent });
                        AssetDatabase.Refresh();
                    }
                }
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

    protected string GetFolderPath(string iPath)
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
