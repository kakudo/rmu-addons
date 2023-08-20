/*:
 * @addondesc Event Move
 * @author Kakudo Yuie (Radian)
 * @help Move the specified event in any direction and any number of steps.
 *
 * @command EventMove
 */

/*:ja
 * @addondesc イベント移動アドオン
 * @author 角度ゆいえ（Radian）
 * @help 指定したイベントを任意の方向に、任意の歩数移動させることができます。マップ上の方向・イベントから見た方向の双方で指定できます。
 * 
 *  @command MoveEvent
 *      @desc イベントを移動させます。
 * 
 *  @arg targetEvent
 *      @desc ターゲットイベント
 *      @type map_event
 *      @default
 *      
 *  @arg direction
 *      @desc 方向
 *      @type select
 *      @option 上（マップ上の方向）
 *      @value 0
 *      @option 右（マップ上の方向）
 *      @value 1
 *      @option 左（マップ上の方向）
 *      @value 2
 *      @option 下（マップ上の方向）
 *      @value 3
 *      @option 前方向（イベントから見て）
 *      @value 4
 *      @option 右方向（イベントから見て）
 *      @value 5
 *      @option 左方向（イベントから見て）
 *      @value 6
 *      @option 後ろ方向（イベントから見て）
 *      @value 7
 *      @default 0
 *      
 *  @arg step
 *      @desc 歩数
 *      @type integer
 *      @default 1
 *      
 *  @arg fixOrientation
 *      @desc 移動中向き固定(移動中にイベントのページを変えると挙動が変になります)
 *      @type boolean
 *      @default false
 *      
 *  @arg frequency
 *      @desc 移動頻度
 *      @type select
 *      @option イベントと同じ
 *      @value 0
 *      @option 最低
 *      @value 1
 *      @option 低
 *      @value 2
 *      @option 標準
 *      @value 3
 *      @option 高
 *      @value 4
 *      @option 最高
 *      @value 5
 *      @default 5
 *      
 *  @arg speed
 *      @desc 移動速度
 *      @type select
 *      @option イベントと同じ
 *      @value 0
 *      @option 1/8倍速
 *      @value 1
 *      @option 1/4倍速
 *      @value 2
 *      @option 1/2倍速
 *      @value 3
 *      @option 標準
 *      @value 4
 *      @option 2倍速
 *      @value 5
 *      @option 4倍速
 *      @value 6
 *      @default 3
 *      
 *  @arg lastDirection
 *      @desc 最後の向き
 *      @type select
 *      @option そのまま
 *      @value 0
 *      @option 上
 *      @value 1
 *      @option 右
 *      @value 2
 *      @option 左
 *      @value 3
 *      @option 下
 *      @value 4
 *      @default 0
 *  
 *  @arg lastOnSwitch
 *      @desc 最後にOnになるスイッチNo（使用しない場合は0）
 *      @type integer
 *      @default 0
 *      
 *  @arg lastMovePosition
 *      @desc 最後に移動する位置(どちらかが-1の場合は移動なし)
 *      @type struct<Position>
 *      
 *  @arg waitForEnd
 *      @desc 完了まで待機
 *      @type boolean
 *      @default false
 */
