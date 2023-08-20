/*:
 * @addondesc Dungeon Light
 * @author Kakudo Yuie (Radian)
 * @help Only illuminate around actors and specified events, darken others.
 * 
 * @param enableSwitchNo
 * @text enableSwitchNo
 * @desc Dungeon Light is enabled while this number of switch is active.
 * @type integer
 * @default 0
 * 
 */

/*:ja
 * @addondesc ダンジョンライト
 * @author 角度ゆいえ（Radian）
 * @help アクターや特定のイベントの周囲だけを光らせ、他を暗くする。
 * イベントのメモ欄に<RD_MAP_LIT:5,2,3>
 * のように入れると、
 * <RD_MAP_LIT:
 *  【光の大きさ】,
 *  【（任意）このスイッチNoがONの時のみ光る】,
 *  【（任意）このスイッチNoがONになると光らない】>
 * として処理が行われる。
 * また、<RD_FRIEND>を指定したイベントにはアクターと同様光が追従する。
 * 
 * @param enableSwitchNo
 * @text 有効化スイッチ番号
 * @desc この番号のスイッチをONにしている間、暗闇と光源が有効化されます。
 * @type integer
 * @default 4
 * 
 * @param traceMode
 * @text トレースモード
 * @desc 0：アクターイベントに追従、1：カメラに追従（RDZoomなどでアクターとカメラを離す場合に使用）
 * @type integer
 * @default 0
 * 
 * @command ColorChange
 *      @text 暗闇色変更
 *      @desc 光が当たっていない部分の色を変更します
 * 
 * @arg color
 *      @text 暗闇色
 *      @desc 光が当たっていない部分の色
 *      @type struct<Color>
 *      @default {"Red": 255, "Green": 0, "Blue": 0}
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
 *  @command ColorBack
 *      @text 暗闇色戻し
 *      @desc 光が当たっていない部分の色を黒に戻します
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
 *  @command ChangeTraceMode
 *      @text トレースモード変更
 *      @desc トレースモードを変更します（0：アクターイベントに追従、1：カメラに追従）
 * 
 *  @arg traceMode
 *      @text トレースモード
 *      @desc 0：アクターイベントに追従、1：カメラに追従（RDZoomなどでアクターとカメラを離す場合に使用）
 *      @type integer
 *      @default 0
 *      
 *  @command ChangeActorLightRadius
 *      @text アクター光源半径変更
 *      @desc アクター光源の半径を変更します
 * 
 *  @arg actorLightRadius
 *      @text アクター光源半径
 *      @desc アクター光源の半径
 *      @type number
 *      @default 2.5
 *      
 *  @arg immediet
 *      @text 即時変更
 *      @desc 即時変更する場合はON, そうでない場合は少しずつ変更
 *      @type boolean
 *      @default true
 * 
 */
/*~struct~Color:
 * 
 * @param Red
 * @text 赤
 * @max 255
 * @type integer
 * @default 0
 * 
 * @param Green
 * @max 255
 * @type integer
 * @default 0
 * 
 * @param Blue
 * @max 255
 * @type integer
 * @default 0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGMaker.Codebase.CoreSystem.Helper;
using UnityEngine.SceneManagement;
using RPGMaker.Codebase.Runtime.Addon;
using System;
using RPGMaker.Codebase.Runtime.Common;

namespace RPGMaker.Codebase.Addon
{
    public class RDDungeonLight
    {
        private static readonly string PREFAB_PATH = "RDDungeonLight/RDDungeonLight";
        private static GameObject m;

        private int enableSwitchNo;
        private int defaultActorLightTraceMode;

        RDLightHandler lightHandler;

        public RDDungeonLight(int enableSwitchNo, int traceMode)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            this.enableSwitchNo = enableSwitchNo;
            defaultActorLightTraceMode = traceMode;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (m != null) return;
            if (scene.name != "SceneMap") return;
            m = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            m.name = "RDDungeonLight";

            m.GetComponent<RDDungeonLightSwitch>().enableSwitchNo = enableSwitchNo;
            lightHandler = m.transform.Find("RDLightHandler").GetComponent<RDLightHandler>();
        }

        public void ColorChange(string color, int duration, bool waitForEnd) {
            if (lightHandler == null) return;
                if (duration == 0) duration = 1;
            var darkColor = JsonUtility.FromJson<DarkColor>(color);
            lightHandler.SetTargetColor(new Color(darkColor.Red / 255f, darkColor.Green / 255f, darkColor.Blue / 255f), duration);
            if (waitForEnd)
            {
                var cb = AddonManager.Instance.TakeOutCommandCallback();
                TimeCallback.Register(cb, duration / 60.0f);
            }
        }

        public void ColorBack(int duration, bool waitForEnd) {
            if (lightHandler == null) return;
            if (duration == 0) duration = 1;
            lightHandler.SetTargetColor(new Color(0, 0, 0), duration);
            if (waitForEnd)
            {
                var cb = AddonManager.Instance.TakeOutCommandCallback();
                TimeCallback.Register(cb, duration / 60.0f);
            }
        }

        public void ChangeTraceMode(int traceMode) {
            if (lightHandler == null) return;
            lightHandler.SetActorTraceMode(traceMode);
        }

        public void ChangeActorLightRadius(double actorLightRadius, bool immediet) {
            if (lightHandler == null) return;
            lightHandler.SetActorLightRadius((float) actorLightRadius, immediet);
        }

        private class DarkColor
        {
            public int Red;
            public int Green;
            public int Blue;
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
