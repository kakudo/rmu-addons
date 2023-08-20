/*:
 *@addondesc Map Info
 * @author Kakudo Yuie (Radian)
 *@help Display a infomation on the screen when an actor touches a specific event.
 * 
 */

/*:ja
 * @addondesc マップ情報表示
 * @author 角度ゆいえ（Radian）
 * @help 特定のイベントにアクターが接触した時に説明を画面上に表示する。
 * 例えばイベントのメモ欄に<RD_MAP_INFO:"説明",forward,2,3>
 * のように入れると、
 * <RD_MAP_INFO:
 *  【出てくる説明】,
 *  【（任意）接触する場所の区分（forward（アクターの前:既定）/on(アクターの場所)/onforward（両方）/up(上のみ)/down(下のみ)/left(左のみ)/right(右のみ)）】,
 *  【（任意）このスイッチNoがONの時のみ表示】,
 *  【（任意）このスイッチNoがONになると表示しなくなる】>
 * として処理が行われる。
 * 自作の為に作っているので表示は固定・白字となり見にくいかもしれません。
 * 適宜prefabの中身を変更してください。
 * 
 */

using UnityEngine;
using RPGMaker.Codebase.Runtime.Map.Component.Character;
using System.Text.RegularExpressions;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Runtime;
using RPGMaker.Codebase.CoreSystem.Helper;
using System;
using UnityEngine.SceneManagement;
using RPGMaker.Codebase.Runtime.Common;

namespace RPGMaker.Codebase.Addon
{
    public class RDMapInfo
    {
        private static readonly string PREFAB_PATH = "RDMapInfo/RDMapInfo";
        private static GameObject m;

        public RDMapInfo()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (m != null) return;
            if (scene.name != "SceneMap") return;
            m = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            m.name = "RDMapInfo";
        }
    }
    public class RDInfo
    {
        public EventOnMap component;
        public string content;
        public RDInfoPlaceEnum infoPlace;
        private int OnSwitchNo = 0;
        private int OffSwitchNo = 0;

        public RDInfo(EventOnMap eventComponent)
        {
            component = eventComponent;
            string[] notes = GetInfoEventNotes(eventComponent);
            content = notes[0];
            if (notes.Length >= 2 && notes[1] != "")
            {
                infoPlace = GetEnumFromString<RDInfoPlaceEnum>(notes[1]);
            }
            else
            {
                infoPlace = RDInfoPlaceEnum.forward;
            }
            if (notes.Length >= 3 && notes[2] != "" && notes[2] != "0")
            {
                OnSwitchNo = int.Parse(notes[2]);
            }
            if (notes.Length >= 4 && notes[3] != "" && notes[3] != "0")
            {
                OffSwitchNo = int.Parse(notes[3]);
            }
        }
        public RDInfo() {

        }
        public bool isActive()
        {
            RuntimeSaveDataModel _runtimeSaveDataModel = DataManager.Self().GetRuntimeSaveDataModel();
            if (OffSwitchNo == 0 && OnSwitchNo == 0) return true;
            if (OffSwitchNo != 0 && OnSwitchNo != 0)
            {
                bool dataOff = _runtimeSaveDataModel.switches.data[OffSwitchNo - 1];
                bool dataOn = _runtimeSaveDataModel.switches.data[OnSwitchNo - 1];
                if (!dataOff && !dataOn) return false;
                if (!dataOff && dataOn) return true;
                return false;
            }
            if (OffSwitchNo != 0)
            {
                if (_runtimeSaveDataModel.switches.data[OffSwitchNo - 1]) return false;
                return true;
            }
            if (_runtimeSaveDataModel.switches.data[OnSwitchNo - 1]) return true;
            return false;
        }
        public static bool IsInfo(EventOnMap eventComponent)
        {
            if (GetInfoEventNotes(eventComponent) == null) return false;
            return true;
        }
        public static string[] GetInfoEventNotes(EventOnMap eventComponent)
        {
            Regex reg = new Regex("<RD_MAP_INFO:(?<note>.*?)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(eventComponent.MapDataModelEvent.note);
            if (!m.Success) return null;
            string note = m.Groups["note"].Value;
            string[] notes = note.Split(',');
            return notes;
        }
        private static T GetEnumFromString<T>(string targetTypeStr)
        {
            return (T)Enum.Parse(typeof(T), targetTypeStr);
        }
    }

    public enum RDInfoPlaceEnum
    {
        forward,
        on,
        onforward,
        up,
        down,
        left,
        right,
    }

}