/*~struct~Position:
 * 
 * @param X
 * @min -1
 * @type integer
 * @default -1
 * 
 * @param Y
 * @min -1
 * @type integer
 * @default -1
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGMaker.Codebase.CoreSystem.Helper;
using RPGMaker.Codebase.Runtime.Map.Component.Character;
using UnityEngine.SceneManagement;
using RPGMaker.Codebase.CoreSystem.Knowledge.Enum;
using RPGMaker.Codebase.Runtime.Addon;

namespace RPGMaker.Codebase.Addon
{
    public class RDEventMove
    {
        private static readonly string PREFAB_PATH = "RDEventMove/Addon_RDEventMove";
        private static List<RDEventMoveUpdate> m;

        public RDEventMove() {
            m = new List<RDEventMoveUpdate>();
        }

        public void MoveEvent(string targetEvent, int direction, int step, bool fixOrientation, int frequency, 
            int speed, int lastDirection, int lastOnSwitch, string lastMovePosition, bool WaitForEnd) {
            string tarEventId = targetEvent.Split(',')[1].Replace("\"", "").Replace("[","").Replace("]", "");
            EventOnMap eventComponent = GetTargetEvent(tarEventId);
            string nextDir = GetNextDirection(direction);
            RDEventMoveUpdate nowEvent = m.Find(emu => emu.eventComponent.MapDataModelEvent.eventId == eventComponent.MapDataModelEvent.eventId);

            if (nowEvent == null)
            {
                GameObject upd = Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
                nowEvent = upd.GetComponent<RDEventMoveUpdate>();
                nowEvent.eventComponent = eventComponent;
                nowEvent.originDirectionFix = eventComponent.MapDataModelEvent.pages[eventComponent.page].walk.directionFix >= 2;
                nowEvent.originSpeed = eventComponent.MapDataModelEvent.pages[eventComponent.page].move.speed;
                nowEvent.lastDirection = lastDirection;
                m.Add(nowEvent);
            }
            if (fixOrientation)
            {
                nowEvent.dirs.Add("X");
            } else
            {
                nowEvent.dirs.Add("O");
            }
            nowEvent.dirs.Add("T");
            nowEvent.dirs.Add(frequency.ToString());
            nowEvent.dirs.Add("V");
            nowEvent.dirs.Add(speed.ToString());
            for (int i=0; i<step; i++)
            {
                if (IsRelativeDir(direction) && !fixOrientation && i > 0)
                {
                    nowEvent.dirs.Add("F");
                } else
                {
                    nowEvent.dirs.Add(nextDir);
                }
            }
            nowEvent.dirs.Add("Z");
            nowEvent.dirs.Add(lastOnSwitch.ToString());
            nowEvent.lastDirection = lastDirection;
            var lastMovePosDic = JsonUtility.FromJson<Position>(lastMovePosition);
            if (lastMovePosDic.X >= 0 && lastMovePosDic.Y >= 0)
            {
                nowEvent.dirs.Add("P");
                nowEvent.dirs.Add(lastMovePosDic.X.ToString());
                nowEvent.dirs.Add(lastMovePosDic.Y.ToString());
            }
            if (WaitForEnd)
            {
                nowEvent.dirs.Add("Q");
                nowEvent.dirs.Add(nowEvent.waitCallBack.Count.ToString());
                nowEvent.waitCallBack.Add(AddonManager.Instance.TakeOutCommandCallback());
            }
        }

        public static void DeleteM(string eventId) {
            m = m.FindAll(emu => emu.eventComponent.MapDataModelEvent.eventId != eventId);
        }

        private string GetNextDirection(int direction) {
            switch (direction)
            {
                case 0:
                    return "N";
                case 1:
                    return "E";
                case 2:
                    return "W";
                case 3:
                    return "S";
                case 4:
                    return "F";
                case 5:
                    return "R";
                case 6:
                    return "L";
                case 7:
                    return "B";
            }
            return "F";
        }
        private bool IsRelativeDir(int direction) {
            switch (direction)
            {
                case 4:
                    return true;
                case 5:
                    return true;
                case 6:
                    return true;
                case 7:
                    return true;
            }
            return false;
        }

        private EventOnMap GetTargetEvent(string eventId) {
            Transform children = GameObject.Find("/Root").GetComponentInChildren<Transform>();
            foreach (Transform child in children)
            {
                EventOnMap eventComponent = child.GetComponent<EventOnMap>();
                if (eventComponent != null)
                {
                    if (eventId == eventComponent.MapDataModelEvent.eventId)
                    {
                        return eventComponent;
                    }
                }
            }
            return null;
        }
        private class Position
        {
            public int X;
            public int Y;
        }
    }
}
