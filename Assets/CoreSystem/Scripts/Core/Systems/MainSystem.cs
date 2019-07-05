﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : Singleton<MainSystem>
{
    /*
     * 截圖功能的解析度倍率
     */
    public int m_ScreenShotSuperSize = 1;

    /*
     * 是否將GameCore設定為不會自動摧毀
     */
    public bool m_IsDontDestroy = true;

    /*
     * 是否使用通用模組建立功能
     */
    public bool m_IsCommonModules = true;

    /*
     * 多久進行一次垃圾收集
     */
    public int m_GCFrequence = 300;

    /*
     * 第一個要進入的模組ID
     */
    public int m_FirstModuleID = 0;

    /*
     * 遊戲系統名稱，設定後會載入遊戲本身的系統管理物件
     */
    //public string m_GameSystemName = "";

    public int m_TimeZone = 8;
    public int m_TargetFPS = 60;
    public CoreEnum.VSYNC_COUNT e_SyncCount = CoreEnum.VSYNC_COUNT.EVERY_V_BLANK;

    void Awake()
    {
        CoreDefine.Initial(m_TimeZone);

        if (m_IsDontDestroy)
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }

        if (e_SyncCount == CoreEnum.VSYNC_COUNT.DONT_SYNC)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = m_TargetFPS;
        }
        else
        {
            QualitySettings.vSyncCount = (int)e_SyncCount;
            Application.targetFrameRate = -1;
        }

        StartCoroutine("GCRoutine");
    }

    new protected void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }

    void Start()
    {
        if (m_IsCommonModules)
        {
            if (Application.isPlaying)
            {
                CommonModule.CreateModules();
            }
        }

        GPUInfo();
    }

    IEnumerator GCRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_GCFrequence);
            System.GC.Collect();
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F4))
        {
            ScreenShot.CaptureScreen("ScreenShot",
                System.DateTime.Now.Year.ToString() +
                System.DateTime.Now.Month.ToString() +
                System.DateTime.Now.Day.ToString() +
                System.DateTime.Now.Hour.ToString() +
                System.DateTime.Now.Minute.ToString() +
                System.DateTime.Now.Second.ToString(),
                m_ScreenShotSuperSize);
        }

        if (m_IsCommonModules && Application.isPlaying && CommonModule.s_IsReady)
        {
            ModuleSystem.Instance.ReadyChangeModule(m_FirstModuleID, InitialFadeOutComplete);
            CommonModule.s_IsReady = false;
        }
    }

    void GPUInfo()
    {
        LogSystem.Instance.Log("GPU Device ID : " + SystemInfo.graphicsDeviceID.ToString());
        LogSystem.Instance.Log("GPU Device Name : " + SystemInfo.graphicsDeviceName);
        LogSystem.Instance.Log("GPU Device Type : " + SystemInfo.graphicsDeviceType.ToString());
        LogSystem.Instance.Log("GPU Device Vendor : " + SystemInfo.graphicsDeviceVendor);
        LogSystem.Instance.Log("GPU Device Vendor ID : " + SystemInfo.graphicsDeviceVendorID.ToString());
        LogSystem.Instance.Log("GPU Device Version : " + SystemInfo.graphicsDeviceVersion);
        LogSystem.Instance.Log("GPU Memory Size : " + SystemInfo.graphicsMemorySize.ToString());
        LogSystem.Instance.Log("GPU Multithread : " + SystemInfo.graphicsMultiThreaded.ToString());
        LogSystem.Instance.Log("GPU Shader Level : " + SystemInfo.graphicsShaderLevel.ToString());
        LogSystem.Instance.Log("GPU NPOT Support : " + SystemInfo.npotSupport.ToString());
    }

    void InitialFadeOutComplete()
    {
        ModuleSystem.Instance.ChangeModule();
    }
}