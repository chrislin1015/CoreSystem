using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

static public class EditorTool
{
    static bool s_EndHorizontal = false;

    static Texture2D s_WhiteTexture;

    public static Texture2D WhiteTexture
    {
        get
        {
            if (s_WhiteTexture != null)
                return s_WhiteTexture;

            s_WhiteTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            s_WhiteTexture.SetPixel(0, 0, new Color(1f, 1f, 1f, 1f));
            s_WhiteTexture.Apply();

            return s_WhiteTexture;
        }
    }

    static public void BeginContents()
    {
        s_EndHorizontal = false;
        EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10.0f));
        GUILayout.Space(10.0f);

        GUILayout.BeginVertical();
        GUILayout.Space(2.0f);
    }

    static public void EndContents()
    {
        GUILayout.Space(3.0f);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (s_EndHorizontal)
        {
            GUILayout.Space(3.0f);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(3.0f);
    }

    static public bool DrawHeader(string text) { return DrawHeader(text, text, false, true); }

    static public bool DrawHeader(string text, string key) { return DrawHeader(text, key, false, true); }

    static public bool DrawHeader(string text, bool detailed) { return DrawHeader(text, text, detailed, !detailed); }

    static public bool DrawHeader(string text, string key, bool forceOn, bool minimalistic)
    {
        bool state = EditorPrefs.GetBool(key, true);

        if (!minimalistic) GUILayout.Space(3f);
        if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal();
        GUI.changed = false;

        if (minimalistic)
        {
            if (state) text = "\u25BC" + (char)0x200a + text;
            else text = "\u25BA" + (char)0x200a + text;

            GUILayout.BeginHorizontal();
            GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
            if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
        else
        {
            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
        }

        if (GUI.changed) EditorPrefs.SetBool(key, state);

        if (!minimalistic) GUILayout.Space(2f);
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;
        if (!forceOn && !state) GUILayout.Space(3f);
        return state;
    }

    static public void DrawSeparator()
    {
        GUILayout.Space(12f);

        if (Event.current.type == EventType.Repaint)
        {
            Texture2D tex = WhiteTexture;
            Rect rect = GUILayoutUtility.GetLastRect();
            GUI.color = new Color(0f, 0f, 0f, 0.25f);
            GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
            GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
            GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
            GUI.color = Color.white;
        }
    }

    static public bool CenterButton(string iName, Color iColor)
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
}
