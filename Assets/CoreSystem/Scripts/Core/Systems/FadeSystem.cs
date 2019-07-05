using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FadeSystem : Singleton<FadeSystem>
{
    protected IFadeObjectBase m_FadeObject;
    protected Action m_FadeInCallback;
    protected Action m_FadeOutCallback;

    public CoreEnum.FADE_TYPE FadeType
    {
        get
        {
            if (m_FadeObject == null)
                return CoreEnum.FADE_TYPE.MAX;
            return m_FadeObject.e_FadeType;
        }
    }

    private void Awake()
    {
    }

    new protected void OnDestroy()
    {
        base.OnDestroy();

        m_FadeInCallback = null;
        m_FadeOutCallback = null;
    }

    public void FadeIn(Action iCallback)
    {
        if (m_FadeObject == null)
            return;

        m_FadeObject.FadeIn(iCallback);
    }

    public void FadeOut(Action iCallback)
    {
        if (m_FadeObject == null)
            return;

        m_FadeObject.FadeOut(iCallback);
    }

    public void SetFadeObject(IFadeObjectBase iFadeObject)
    {
        if (iFadeObject == null)
            return;

        m_FadeObject = iFadeObject;
    }
}
