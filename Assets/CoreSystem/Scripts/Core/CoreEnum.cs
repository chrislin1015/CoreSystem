using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreEnum
{
    /*
     * * 定義平台代號，與伺服器溝通時使用
     */
    public enum DEVICE_TYPE
    {
        EDITOR,
        WINDOWS,
        IOS,
        ANDROID,
        WINPHONE,
        WEB,
        VIVE,
        OCULUS,
        MSMR,
        MAX
    }

    /*
     * 淡入淡出系統的狀態
     */
    public enum FADE_TYPE
    {
        FADE_NONE,
        FADE_IN,
        FADE_IN_FINISH,
        FADE_OUT,
        FADE_OUT_FINISH,
        MAX
    }

    /*
     * 網路狀態
     */
    public enum NETWORK_TYPE
    {
        NONE,
        THREE_G,
        WIFI,
        MAX
    }

    /*
     * 語言分類
     */
    public enum LANGUAGE_TYPE
    {
        ZH_TW,
        ZH_CN,
        EN_US,
        JA,
        MAX
    }

    public enum ISO_COUNTRY_CODE
    {
        TWN,
        CHN,
        JPN,
        USA,
        MAX
    }

    public enum XMLKIND
    {
        TEMPORARY,
        PERSISTENT,
        RESOURCE,
    }

    public enum VSYNC_COUNT
    {
        DONT_SYNC,
        EVERY_V_BLANK,
        EVERY_2_V_BLANK,
        MAX,
    }
}
