/*:
 * @addondesc Skill Switch
 * @author Kakudo Yuie (Radian)
 * @help Turn on / off the specified switch when using the skill or item in battle.
 * 
 */

/*:ja
 * @addondesc スキルスイッチ
 * @author 角度ゆいえ（Radian）
 * @help 特定のスキルをバトル中に使用した時に指定したスイッチをON/OFFできるようにする。
 * 例えばスキルのメモ欄に<RD_SKILL_SWITCH:32,31,31,32>
 * のように入れると、
 * <RD_SKILL_SWITCH:
 *  【このスキルを使うとONになるスイッチ】,
 *  【このスキルを使うとOFFになるスイッチ】,
 *  【このスイッチがONの時しかスキルは使えない】,
 *  【このスイッチがONになるとこのスキルは使えなくなる】>
 * として処理が行われる。（0を入れるとその要素は無視）
 * 
 * このアドオンは師走冬也様制作の「ReplaceCodesAddon」によるコード置換を前提としています。
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using RPGMaker.Codebase.CoreSystem.Helper;
using RPGMaker.Codebase.Runtime.Battle;
using RPGMaker.Codebase.Runtime.Battle.Objects;
using System.Text.RegularExpressions;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Runtime;
using RPGMaker.Codebase.Runtime.Common;

namespace RPGMaker.Codebase.Addon
{
    public class RDSkillSwitch
    {
        public RDSkillSwitch()
        {
        }
    }
    public class RDSkillSwitchDataModel
    {
        public int execOnSwitchNo = 0;
        public int execOffSwitchNo = 0;
        private int OnSwitchNo = 0;
        private int OffSwitchNo = 0;

        public RDSkillSwitchDataModel(string[] notes)
        {
            int eOn = int.Parse(notes[0]);
            if (eOn > 0)
            {
                execOnSwitchNo = eOn;
            }
            if (notes.Length > 1 && notes[1] != "")
            {
                int eOff = int.Parse(notes[1]);
                if (eOff > 0)
                {
                    execOffSwitchNo = eOff;
                }
            }
            if (notes.Length > 2 && notes[2] != "")
            {
                int on = int.Parse(notes[2]);
                if (on > 0)
                {
                    OnSwitchNo = on;
                }
            }
            if (notes.Length > 3 && notes[3] != "")
            {
                int off = int.Parse(notes[3]);
                if (off > 0)
                {
                    OffSwitchNo = off;
                }
            }
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
                if (dataOn) return true;
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
    }
    public static class RDSkillSwitchInternal
    {
        public static RDSkillSwitchDataModel GetSkillSwitchModelFromMemo(string memo)
        {
            if (memo == null) return null;
            Regex reg = new Regex("<RD_SKILL_SWITCH:(?<note>.*?)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(memo);
            if (!m.Success) return null;
            string note = m.Groups["note"].Value;
            return new RDSkillSwitchDataModel(note.Split(','));
        }
    }

}
