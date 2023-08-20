using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RDFadeOutUpdate : MonoBehaviour
{
    private int nowDuration = 0;

    private CanvasGroup mc;

    public float targetAlpha = 0;
    private float div;

    private Image bgBackImg;

    private Sprite initBgImg;

    private void Start() {
        mc = gameObject.GetComponent<CanvasGroup>();
        bgBackImg = transform.Find("FadeOutBack").GetComponent<Image>();
        initBgImg = bgBackImg.sprite;
    }
    private void FixedUpdate() {
        if (nowDuration <= 0)
        {
            if (mc.alpha == targetAlpha) return;
            if (targetAlpha != mc.alpha)
            {
                mc.alpha = targetAlpha;
                return;
            }
        }
        mc.alpha += div;
        nowDuration--;

    }
    public void SetOption(int duration, float targetAlpha) {
        nowDuration = duration;
        this.targetAlpha = targetAlpha;
        div = (targetAlpha - mc.alpha) / duration;
    }

    public void SetBgImage(Sprite bgSprite) {
        bgBackImg.sprite = bgSprite;
    }

    public void InitBgImage() {
        bgBackImg.sprite = initBgImg;
    }

}
