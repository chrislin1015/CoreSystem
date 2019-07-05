using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeUIObject : IFadeObjectBase
{
    [SerializeField]
    protected float m_FadeTime;
    protected CanvasGroup m_CanvasGroup;
    protected Action m_FadeInCallBack;
    protected Action m_FadeOutCallBack;
    protected Image m_FadeImage;

    void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_FadeImage = GetComponent<Image>();
    }

    void Start()
    {
        FadeSystem.Instance.SetFadeObject(this);
    }

    void OnDestroy()
    {
        m_CanvasGroup = null;
        m_FadeInCallBack = null;
        m_FadeOutCallBack = null;
    }

    public override void FadeIn(Action iCallback)
    {
        if (e_FadeType != CoreEnum.FADE_TYPE.FADE_NONE)
            return;

        e_FadeType = CoreEnum.FADE_TYPE.FADE_IN;
        DOTween.To(() => m_CanvasGroup.alpha, x => m_CanvasGroup.alpha = x, 0.0f, m_FadeTime).SetEase(Ease.OutSine).OnComplete(FadeInComplete);
        m_FadeInCallBack = iCallback;
    }

    public void FadeInComplete()
    {
        e_FadeType = CoreEnum.FADE_TYPE.FADE_NONE;

        if (m_FadeInCallBack != null)
            m_FadeInCallBack();

        m_CanvasGroup.interactable = false;
        m_FadeImage.raycastTarget = false;
    }

    public override void FadeOut(Action iCallback)
    {
        if (e_FadeType != CoreEnum.FADE_TYPE.FADE_NONE)
            return;

        e_FadeType = CoreEnum.FADE_TYPE.FADE_OUT;
        DOTween.To(() => m_CanvasGroup.alpha, x => m_CanvasGroup.alpha = x, 1.0f, m_FadeTime).SetEase(Ease.OutSine).OnComplete(FadeOutComplete);
        m_FadeOutCallBack = iCallback;
        m_CanvasGroup.interactable = true;
        m_FadeImage.raycastTarget = true;
    }

    public void FadeOutComplete()
    {
        e_FadeType = CoreEnum.FADE_TYPE.FADE_NONE;

        if (m_FadeOutCallBack != null)
            m_FadeOutCallBack();
    }
}
