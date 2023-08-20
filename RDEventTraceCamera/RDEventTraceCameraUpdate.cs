using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using RPGMaker.Codebase.Runtime.Map;
using RPGMaker.Codebase.CoreSystem.Helper;
using RPGMaker.Codebase.Runtime.Map.Component.Character;

public class RDEventTraceCameraUpdate : MonoBehaviour
{
    private static RDEventTraceCameraUpdate m;

    private Camera mainCamera;

    private bool endFlag = false;
    private bool returnFlag = false;
    private List<EventOnMap> events;

    private float tmpX;
    private float tmpY;

    private bool autoScale;
    private float minCameraSize;
    private int margin;

    private static readonly float INIT_CAMERA_SIZE = 5.0f;
    private static readonly int INIT_LATERAL_PIXEL_SIZE = 1920;
    private static readonly int INIT_VERTICAL_PIXEL_SIZE = 1080;
    private static readonly float INIT_LATERAL_TILE_SIZE = 18.2f;
    private static readonly float INIT_VERTICAL_TILE_SIZE = INIT_LATERAL_TILE_SIZE * INIT_VERTICAL_PIXEL_SIZE / INIT_LATERAL_PIXEL_SIZE;

    private float tmpCameraSize = INIT_CAMERA_SIZE;

    private void Awake()
    {
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
    private void FixedUpdate()
    {
        if (endFlag)
        {
            if (returnFlag)
            {
                m.mainCamera.transform.localPosition = new Vector3(0, 1, m.mainCamera.transform.localPosition.z);
            }
            m = null;
            Destroy(gameObject);
            return;
        }
        if (m == null || m.events == null || m.events.Count <= 0) return;
        float xmax = 0;
        float ymax = 0;
        float xmin = 0;
        float ymin = 0;
        bool upd = false;
        m.events.ForEach(e =>
        {
            float x = e.transform.position.x;
            float y = e.transform.position.y;
            if (upd)
            {
                if (x > xmax)
                {
                    xmax = x;
                }
                else if (x < xmin)
                {
                    xmin = x;
                }
                if (y > ymax)
                {
                    ymax = y;
                }
                else if (y < ymin)
                {
                    ymin = y;
                }
            } else
            {
                xmax = x;
                xmin = x;
                ymax = y;
                ymin = y;
                upd = true;
            }
        });
        float x = (xmax + xmin) / 2;
        float y = (ymax + ymin) / 2;
        if (tmpX == x && tmpY == y) return;
        tmpX = x;
        tmpY = y;
        m.mainCamera.transform.position = new Vector3(x + 0.5f, y + 1, m.mainCamera.transform.position.z);

        if (!m.autoScale) return;
        float sizeLateral = xmax - xmin + 1;
        float sizeVertical = ymax - ymin + 2;
        sizeLateral += 2 * margin;
        sizeVertical += 2 * margin;
        float targetCameraSizeX = INIT_CAMERA_SIZE * sizeLateral / INIT_LATERAL_TILE_SIZE;
        float targetCameraSizeY = INIT_CAMERA_SIZE * sizeVertical / INIT_VERTICAL_TILE_SIZE;
        float targetCameraSize = Math.Max(targetCameraSizeX, targetCameraSizeY);
        if (targetCameraSize < minCameraSize) targetCameraSize = minCameraSize;

        if (tmpCameraSize == targetCameraSize) return;
        tmpCameraSize = targetCameraSize;
        m.mainCamera.orthographicSize = targetCameraSize;
    }
    public static void SetMainCamera(Camera camera)
    {
        m.mainCamera = camera;
    }
    public static void SetAutoTrace(double maxMagnification, int margin)
    {
        m.autoScale = true;
        m.minCameraSize = (float) (INIT_CAMERA_SIZE / maxMagnification);
        m.margin = margin;
    }
    public static void StartTrace(List<EventOnMap> events)
    {
        m.events = events;
        m.autoScale = false;
    }
    public static void EndTrace()
    {
        if (m == null) return;
        if (m.mainCamera == null) return;
        m.events = new();
        m.returnFlag = false;
        m.endFlag = true;
    }
    public static void EndTraceAndReturn()
    {
        if (m == null) return;
        if (m.mainCamera == null) return;
        m.events = new();
        m.returnFlag = true;
        m.endFlag = true;
    }
}
