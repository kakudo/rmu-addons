using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RDZoomUpdate : MonoBehaviour
{
    private static RDZoomUpdate m;

    private double targetSize;
    private float targetX;
    private float targetY;
    private int maxDuration;
    private int nowDuration = 0;
    private float initX;
    private float initY;
    private double initSize;
    private Camera mainCamera;

    private bool localFlag;
    private bool simpleFlag;

    private int magEasingType;
    private int posEasingType;

    private static readonly double INIT_CAMERA_SIZE = 5.0;

    private void Awake() {
        if (m == null)
        {
            DontDestroyOnLoad(gameObject);
            m = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate() {
        if (nowDuration <= 0)
        {
            Destroy(gameObject);
            return;
        }
        nowDuration--;

        int time = maxDuration - nowDuration;

        m.mainCamera.orthographicSize = GetNowMag(m.magEasingType, (float) time / 60, (float) maxDuration / 60, (float) initSize, (float) targetSize);

        if (simpleFlag) return;

        Vector2 nowPos = GetNowPos(m.posEasingType, (float) time / 60, (float) maxDuration / 60, new Vector2(m.initX, m.initY), new Vector2(m.targetX, m.targetY));
        if (m.localFlag)
        {
            m.mainCamera.transform.localPosition = new Vector3(nowPos.x, nowPos.y, m.mainCamera.transform.localPosition.z);
        }
        else
        {
            m.mainCamera.transform.position = new Vector3(nowPos.x, nowPos.y, m.mainCamera.transform.position.z);
        }
    }
    public static void SetMainCamera(Camera camera) {
        m.mainCamera = camera;
    }
    public static void StartPositionZoom(double magnification, float targetX, float targetY, int duration) {
        m.simpleFlag = false;
        m.localFlag = false;
        InitZoom(magnification, targetX + 0.5f, targetY + 1, duration);
    }
    public static void StartActorZoom(double magnification, int duration)
    {
        m.simpleFlag = false;
        m.localFlag = true;
        InitZoom(magnification, 0, 1, duration);
    }
    public static void StartSimpleZoom(double magnification, int duration)
    {
        m.simpleFlag = true;
        InitZoom(magnification, 0, 0, duration);
    }
    private static void InitZoom(double magnification, float targetX, float targetY, int duration)
    {
        m.targetSize = INIT_CAMERA_SIZE / magnification;
        m.initSize = m.mainCamera.orthographicSize;
        m.maxDuration = duration;
        m.nowDuration = duration;
        if (!m.simpleFlag)
        {
            m.targetX = targetX;
            m.targetY = targetY;
            if (m.localFlag)
            {
                m.initX = m.mainCamera.transform.localPosition.x;
                m.initY = m.mainCamera.transform.localPosition.y;
            }
            else
            {
                m.initX = m.mainCamera.transform.position.x;
                m.initY = m.mainCamera.transform.position.y;
            }
        }
    }
    public static void SetMagEasing(int magEasing) {
        m.magEasingType = magEasing;
    }
    public static void SetPosEasing(int posEasing) {
        m.posEasingType = posEasing;
    }

    private float GetNowMag(int easingType, float t, float totaltime, float min, float max) {
        switch (easingType)
        {
            case 2:
                return CubicIn(t, totaltime, min, max);
            case 3:
                return CubicOut(t, totaltime, min, max);
            case 4:
                return CubicInOut(t, totaltime, min, max);
        }
        return (max * t + min * (totaltime - t)) / totaltime;
    }

    private float CubicIn(float t, float totaltime, float min, float max) {
        max -= min;
        t /= totaltime;
        return max * t * t * t + min;
    }

    private float CubicOut(float t, float totaltime, float min, float max) {
        max -= min;
        t = t / totaltime - 1;
        return max * (t * t * t + 1) + min;
    }

    private float CubicInOut(float t, float totaltime, float min, float max) {
        max -= min;
        t /= totaltime / 2;
        if (t < 1) return max / 2 * t * t * t + min;

        t = t - 2;
        return max / 2 * (t * t * t + 2) + min;
    }

    private Vector2 GetNowPos(int easingType, float t, float totaltime, Vector2 min, Vector2 max) {
        switch (easingType)
        {
            case 2:
                return CubicInVector(t, totaltime, min, max);
            case 3:
                return CubicOutVector(t, totaltime, min, max);
            case 4:
                return CubicInOutVector(t, totaltime, min, max);
        }
        return (max * t + min * (totaltime - t)) / totaltime;
    }

    private Vector2 CubicInVector(float t, float totaltime, Vector2 min, Vector2 max) {
        max -= min;
        t /= totaltime;
        return max * t * t * t + min;
    }

    private Vector2 CubicOutVector(float t, float totaltime, Vector2 min, Vector2 max) {
        max -= min;
        t = t / totaltime - 1;
        return max * (t * t * t + 1) + min;
    }

    private Vector2 CubicInOutVector(float t, float totaltime, Vector2 min, Vector2 max) {
        max -= min;
        t /= totaltime / 2;
        if (t < 1) return max / 2 * t * t * t + min;

        t = t - 2;
        return max / 2 * (t * t * t + 2) + min;
    }
}
