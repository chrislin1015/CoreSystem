using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoreDefine
{
    public static TimeSpan s_TimeZoneSpan = new TimeSpan();

    public const string CORE_VERSION = "0.0.1";
    /// <summary>
    /// 定義一秒有多少豪秒
    /// </summary>
    public const int MILLISECONDS = 1000;

    /// <summary>
    /// 螢幕解低度寬除以高的比值
    /// </summary>
    static public float SCREENASPECT = 16.0f / 9.0f;

    /// <summary>
    /// 螢幕解析度百分之5的大小
    /// </summary>
    static public Vector2 SCREEN5PERCENT;

    /// <summary>
    /// 螢幕解析度百分之10的大小
    /// </summary>
    static public Vector2 SCREEN10PERCENT;

    /// <summary>
    /// 螢幕解析度百分之25的大小
    /// </summary>
    static public Vector2 SCREEN25PERCENT;

    /// <summary>
    /// 螢幕解析度百分之50的大小
    /// </summary>
    static public Vector2 SCREEN50PERCENT;

    /// <summary>
    /// 螢幕解析度百分之75的大小
    /// </summary>
    static public Vector2 SCREEN75PERCENT;
    static public Rect SCREENRECT5PERCENT;
    static public Rect SCREENRECT10PERCENT;
    static public Rect SCREENRECT50PERCENT;

    static public Vector2 FOUR_K = new Vector2(3840.0f, 2160.0f);
    static public Vector2 FULLHD = new Vector2(1920.0f, 1080.0f);
    static public Vector2 P720 = new Vector2(1280.0f, 720.0f);
    static public Vector2 P480 = new Vector2(640.0f, 480.0f);

    static public int BYTE_2048 = 2048;
    static public int BYTE_1024 = 1024;
    static public int BYTE_512 = 512;
    static public int BYTE_256 = 256;
    static public int BYTE_128 = 128;
    static public int BYTE_64 = 64;
    static public int BYTE_32 = 32;
    static public int BYTE_16 = 16;
    static public int BYTE_8 = 8;

    static public float ANGLE_000 = 0.0f;
    static public float ANGEL_030 = 30.0f;
    static public float ANGEL_045 = 45.0f;
    static public float ANGEL_060 = 60.0f;
    static public float ANGEL_075 = 75.0f;
    static public float ANGEL_090 = 90.0f;
    static public float ANGEL_120 = 120.0f;
    static public float ANGEL_135 = 135.0f;
    static public float ANGEL_150 = 150.0f;
    static public float ANGEL_180 = 180.0f;
    static public float ANGEL_225 = 225.0f;
    static public float ANGEL_270 = 270.0f;
    static public float ANGEL_315 = 315.0f;
    static public float ANGEL_360 = 360.0f;

    static public Color WHITE_ALPHA = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    static public Color GRAY_ALPHA = new Color(0.5f, 0.5f, 0.5f, 0.0f);

    static public void Initial(int iTimeZone)
    {
        s_TimeZoneSpan = new TimeSpan(iTimeZone, 0, 0);
        SCREENASPECT = (float)Screen.width / (float)Screen.height;
        SCREEN5PERCENT = new Vector2(Screen.width * 0.05f, Screen.height * 0.05f);
        SCREEN10PERCENT = new Vector2(Screen.width * 0.1f, Screen.height * 0.1f);
        SCREEN25PERCENT = new Vector2(Screen.width * 0.25f, Screen.height * 0.25f);
        SCREEN50PERCENT = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        SCREEN75PERCENT = new Vector2(Screen.width * 0.75f, Screen.height * 0.75f);

        SCREENRECT5PERCENT = new Rect(SCREEN5PERCENT.x, SCREEN5PERCENT.y, Screen.width - SCREEN5PERCENT.x, Screen.height - SCREEN5PERCENT.y);
        SCREENRECT10PERCENT = new Rect(SCREEN10PERCENT.x, SCREEN10PERCENT.y, Screen.width - SCREEN10PERCENT.x, Screen.height - SCREEN10PERCENT.y);
        SCREENRECT50PERCENT = new Rect(SCREEN50PERCENT.x, SCREEN50PERCENT.y, Screen.width - SCREEN50PERCENT.x, Screen.height - SCREEN50PERCENT.y);
    }
}
