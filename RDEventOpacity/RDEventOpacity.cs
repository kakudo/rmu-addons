/*:
 * @addondesc Event Opacity
 * @author Kakudo Yuie (Radian)
 * @help For Change Opacity of selected event.
 *
 * @command ChangeOpacity
 * 
 */

/*:ja
 * @addondesc イベント透明度変更
 * @author 角度ゆいえ（Radian）
 * @help 指定したイベントの不透明度を変更します
 * 
 *  @command ChangeOpacity
 *      @text 不透明度
 *      @desc 指定したイベントの不透明度を変更します
 * 
 *  @arg mapEvent
 *      @text イベント
 *      @desc 不透明度変更対象のイベント
 *      @type map_event
 *      
 *  @arg targetOpacity
 *      @text 不透明度
 *      @desc 変更後の不透明度(0～255)
 *      @type integer
 *      @default 255
 *      
 *  @arg duration
 *      @text 終了時間
 *      @desc 終了時間（フレーム単位）
 *      @type integer
 *      @default 60
 *      
 *  @arg waitForEnd
 *      @text 完了までウェイト
 *      @desc 完了までウェイト
 *      @type boolean
 *      @default false
 *      
 */

using UnityEngine;
using System.Collections.Generic;
using RPGMaker.Codebase.CoreSystem.Helper;
using RPGMaker.Codebase.Runtime.Map.Component.Character;
using RPGMaker.Codebase.Runtime.Addon;
using System;
using System.Collections;
using SimpleJSON;
using RPGMaker.Codebase.Runtime.Common;

namespace RPGMaker.Codebase.Addon
{
    public class RDEventOpacity
    {
        private static readonly string PREFAB_PATH = "RDEventOpacity/Addon_RDEventOpacity";

        public RDEventOpacity() {
            
        }

        public void ChangeOpacity(string mapEvent, int targetOpacity, int duration, bool waitForEnd) {
            if (duration == 0) duration = 1;
            var eventArr = JSON.Parse(mapEvent).AsArray;
            var eventId = eventArr[1];
            var eventData = GetTargetEvent(eventId);
            if (eventData == null) return;
            var obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            obj.name = "ChangeOpacityObj";

            obj.GetComponent<RDEventOpacityUpdate>().StartChangeOpacity(eventData, targetOpacity, duration);
            if (waitForEnd)
            {
                var cb = AddonManager.Instance.TakeOutCommandCallback();
                TimeCallback.Register(cb, duration / 60.0f);
            }
        }


        private EventOnMap GetTargetEvent(string eventId)
        {
            EventOnMap ret = null;
            Transform children = GameObject.Find("/Root").GetComponentInChildren<Transform>();
            foreach (Transform child in children)
            {
                EventOnMap eventComponent = child.GetComponent<EventOnMap>();
                if (eventComponent != null)
                {
                    if (eventId == eventComponent.MapDataModelEvent.eventId)
                    {
                        ret = eventComponent;
                    }
                }
            }
            return ret;
        }

        private class TimeCallback
        {
            private static List<TimeCallback> _timeCallbacks = new List<TimeCallback>();
            private Action _callback;
            private float _seconds;

            public static void Register(Action callback, float seconds) {
                _timeCallbacks.Add(new TimeCallback(callback, seconds));
            }

            private TimeCallback(Action callback, float seconds) {
                _callback = callback;
                _seconds = seconds;
                TforuUtility.Instance.StartCoroutine(Wait());
            }

            private IEnumerator Wait() {
                yield return new WaitForSeconds(_seconds);
                _timeCallbacks.Remove(this);
                _callback();
            }
        }

    }
}
