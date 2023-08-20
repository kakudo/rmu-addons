/*:
 * @addondesc Zoom Add-on
 * @author Kakudo Yuie (Radian)
 * @help For zoom in / out during MapScene by calling from event command. Also can be set the coordinates to zoom point and end time.
 *
 * @command SimpleZoom
 *
 * @command ActorZoom
 * 
 * @command EventZoom
 * 
 * @command PositionZoom
 *
 * @command EndZoom
 */

/*:ja
 * @addondesc ズームアドオン
 * @author 角度ゆいえ（Radian）
 * @help マップシーンで任意の場所を指定した時間でズームイン/アウトすることができます。
 * 
 *  @command SimpleZoom
 *      @text 単純ズーム
 *      @desc 現在のカメラ位置でズームだけを行います
 * 
 *  @arg magnification
 *      @text 拡大倍率
 *      @desc 拡大倍率
 *      @type number
 *      @default 1.0
 *      
 *  @arg duration
 *      @text 終了時間
 *      @desc 終了時間（フレーム単位）
 *      @type integer
 *      @default 60
 *      
 *  @arg magEasing
 *      @text 倍率イージング
 *      @desc カメラ倍率のイージング処理
 *      @type select
 *      @option 一定速度
 *      @value 1
 *      @option ゆっくり始まる
 *      @value 2
 *      @option ゆっくり終わる
 *      @value 3
 *      @option ゆっくり始まってゆっくり終わる
 *      @value 4
 *      @default 1
 *  
 *  @arg waitForEnd
 *      @text 完了までウェイト
 *      @desc 完了までウェイト
 *      @type boolean
 *      @default false
 *      
 *  @command ActorZoom
 *      @text アクターズーム
 *      @desc 先頭アクターの位置でズームします
 * 
 *  @arg magnification
 *      @text 拡大倍率
 *      @desc 拡大倍率
 *      @type number
 *      @default 1.0
 *      
 *  @arg duration
 *      @text 終了時間
 *      @desc 終了時間（フレーム単位）
 *      @type integer
 *      @default 60
 *      
 *  @arg magEasing
 *      @text 倍率イージング
 *      @desc カメラ倍率のイージング処理
 *      @type select
 *      @option 一定速度
 *      @value 1
 *      @option ゆっくり始まる
 *      @value 2
 *      @option ゆっくり終わる
 *      @value 3
 *      @option ゆっくり始まってゆっくり終わる
 *      @value 4
 *      @default 1
 *      
 *  @arg posEasing
 *      @text 位置イージング
 *      @desc カメラ位置のイージング処理
 *      @type select
 *      @option 一定速度
 *      @value 1
 *      @option ゆっくり始まる
 *      @value 2
 *      @option ゆっくり終わる
 *      @value 3
 *      @option ゆっくり始まってゆっくり終わる
 *      @value 4
 *      @default 1
 *  
 *  @arg waitForEnd
 *      @text 完了までウェイト
 *      @desc 完了までウェイト
 *      @type boolean
 *      @default false
 *      
 *  @command EventZoom
 *      @text イベントズーム
 *      @desc 指定したイベントの位置でズームします。
 * 
 *  @arg magnification
 *      @text 拡大倍率
 *      @desc 拡大倍率
 *      @type number
 *      @default 1.0
 *      
 *  @arg events
 *      @text イベント
 *      @desc ズーム対象のイベント（複数指定するとその中心）
 *      @type map_event[]
 *      
 *  @arg duration
 *      @text 終了時間
 *      @desc 終了時間（フレーム単位）
 *      @type integer
 *      @default 60
 *      
 *  @arg magEasing
 *      @text 倍率イージング
 *      @desc カメラ倍率のイージング処理
 *      @type select
 *      @option 一定速度
 *      @value 1
 *      @option ゆっくり始まる
 *      @value 2
 *      @option ゆっくり終わる
 *      @value 3
 *      @option ゆっくり始まってゆっくり終わる
 *      @value 4
 *      @default 1
 *      
 *  @arg posEasing
 *      @text 位置イージング
 *      @desc カメラ位置のイージング処理
 *      @type select
 *      @option 一定速度
 *      @value 1
 *      @option ゆっくり始まる
 *      @value 2
 *      @option ゆっくり終わる
 *      @value 3
 *      @option ゆっくり始まってゆっくり終わる
 *      @value 4
 *      @default 1
 *      
 *  @arg waitForEnd
 *      @text 完了までウェイト
 *      @desc 完了までウェイト
 *      @type boolean
 *      @default false
 *      
 *  @command PositionZoom
 *      @text 位置指定ズーム
 *      @desc 座標を指定してズームします
 * 
 *  @arg magnification
 *      @text 拡大倍率
 *      @desc 拡大倍率
 *      @type number
 *      @default 1.0
 *      
 *  @arg x
 *      @text X座標
 *      @desc X座標
 *      @type integer
 *      @default 0
 *      
 *  @arg y
 *      @text Y座標
 *      @desc Y座標
 *      @type integer
 *      @default 0
 *      
 *  @arg duration
 *      @text 終了時間
 *      @desc 終了時間（フレーム単位）
 *      @type integer
 *      @default 60
 *      
 *  @arg magEasing
 *      @text 倍率イージング
 *      @desc カメラ倍率のイージング処理
 *      @type select
 *      @option 一定速度
 *      @value 1
 *      @option ゆっくり始まる
 *      @value 2
 *      @option ゆっくり終わる
 *      @value 3
 *      @option ゆっくり始まってゆっくり終わる
 *      @value 4
 *      @default 1
 *      
 *  @arg posEasing
 *      @text 位置イージング
 *      @desc カメラ位置のイージング処理
 *      @type select
 *      @option 一定速度
 *      @value 1
 *      @option ゆっくり始まる
 *      @value 2
 *      @option ゆっくり終わる
 *      @value 3
 *      @option ゆっくり始まってゆっくり終わる
 *      @value 4
 *      @default 1
 *  
 *  @arg waitForEnd
 *      @text 完了までウェイト
 *      @desc 完了までウェイト
 *      @type boolean
 *      @default false
 *
 *  @command EndZoom
 *      @text ズーム終了
 *      @desc ズームを終了します
 * 
 *  @arg duration
 *      @text 終了時間
 *      @desc 終了時間（フレーム単位）
 *      @type integer
 *      @default 60
 *      
 *  @arg magEasing
 *      @text 倍率イージング
 *      @desc カメラ倍率のイージング処理
 *      @type select
 *      @option 一定速度
 *      @value 1
 *      @option ゆっくり始まる
 *      @value 2
 *      @option ゆっくり終わる
 *      @value 3
 *      @option ゆっくり始まってゆっくり終わる
 *      @value 4
 *      @default 1
 *      
 *  @arg posEasing
 *      @text 位置イージング
 *      @desc カメラ位置のイージング処理
 *      @type select
 *      @option 一定速度
 *      @value 1
 *      @option ゆっくり始まる
 *      @value 2
 *      @option ゆっくり終わる
 *      @value 3
 *      @option ゆっくり始まってゆっくり終わる
 *      @value 4
 *      @default 1
 *      
 *  @arg waitForEnd
 *      @text 完了までウェイト
 *      @desc 完了までウェイト
 *      @type boolean
 *      @default false
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
    public class RDZoom
    {
        private static readonly string PREFAB_PATH = "RDZoom/Addon_RDZoom";

        public RDZoom() {
            
        }

        public void SimpleZoom(double magnification, int duration, int magEasing, bool waitForEnd) {
            if (duration == 0) duration = 1;
            Camera mainCamera = GetMainCamera();
            if (mainCamera == null) return;
            UnityEngine.Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            RDZoomUpdate.SetMainCamera(mainCamera);
            RDZoomUpdate.StartSimpleZoom(magnification, duration);
            if (waitForEnd)
            {
                var cb = AddonManager.Instance.TakeOutCommandCallback();
                TimeCallback.Register(cb, duration / 60.0f);
            }
            RDZoomUpdate.SetMagEasing(magEasing);
        }

        public void ActorZoom(double magnification, int duration, int magEasing, int posEasing, bool waitForEnd)
        {
            if (duration == 0) duration = 1;
            Camera mainCamera = GetMainCamera();
            if (mainCamera == null) return;
            UnityEngine.Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            RDZoomUpdate.SetMainCamera(mainCamera);
            RDZoomUpdate.StartActorZoom(magnification, duration);
            if (waitForEnd)
            {
                var cb = AddonManager.Instance.TakeOutCommandCallback();
                TimeCallback.Register(cb, duration / 60.0f);
            }
            RDZoomUpdate.SetMagEasing(magEasing);
            RDZoomUpdate.SetPosEasing(posEasing);
        }

        public void PositionZoom(double magnification, int x, int y, int duration, int magEasing, int posEasing, bool waitForEnd) {
            if (duration == 0) duration = 1;
            Camera mainCamera = GetMainCamera();
            if (mainCamera == null) return;
            UnityEngine.Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            RDZoomUpdate.SetMainCamera(mainCamera);
            RDZoomUpdate.StartPositionZoom(magnification, x, -1 * y, duration);
            if (waitForEnd)
            {
                var cb = AddonManager.Instance.TakeOutCommandCallback();
                TimeCallback.Register(cb, duration / 60.0f);
            }
            RDZoomUpdate.SetMagEasing(magEasing);
            RDZoomUpdate.SetPosEasing(posEasing);
        }

        public void EventZoom(double magnification, string events, int duration, int magEasing, int posEasing, bool waitForEnd) {
            if (duration == 0) duration = 1;
            Camera mainCamera = GetMainCamera();
            if (mainCamera == null) return;
            var eventMapArr = JSON.Parse(events).AsArray;
            var eventIdList = new List<string>();
            for (var i=0; i< eventMapArr.Count; i++)
            {
                eventIdList.Add(eventMapArr[i].AsArray[1].Value);
            }
            if (eventIdList.Count <= 0) return;
            var eventData = GetTargetEvents(eventIdList);
            if (eventData.Count <= 0) return;
            UnityEngine.Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            RDZoomUpdate.SetMainCamera(mainCamera);
            float xmax = 0;
            float ymax = 0;
            float xmin = 0;
            float ymin = 0;
            bool upd = false;
            eventData.ForEach(e =>
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
                }
                else
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

            RDZoomUpdate.StartPositionZoom(magnification, x, y, duration);
            if (waitForEnd)
            {
                var cb = AddonManager.Instance.TakeOutCommandCallback();
                TimeCallback.Register(cb, duration / 60.0f);
            }
            RDZoomUpdate.SetMagEasing(magEasing);
            RDZoomUpdate.SetPosEasing(posEasing);
        }

        public void EndZoom(int duration, int magEasing, int posEasing, bool waitForEnd) {
            if (duration == 0) duration = 1;
            UnityEngine.Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            Camera mainCamera = GetMainCamera();
            if (mainCamera == null) return;
            RDZoomUpdate.SetMainCamera(mainCamera);
            RDZoomUpdate.StartActorZoom(1.0, duration);
            if (waitForEnd)
            {
                var cb = AddonManager.Instance.TakeOutCommandCallback();
                TimeCallback.Register(cb, duration / 60.0f);
            }
            RDZoomUpdate.SetMagEasing(magEasing);
            RDZoomUpdate.SetPosEasing(posEasing);
        }

        private Camera GetMainCamera() {
            Transform children = GameObject.Find("/Root").GetComponentInChildren<Transform>();
            foreach (Transform child in children)
            {
                ActorOnMap actorComponent = child.GetComponent<ActorOnMap>();
                if (actorComponent != null)
                {
                    Transform mainCameraObj = child.Find("Main Camera");
                    if (mainCameraObj != null)
                    {
                        return mainCameraObj.GetComponent<Camera>();
                    }
                }
            }
            return null;
        }

        private List<EventOnMap> GetTargetEvents(List<string> eventIdList)
        {
            var ret = new List<EventOnMap>();
            Transform children = GameObject.Find("/Root").GetComponentInChildren<Transform>();
            foreach (Transform child in children)
            {
                EventOnMap eventComponent = child.GetComponent<EventOnMap>();
                if (eventComponent != null)
                {
                    if (eventIdList.Contains(eventComponent.MapDataModelEvent.eventId))
                    {
                        ret.Add(eventComponent);
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
