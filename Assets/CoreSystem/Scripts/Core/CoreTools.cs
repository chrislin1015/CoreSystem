using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Reflection;
using System;
using System.Net.NetworkInformation;
using System.IO;
using System.Collections.Generic;

public static class CoreTools
{
    static TimeSpan s_24Hour = new TimeSpan(24, 0, 0);
    static TimeSpan s_1Hour = new TimeSpan(1, 0, 0);

    public static TimeSpan Get24Hour()
    {
        return s_24Hour;
    }

    public static TimeSpan Get1Hour()
    {
        return s_1Hour;
    }

    static public void RecursiveDownSetLayer(Transform iT, int iLayer)
    {
        if (iT == null)
            return;

        foreach (Transform _Child in iT)
        {
            RecursiveDownSetLayer(_Child, iLayer);
        }

        iT.gameObject.layer = iLayer;
    }

    static public void RecursiveDownSetRenderQueue(Transform iT, int iQueue)
    {
        if (iT == null)
            return;

        foreach (Transform _Child in iT)
        {
            RecursiveDownSetRenderQueue(_Child, iQueue);
        }

        Renderer _Renderer = iT.GetComponent<Renderer>();
        if (_Renderer == null || _Renderer.material == null)
            return;

        _Renderer.material.renderQueue = iQueue;
    }

    static public Vector3 LocalToWorld(Transform iObj, Transform iRoot)
    {
        if (iObj == null)
            return Vector3.zero;

        if (iRoot == null)
            return iObj.localPosition;

        Vector3 _tempposition = iRoot.worldToLocalMatrix * iObj.position;
        return _tempposition;
    }

    /*
     * 正負1轉換成0~1的值
     */
    static public float PlusMinusOneToZeroOne(float iValue)
    {
        return (iValue + 1.0f) * 0.5f;
    }

    /*
     * 0~1轉換成正負1
     */
    static public float ZeroOneToPlusMinusOne(float iValue)
    {
        return (iValue - 0.5f) * 2.0f;
    }

    /*
     * p0 --> start point. p1 --> control point 1. p2 --> control point 2. p3 --> end point
     */
    static public Vector3 BezierCurve(Vector3 iPoint0, Vector3 iPoint1, Vector3 iPoint2, Vector3 iPoint3, float iT)
    {
        float _U = 1.0f - iT;
        float _TT = iT * iT;
        float _UU = _U * _U;
        float _UUU = _UU * _U;
        float _TTT = _TT * iT;

        Vector3 _Final = _UUU * iPoint0; //first term
        _Final += 3 * _UU * iT * iPoint1; //second term
        _Final += 3 * _U * _TT * iPoint2; //third term
        _Final += _TTT * iPoint3; //fourth term
        return _Final;
    }

    /*
     * 加到指定的父節點下並且本地座標設為單位矩陣
     */
    static public void AddChild(Transform iParent, Transform iChild)
    {
        iChild.SetParent(iParent);
        iChild.localPosition = Vector3.zero;
        iChild.localScale = Vector3.one;
        iChild.localRotation = Quaternion.identity;
    }

    static public Transform GetChildByEquals(Transform iT, string iName)
    {
        if (iT == null)
            return null;

        if (iT.name.Equals(iName))
            return iT;

        foreach (Transform _Child in iT)
        {
            Transform _Temp = GetChildByEquals(_Child, iName);
            if (_Temp != null)
                return _Temp;
        }

        return null;
    }

    static public void GetChildsByEquals(Transform iT, string iName, List<Transform> iList)
    {
        if (iT == null || iList == null)
            return;

        if (iT.name.Equals(iName))
        {
            iList.Add(iT);
        }

        foreach (Transform _Child in iT)
        {
            GetChildsByEquals(_Child, iName, iList);
        }
    }

    static public Transform GetChildByContains(Transform iT, string iName)
    {
        if (iT == null)
            return null;

        if (iT.name.Contains(iName))
            return iT;

        foreach (Transform _child in iT)
        {
            Transform _temp = GetChildByContains(_child, iName);
            if (_temp != null)
                return _temp;
        }

        return null;
    }

