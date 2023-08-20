using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPGMaker.Codebase.Runtime.Map.Component.Character;

public class RDEventOpacityUpdate : MonoBehaviour
{
    private float targetOpacity;
    private float initOpacity;

    private int maxDuration;
    private int nowDuration = 0;

    private CanvasRenderer targetCanvas;

    private void FixedUpdate() {
        if (nowDuration <= 0)
        {
            Destroy(gameObject);
            return;
        }
        nowDuration--;

        int time = maxDuration - nowDuration;

        var nowAlpha = ((targetOpacity * time) + (initOpacity * nowDuration)) / maxDuration;
        targetCanvas.SetAlpha(nowAlpha);
    }

    public void StartChangeOpacity(EventOnMap eventData, int targetOpacity, int duration) {
        maxDuration = duration;
        this.targetOpacity = 1.0f * targetOpacity / 255;
        Transform targetObj = eventData.transform.Find("actor");
        targetCanvas = targetObj.GetComponent<CanvasRenderer>();
        if (targetCanvas == null)
        {
            nowDuration = 0;
            return;
        }
        initOpacity = targetCanvas.GetAlpha();
        nowDuration = duration;
    }
}
