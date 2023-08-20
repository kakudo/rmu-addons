/*:
 * @addondesc FadeOut
 * @author Kakudo Yuie (Radian)
 * @help Fade out screen with your own UI, and execute next events during the fade out.
 * Version 0.1
 *
 * @command FadeOut
 *
 * @command FadeIn
*/

/*:ja
 * @addondesc フェードアウト
 * @author 角度ゆいえ（Radian）
 * @help 画面をフェードアウトします。独自UIでもフェードアウトでき、フェードアウト中に次イベントを動かすこともできます。
 * Version 0.1
 * 
 *  @command FadeOut
 *      @desc フェードアウトします
 * 
 *  @arg duration
 *      @desc 終了時間（フレーム単位）
 *      @type integer
 *      @default 60
 *      
 *  @arg bgPicture
 *      @text 背景画像
 *      @desc 背景画像ファイルを指定。未指定の場合は黒塗り
 *      @type file
 *  
 *  @arg waitForEnd
 *      @desc 完了までウェイト
 *      @type boolean
 *      @default false
 *      
 *  @command FadeIn
 *      @desc フェードインします。フェードアウトしていない場合は何も起こりません。
 * 
 *  @arg duration
 *      @desc 終了時間（フレーム単位）
 *      @type integer
 *      @default 60
 *      
 *  @arg waitForEnd
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
using RPGMaker.Codebase.Runtime.Common;
using UnityEngine.SceneManagement;

namespace RPGMaker.Codebase.Addon
{
    public class RDFadeOut
    {
        private static readonly string PREFAB_PATH = "RDFadeOut/Addon_RDFadeOut";
        private static GameObject m;

        public RDFadeOut() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (m != null) return;
            if (scene.name != "SceneMap") return;
            m = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            m.name = "Addon_RDFadeOut";
        }

        public void FadeOut(int duration, string bgPicture, bool waitForEnd) {
            if (duration == 0) duration = 1;
            if (m == null) return;

            var fadeout = m.GetComponent<RDFadeOutUpdate>();

            if (bgPicture != null && bgPicture != "")
            {
                var texture2D =
                    UnityEditorWrapper.AssetDatabaseWrapper.LoadAssetAtPath<Texture2D>("Assets/RPGMaker/Storage/" + bgPicture + ".png");
                Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                    Vector2.zero);

                fadeout.SetBgImage(sprite);
            } else
            {
                fadeout.InitBgImage();
            }

            fadeout.SetOption(duration, 1f);
            if (waitForEnd)
            {
                var cb = AddonManager.Instance.TakeOutCommandCallback();
                TimeCallback.Register(cb, duration / 60.0f);
            }
        }

        public void FadeIn(int duration, bool waitForEnd) {
            if (m == null) return;
            if (duration == 0) duration = 1;

            var fadeout = m.GetComponent<RDFadeOutUpdate>();

            fadeout.SetOption(duration, 0);
            if (waitForEnd)
            {
                var cb = AddonManager.Instance.TakeOutCommandCallback();
                TimeCallback.Register(cb, duration / 60.0f);
            }
        }

        private class TimeCallback
        {
            private static List<TimeCallback> _timeCallbacks = new();
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


