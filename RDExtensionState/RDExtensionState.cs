/*:
 * @addondesc Extension State
 * @author Kakudo Yuie (Radian)
 * @help When the special state is released, another state is added.
 * 
 */

/*:ja
 * @addondesc 拡張ステート
 * @author 角度ゆいえ（Radian）
 * @help 
 * 解除された時に別のステートを付与する特殊なステートが作れるようになります。
 * <RD_EXT_STATE_ID:【派生するステートのID※】>
 * 　※通常は名前を見るが、RD_STATE_IDが設定されてるステートはそちらを見る
 * <RD_STATE_ID:【そのステートのID】>
 * 
 * 特定の能力値を値で増減させられる特殊なステートが作れるようになります。
 * <RD_STATE_VAL_BUFF:【能力値区分（数値を-で複数指定）】,【値（数値）】>
 * 例（<RD_STATE_VAL_BUFF:3-5,20>）
 * 【能力値区分】
 * Mhp:0
 * Mmp:1
 * Atk:2
 * Def:3
 * Mat:4
 * Mdf:5
 * Agi:6
 * Luk:7
 * 
 * 同じステートを付けられた時にカウントを初期化しないようにする場合は以下をステートのメモ欄に追加してください。
 * <RD_DISABLE_RESET_STATE>
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
using RPGMaker.Codebase.Runtime.Battle.Objects;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.State;
using System.Text.RegularExpressions;
using RPGMaker.Codebase.Runtime.Common;

namespace RPGMaker.Codebase.Addon
{
    public class RDExtensionState
    {
        public RDExtensionState()
        {
        }
    }
    public static class RDExtensionStateInternal
    {
        public static string GetRDStateId(StateDataModel state)
        {
            Regex reg = new Regex("<RD_STATE_ID:(?<note>.*?)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(state.note);
            if (!m.Success) return state.name;
            return m.Groups["note"].Value;
        }
        public static string GetRDExtStateId(StateDataModel state)
        {
            Regex reg = new Regex("<RD_EXT_STATE_ID:(?<note>.*?)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(state.note);
            if (!m.Success) return null;
            return m.Groups["note"].Value;
        }
        public static string GetRDExtStateValBuff(StateDataModel state) {
            Regex reg = new Regex("<RD_STATE_VAL_BUFF:(?<note>.*?)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(state.note);
            if (!m.Success) return null;
            return m.Groups["note"].Value;
        }
        public static bool IsDisableResetState(StateDataModel state) {
            Regex reg = new Regex("<RD_DISABLE_RESET_STATE>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(state.note);
            if (!m.Success) return false;
            return true;
        }
    }
}