    static public void GetChildsByContains(Transform iT, string iName, List<Transform> iList)
    {
        if (iT == null || iList == null)
            return;

        if (iT.name.Contains(iName))
        {
            iList.Add(iT);
        }

        foreach (Transform _Child in iT)
        {
            GetChildsByContains(_Child, iName, iList);
        }
    }

    static public Transform GetParentByContains(Transform iT, string iName)
    {
        if (iT == null)
            return null;

        if (iT.name.Contains(iName))
            return iT;

        return GetParentByContains(iT.parent, iName);
    }

    static public Transform GetParentByEquals(Transform iT, string iName)
    {
        if (iT == null)
            return null;

        if (iT.name.Equals(iName))
            return iT;

        return GetParentByEquals(iT.parent, iName);
    }

    //change time from float to string
    static public string TimeFtoS(float iTime)
    {
        int _S = (int)iTime / 1000;
        int _M = _S / 60;
        int _MS = (int)((iTime - ((float)_S * 1000))) / 10;
        _S %= 60;

        string _Mstring = _M.ToString();
        string _Sstring = _S.ToString();
        string _MSstring = _MS.ToString();

        if (_M < 10)
        {
            _Mstring = "0" + _Mstring;
        }

        if (_S < 10)
        {
            _Sstring = "0" + _Sstring;
        }

        if (_MS < 10)
        {
            _MSstring = "0" + _MSstring;
        }

        return _Mstring + ":" + _Sstring + ":" + _MSstring;
    }

    static public DateTime PHPTimeToCSharpTime(long iPHPTime, int iTimeZone)
    {
        DateTime _1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        TimeSpan _Span = new TimeSpan(iPHPTime * 10000000);
        DateTime _Data = new DateTime(_Span.Ticks + _1970.Ticks + CoreDefine.s_TimeZoneSpan.Ticks);
        return _Data;
    }

    static public float ClampAngle(float angle, float min = -360.0f, float max = 360.0f)
    {
        angle = angle < CoreDefine.ANGEL_360 ? angle += CoreDefine.ANGEL_360 : angle;
        angle = angle > CoreDefine.ANGEL_360 ? angle -= CoreDefine.ANGEL_360 : angle;
        angle = Mathf.Clamp(angle, min, max);

        return angle;
    }

    static public void SetAllChildActive(GameObject target, bool active)
    {
        int _count = target.transform.childCount;
        for (int i = 0; i < _count; i++)
        {
            GameObject _child = target.transform.GetChild(i).gameObject;
            SetAllChildActive(_child, active);
        }

        target.SetActive(active);
    }

    static public System.DateTime ConvertJavaMiliSecondToDateTime(long javams)
    {
        System.DateTime _utcbasetime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        System.DateTime _dt = _utcbasetime.Add(new System.TimeSpan(javams * System.TimeSpan.TicksPerMillisecond)).ToLocalTime();
        return _dt;
    }

#if ASSETBUNDLE
    static public T LoadPrefab<T>(string name, string folder) where T : class
    {
    if (string.IsNullOrEmpty(name))
    {
    Debug.Log("Don't have any file name");
    return default(T);
    }

    T _resource = CHILDHOOD.AssetLoader.Instance.LoadAsset<T>(name) as T;
    if (_resource == null)
    {
    string _path = string.IsNullOrEmpty(folder) ? name : folder + "/" + name;
    _resource = Resources.Load(_path, typeof(T)) as T;
    if (_resource == null)
    {
    Debug.Log("Can not load resource : " + path);
    }
    }
    return _resource;
    }
#else
    static public T LoadResource<T>(string iName, string iFolder, bool iIsFullPath = false) where T : class
    {
        if (string.IsNullOrEmpty(iName))
        {
            return default(T);
        }

        string _filepath = string.IsNullOrEmpty(iFolder) ? iName : iFolder + "/" + iName;

        T _Resource = default(T);
        if (iIsFullPath)
        {
#if UNITY_EDITOR
            //_Resource = AssetDatabase.LoadAssetAtPath(_filepath, typeof(T)) as T;
#endif
        }
        else
        {
            _Resource = Resources.Load(_filepath, typeof(T)) as T;
        }

        if (_Resource == null)
        {
            Debug.Log("Can not load prefab data : " + _filepath);
            return null;
        }

        return _Resource;
    }
#endif

