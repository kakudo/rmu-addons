using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGMaker.Codebase.Runtime.Map.Component.Character;
using RPGMaker.Codebase.CoreSystem.Knowledge.Enum;
using RPGMaker.Codebase.Addon;
using System;
using System.Reflection;
using RPGMaker.Codebase.CoreSystem.Helper;
using RPGMaker.Codebase.Runtime.Common;
using RPGMaker.Codebase.Runtime.Map;
public class RDEventMoveUpdate : MonoBehaviour
{
    public EventOnMap eventComponent;
    public List<string> dirs = new List<string>();

    public bool originDirectionFix;
    public int frequency;

    public int originSpeed;
    public int lastDirection;

    private int interval = 0;
    private int nowTime = 0;

    public List<Action> waitCallBack = new();

    void FixedUpdate()
    {
        if (dirs.Count <= 0)
        {
            if (eventComponent)
            {
                RDEventMove.DeleteM(eventComponent.MapDataModelEvent.eventId);
                eventComponent.SetIsLockDirection(originDirectionFix);
                eventComponent.SetCharacterSpeed(GetMoveSpeed(originSpeed));
                switch (lastDirection)
                {
                    case 1:
                        eventComponent.ChangeCharacterDirection(CharacterMoveDirectionEnum.Up);
                        break;
                    case 2:
                        eventComponent.ChangeCharacterDirection(CharacterMoveDirectionEnum.Right);
                        break;
                    case 3:
                        eventComponent.ChangeCharacterDirection(CharacterMoveDirectionEnum.Left);
                        break;
                    case 4:
                        eventComponent.ChangeCharacterDirection(CharacterMoveDirectionEnum.Down);
                        break;
                }
            }
            Destroy(gameObject);
            return;
        }

        if (interval > nowTime)
        {
            nowTime++;
            return;
        }

        Type eMapClassType = eventComponent.GetType();
        FieldInfo _privateIsMoving = eMapClassType.GetField("_isMoving", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);
        bool isMoving = (bool) (_privateIsMoving.GetValue(eventComponent));

        if (!isMoving)
        {
            string nowDir = dirs[0];
            dirs.RemoveAt(0);
            if (nowDir == "O")
            {
                eventComponent.SetIsLockDirection(false);
                nowDir = dirs[0];
                dirs.RemoveAt(0);
            }
            else if (nowDir == "X")
            {
                eventComponent.SetIsLockDirection(true);
                nowDir = dirs[0];
                dirs.RemoveAt(0);
            }
            if (nowDir == "T")
            {
                nowDir = dirs[0];
                dirs.RemoveAt(0);
                int intervalType = int.Parse(nowDir);
                if (intervalType == 0)
                {
                    intervalType = eventComponent.MapDataModelEvent.pages[eventComponent.page].move.frequency;
                }
                interval = GetInterval(intervalType);
                nowDir = dirs[0];
                dirs.RemoveAt(0);
            }
            if (nowDir == "V")
            {
                nowDir = dirs[0];
                dirs.RemoveAt(0);
                int speedType = int.Parse(nowDir);
                if (speedType == 0)
                {
                    speedType = eventComponent.MapDataModelEvent.pages[eventComponent.page].move.speed + 1;
                }
                eventComponent.SetCharacterSpeed(GetMoveSpeed(speedType - 1));
                nowDir = dirs[0];
                dirs.RemoveAt(0);
            }
            if (nowDir == "Z")
            {
                nowDir = dirs[0];
                dirs.RemoveAt(0);
                int onSwitchNo = int.Parse(nowDir);
                if (onSwitchNo > 0)
                {
                    DataManager.Self().GetRuntimeSaveDataModel().switches.data[onSwitchNo - 1] = true;
                }
                if (dirs.Count <= 0) return;
                nowDir = dirs[0];
                dirs.RemoveAt(0);
            }
            if (nowDir == "P")
            {
                int moveX = int.Parse(dirs[0]);
                dirs.RemoveAt(0);
                int moveY = -1 * int.Parse(dirs[0]);
                dirs.RemoveAt(0);
                eventComponent.SetToPositionOnTile(new Vector2(moveX, moveY));
                //eventComponent.y_next--;
                //eventComponent.y_now--;
                if (dirs.Count <= 0) return;
                nowDir = dirs[0];
                dirs.RemoveAt(0);
            }
            if (nowDir == "Q")
            {
                int callBackIndex = int.Parse(dirs[0]);
                dirs.RemoveAt(0);

                waitCallBack[callBackIndex]();

                if (dirs.Count <= 0) return;
                nowDir = dirs[0];
                dirs.RemoveAt(0);
            }
            if (nowDir == "N" || nowDir == "S" || nowDir == "E" || nowDir == "W" || nowDir == "F" || nowDir == "R" || nowDir == "L" || nowDir == "B")
            {
                CharacterMoveDirectionEnum d = eventComponent.GetCurrentDirection();
                switch (nowDir)
                {
                    case "N":
                        eventComponent.MoveUpOneUnit();
                        break;
                    case "S":
                        eventComponent.MoveDownOneUnit();
                        break;
                    case "E":
                        eventComponent.MoveRightOneUnit();
                        break;
                    case "W":
                        eventComponent.MoveLeftOneUnit();
                        break;
                    case "F":
                        switch (d)
                        {
                            case CharacterMoveDirectionEnum.Up:
                                eventComponent.MoveUpOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Down:
                                eventComponent.MoveDownOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Right:
                                eventComponent.MoveRightOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Left:
                                eventComponent.MoveLeftOneUnit();
                                break;
                        }
                        break;
                    case "R":
                        switch (d)
                        {
                            case CharacterMoveDirectionEnum.Up:
                                eventComponent.MoveRightOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Down:
                                eventComponent.MoveLeftOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Right:
                                eventComponent.MoveDownOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Left:
                                eventComponent.MoveUpOneUnit();
                                break;
                        }
                        break;
                    case "L":
                        switch (d)
                        {
                            case CharacterMoveDirectionEnum.Up:
                                eventComponent.MoveLeftOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Down:
                                eventComponent.MoveDownOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Right:
                                eventComponent.MoveUpOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Left:
                                eventComponent.MoveDownOneUnit();
                                break;
                        }
                        break;
                    case "B":
                        switch (d)
                        {
                            case CharacterMoveDirectionEnum.Up:
                                eventComponent.MoveDownOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Down:
                                eventComponent.MoveUpOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Right:
                                eventComponent.MoveLeftOneUnit();
                                break;
                            case CharacterMoveDirectionEnum.Left:
                                eventComponent.MoveRightOneUnit();
                                break;
                        }
                        break;
                }
                nowTime = 0;
            }
        }
    }
    private int GetInterval(int intervalType) {
        switch (intervalType)
        {
            case 1:
                return 120;
            case 2:
                return 90;
            case 3:
                return 60;
            case 4:
                return 40;
            case 5:
                return 0;
        }
        return 60;
    }
    private float GetMoveSpeed(int speedType) {
        float speed = 6f;
        switch (speedType)
        {
            case 0:
                speed = speed / 8;
                break;
            case 1:
                speed = speed / 4;
                break;
            case 2:
                speed = speed / 2;
                break;
            case 4:
                speed = speed * 2;
                break;
            case 5:
                speed = speed * 4;
                break;
        }
        return speed;
    }
}
