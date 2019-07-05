using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class IFadeObjectBase : MonoBehaviour
{
    public CoreEnum.FADE_TYPE e_FadeType;
    public abstract void FadeOut(Action iCallback);
    public abstract void FadeIn(Action iCallback);
}