    #region MATH
    /*
     * 階層公式
     */
    static public int MathFactorial(int iN)
    {
        int _Fact = 1;
        for (int i = 0; i < iN; ++i)
        {
            _Fact = _Fact * i;
        }

        return _Fact;
    }

    /*
     * 組合公式
     *   n           n!
     * C    = ---------------
     *   m       m!(n-m)!
     * @parameter 1. iN = 組合總數, 2. iM = 取出幾個
     */
    static public int MathCombinationFunction(int iN, int iM)
    {
        int _N = MathFactorial(iN);
        int _M = MathFactorial(iM) * MathFactorial(iN - iM);

        int _Result = _N / _M;

        return _Result;
    }
    #endregion

    #region NGUI
    static public Vector3 NGUIToScreenSpace(Vector3 iNGUI, float iNGUIWidth, float iNGUIHeight)
    {
        float _W = iNGUI.x / (iNGUIWidth * 0.5f);
        float _H = iNGUI.y / (iNGUIHeight * 0.5f);
        _W = PlusMinusOneToZeroOne(_W);
        _H = PlusMinusOneToZeroOne(_H);

        Vector3 _Screen = new Vector3(Screen.width * iNGUIWidth, Screen.height * iNGUIHeight, 0.0f);
        return _Screen;
    }

    static public Vector3 MousePointToNGUI(Vector3 iMousePoint, float iNGUIW, float iNGUIH)
    {
        float _X = iMousePoint.x / Screen.width;
        float _Y = iMousePoint.y / Screen.height;

        _X = ZeroOneToPlusMinusOne(_X);
        _Y = ZeroOneToPlusMinusOne(_Y);

        Vector3 _NGUI = new Vector3(_X * iNGUIW / 2, _Y * iNGUIH / 2, 0.0f);
        return _NGUI;
    }
    #endregion //NGUI

    //判斷字串是否為數字
    public static bool IsDigits(string iText)
    {
        if (string.IsNullOrEmpty(iText))
        {
            return false;
        }

        foreach (char c in iText)
        {
            if (c < '0' || c > '9')
            {
                return false;
            }
        }

        return true;
    }

    #region Enum Extensions
    /*
     * 該列舉變數理是否有一個列舉常數
     * @param 是要被檢驗的列舉變數
     * @param 要查證的列舉常數
     * @return 如果有該指定的列舉常數則回傳true
     */
    public static bool Has<T>(System.Enum iType, T iValue)
    {
        try
        {
            int _Type = (int)(object)iType;
            int _Value = (int)(object)iValue;
            return ((_Type & _Value) == _Value);
        }
        catch
        {
            return false;
        }
    }

    /*
     * 該列舉變數理是否為列舉常數
     * @param 是要被檢驗的列舉變數
     * @param 要查證的列舉常數
     * @return 如果是指定的列舉常數則回傳true
     */
    public static bool Is<T>(System.Enum iType, T iValue)
    {
        try
        {
            int _Type = (int)(object)iType;
            int _Value = (int)(object)iValue;
            return _Type == _Value;
        }
        catch
        {
            return false;
        }
    }

    /*
     * 從列舉變數中新增某一個列舉常數
     * @param 是要被新增的列舉變數
     * @param 指定的列舉常數
     * @return 回傳被修改過後的列舉變數
     */
    public static T Add<T>(System.Enum iType, T iValue)
    {
        try
        {
            return (T)(object)(((int)(object)iType | (int)(object)iValue));
        }
        catch (System.Exception ex)
        {
            throw new System.ArgumentException(
                string.Format(
                    "Could not append value from enumerated type '{0}'.",
                    typeof(T).Name
                ), ex);
        }
    }

