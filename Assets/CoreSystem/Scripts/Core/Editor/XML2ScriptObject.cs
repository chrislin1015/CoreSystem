using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class XML2ScriptObject : EditorWindow
{
    static protected XML2ScriptObject s_Instance;

    protected TextAsset m_StringSource;
    protected string m_SourcePath;
    protected string m_SourceContent;

    [MenuItem("CoreTool/XML To ScriptObject")]
    static void Init()
    {
        s_Instance = (XML2ScriptObject)EditorWindow.GetWindow(typeof(XML2ScriptObject));
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
            EditorTool.CenterButton("Create Scriptable Object", Color.gray);
        }
        else
        {
            if (EditorTool.CenterButton("Create Scriptable Object", Color.green))
            {
                ShowNotification(new GUIContent("Making string table..."));

                foreach (TextAsset _Text in _TextAssets)
                {
                    m_SourcePath = AssetDatabase.GetAssetPath(_Text.GetInstanceID());
                    m_SourceContent = _Text.text;

                    string _Path = FileTool.GetFolderPath(m_SourcePath);

                    string _SOPath = _Path + _Text.name + ".asset";
                    ScriptableObject _SO = ScriptableObject.CreateInstance(_Text.name + "SO");
                    if (_SO != null)
                    {
                        Object _Temp = AssetDatabase.LoadAssetAtPath(_SOPath, _SO.GetType());

                        if (_Temp == null)
                        {
                            System.Reflection.MethodInfo _M = _SO.GetType().GetMethod("DataFromXML");
                            _M.Invoke(_SO, new object[] { m_SourceContent });
                            AssetDatabase.CreateAsset(_SO, _SOPath);
                            AssetDatabase.Refresh();
                        }
                        else
                        {
                            _SO = _Temp as ScriptableObject;
                            System.Reflection.MethodInfo _M = _SO.GetType().GetMethod("DataFromXML");
                            _M.Invoke(_SO, new object[] { m_SourceContent });
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }
        }
    }
}
