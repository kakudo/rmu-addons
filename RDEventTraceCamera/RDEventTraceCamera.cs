/*:
 * @addondesc EventTraceCamera Add-on
 * @author Kakudo Yuie (Radian)
 * @help The camera follow the event by setting the name of the event in this add-on. If multiple events are specified, the midpoint will be followed.
 *
 * @command TraceOn
 *
 * @command TraceOff
 * 
 * @command TraceOffAndReturn
 */

/*:ja
 * @addondesc イベント追従カメラ
 * @author 角度ゆいえ（Radian）
 * @help 指定したイベントにカメラを追従させることができます。複数のイベントを指定するとその中心を追従します。
 * 
 *  @command TraceOn
 *      @desc カメラにイベントの追従を開始させます
 *      
 *  @arg events
 *      @desc イベント
 *      @type map_event[]
 *      
 *  @arg autoScale
 *      @desc 選択されているイベントが全て収まるように自動でカメラサイズをスケーリングします
 *      @type boolean
 *      @default false
 *      
 *  @arg maxMagnification
 *      @desc autoScaleをonにしている時、最大の拡大率を指定します。
 *      @type number
 *      @default 1.0
 *      
 *  @arg margin
 *      @desc autoScaleをonにしている時、イベントを全て納めた上での余白を設定します。
 *      @type integer
 *      @default 1
 * 
 *  @command TraceOff
 *      @desc イベントの追従を停止します
 * 
 *  @command TraceOffAndReturn
 *      @desc イベントの追従を停止し、アクターにカメラを戻します
 * 
 */
using UnityEngine;
using System.Collections.Generic;
using RPGMaker.Codebase.CoreSystem.Helper;
using RPGMaker.Codebase.Runtime.Map.Component.Character;
using RPGMaker.Codebase.Runtime.Addon;
using SimpleJSON;

namespace RPGMaker.Codebase.Addon
{
    public class RDEventTraceCamera
    {
        private static readonly string PREFAB_PATH = "RDEventTraceCamera/Addon_RDEventTraceCamera";

        public RDEventTraceCamera()
        {

        }

        public void TraceOn(string events, bool autoTrace, double maxMagnification, int margin)
        {
            Camera mainCamera = GetMainCamera();
            if (mainCamera == null) return;
            var eventMapArr = JSON.Parse(events).AsArray;
            var eventIdList = new List<string>();
            for (var i = 0; i < eventMapArr.Count; i++)
            {
                eventIdList.Add(eventMapArr[i].AsArray[1].Value);
            }
            if (eventIdList.Count <= 0) return;
            var eventData = GetTargetEvents(eventIdList);
            if (eventData.Count <= 0) return;
            Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            RDEventTraceCameraUpdate.SetMainCamera(mainCamera);
            RDEventTraceCameraUpdate.StartTrace(eventData);
            if (autoTrace)
            {
                RDEventTraceCameraUpdate.SetAutoTrace(maxMagnification, margin);
            }
        }

        public void TraceOff()
        {
            RDEventTraceCameraUpdate.EndTrace();
        }

        public void TraceOffAndReturn()
        {
            RDEventTraceCameraUpdate.EndTraceAndReturn();
        }

        private Camera GetMainCamera()
        {
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

    }
}