    /*
     * 從列舉變數中移除某一個列舉常數
     * @param 是要被移除的列舉變數
     * @param 指定的列舉常數
     * @return 回傳被修改過後的列舉變數
     */
    public static T Remove<T>(System.Enum iType, T iValue)
    {
        try
        {
            return (T)(object)(((int)(object)iType & ~(int)(object)iValue));
        }
        catch (System.Exception ex)
        {
            throw new System.ArgumentException(
                string.Format(
                    "Could not remove value from enumerated type '{0}'.",
                    typeof(T).Name
                ), ex);
        }
    }

    /*
     * 字串轉enum
     * @param 要轉換的字串
     * @return 回傳字串對應的enum值
     * @remark T為指定的enum型別
     */
    static public T StringToEnum<T>(string iStr)
    {
        T _Enum = (T)System.Enum.Parse(typeof(T), iStr);

        return _Enum;
    }
    #endregion Enum Extensions

    /*
     * 將數字轉為三個位數加上一個,的字串格式
     */
    static public string NumberToDotString(long iNumber, int iSpliteCount = 3, string iSpliteChar = ",")
    {
        string _String = iNumber.ToString();
        int _DotCount = _String.Length / iSpliteCount;
        string _TempString = _String;
        for (int i = 0; i < _DotCount; ++i)
        {
            int _Offset = (i * iSpliteCount) + (i * 1) + iSpliteCount;
            if (_Offset >= _TempString.Length)
                break;
            _TempString = _TempString.Insert(_TempString.Length - _Offset, iSpliteChar);
        }

        return _TempString;
    }

    static public string StringToDotString(string iString, int iSpliteCount = 3, string iSpliteChar = ",")
    {
        int _DotCount = iString.Length / iSpliteCount;
        string _TempString = iString;
        for (int i = 0; i < _DotCount; ++i)
        {
            int _Offset = (i * iSpliteCount) + (i * iSpliteChar.Length) + iSpliteCount;
            if (_Offset >= _TempString.Length)
                break;
            _TempString = _TempString.Insert(_TempString.Length - _Offset, iSpliteChar);
        }
        return _TempString;
    }

    static public bool IsCantainChinese(string iText)
    {
        if (string.IsNullOrEmpty(iText))
            return false;

        int _Code = 0;
        int _CHStart = Convert.ToInt32("4e00", 16);
        int _CHEnd = Convert.ToInt32("9fff", 16);

        for (int i = 0; i < iText.Length; ++i)
        {
            _Code = Char.ConvertToUtf32(iText, i);
            if (_Code >= _CHStart && _Code <= _CHEnd)
            {
                return true;
            }
        }

        return false;
    }

    /**
      * Returns safe text from TextAsset.
      * 
      * Text files can contain byte order mark (BOM) to specify encoding details.
      * Generally, BOM is consumed when loading text from a file (for example with TextReader or XmlReader).
      * TextAsset provides "text" field that contains "raw" file text where BOM is preserved.
      * This can cause errors. 
      * For example, when trying to read xml with XmlReader.
      *        (XmlException: Text node cannot appear in this state.  Line 1, position 1.
      *         Mono.Xml2.XmlTextReader.ReadText (Boolean notWhitespace)... )
      * 
      * */
    public static string GetTextWithoutBOM(TextAsset iTextAsset)
    {
        MemoryStream _MemoryStream = new MemoryStream(iTextAsset.bytes);
        StreamReader _StreamReader = new StreamReader(_MemoryStream, true);

        string _Result = _StreamReader.ReadToEnd();

        _StreamReader.Close();
        _MemoryStream.Close();

        return _Result;
    }

    public static string GetTextWithoutBOM(string iText)
    {
        byte[] _TextBytes = System.Text.Encoding.UTF8.GetBytes(iText);
        MemoryStream _MemoryStream = new MemoryStream(_TextBytes);
        StreamReader _StreamReader = new StreamReader(_MemoryStream, true);

        string _Result = _StreamReader.ReadToEnd();

        _StreamReader.Close();
        _MemoryStream.Close();

        return _Result;
    }

    static public string GetMACAddress()
    {
        NetworkInterface[] NIList = NetworkInterface.GetAllNetworkInterfaces();
        for (int i = 0; i < NIList.Length; ++i)
        {
            PhysicalAddress _PA = NIList[i].GetPhysicalAddress();
            string _MAC = _PA.ToString();
            if (string.IsNullOrEmpty(_MAC) == false)
            {
                return _MAC;
            }
        }

        return null;
    }

    static public long GetNowTimeTick()
    {
        long _Now = System.DateTime.UtcNow.Ticks + CoreDefine.s_TimeZoneSpan.Ticks;
        return _Now;
    }

    static public bool IsTimeOK(long iFrom, long iTo, int iTimeZone)
    {
        System.DateTime _From = CoreTools.PHPTimeToCSharpTime(iFrom, iTimeZone);
        System.DateTime _To = CoreTools.PHPTimeToCSharpTime(iTo, iTimeZone);

        long _Now = GetNowTimeTick();
        if (_Now >= _From.Ticks && _Now <= _To.Ticks)
        {
            return true;
        }

        return false;
    }

    static public long ActiveNextCheckTime()
    {
        long _Now = GetNowTimeTick();
        TimeSpan _OneHour = new TimeSpan(0, 30, 0);
        DateTime _NextTime = new DateTime(_Now + _OneHour.Ticks);
        _NextTime = new DateTime(_NextTime.Year, _NextTime.Month, _NextTime.Day, _NextTime.Hour, _NextTime.Minute, 0);

        return _NextTime.Ticks;
    }

    static public long NextCheckTime(int iH, int iM, int iS)
    {
        long _Now = GetNowTimeTick();
        TimeSpan _NextSpan = new TimeSpan(iH, iM, iS);
        DateTime _NextTime = new DateTime(_Now + _NextSpan.Ticks);
        _NextTime = new DateTime(_NextTime.Year, _NextTime.Month, _NextTime.Day, _NextTime.Hour, _NextTime.Minute, 0);

        return _NextTime.Ticks;
    }

    static public long GetNextDay()
    {
        TimeSpan _NextTime = Get24Hour();
        //先計算基本時間的下一天
        DateTime _NextDay = new DateTime(_NextTime.Ticks + DateTime.UtcNow.Ticks);
        //鎖定在下一天的0點
        _NextDay = new DateTime(_NextDay.Year, _NextDay.Month, _NextDay.Day, 0, 0, 0, DateTimeKind.Utc);
        //加上時區
        return _NextDay.Ticks + CoreDefine.s_TimeZoneSpan.Ticks;
    }

    static public long GetNextHour()
    {
        TimeSpan _NextTime = Get1Hour();
        //先計算基本時間的下一個準點
        DateTime _NextDay = new DateTime(_NextTime.Ticks + DateTime.UtcNow.Ticks);
        //鎖定在準點的0分0秒
        _NextDay = new DateTime(_NextDay.Year, _NextDay.Month, _NextDay.Day, _NextDay.Hour, 0, 0, DateTimeKind.Utc);
        //加上時區
        return _NextDay.Ticks + CoreDefine.s_TimeZoneSpan.Ticks;
    }

    static public float HorizontalDistance(Vector3 iPos, Vector3 iTargetPos, float iOffset1 = 0f, float iOffset2 = 0f)
    {
        iPos.y = 0f;
        iTargetPos.y = 0f;

        float _X = iPos.x - iTargetPos.x;
        float _Y = iPos.y - iTargetPos.y;
        float _Z = iPos.z - iTargetPos.z;
        double _Dis = _X * _X + _Y * _Y + _Z * _Z - iOffset1 * iOffset1 - iOffset2 * iOffset2;
        _Dis = Math.Round(_Dis, 2);

        return (_Dis < 0f) ? 0f : (float)_Dis;
    }
}
