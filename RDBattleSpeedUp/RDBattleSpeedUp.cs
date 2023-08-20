/*:
 * @addondesc Battle SpeedUp
 * @author Kakudo Yuie (Radian)
 * @help Battle Speed Up!!
 * 
 * @param execSwitchNo
 * @text execSwitchNo
 * @desc execSwitchNo
 * @type integer
 * @default 0
 * 
 * @param skipOnRate
 * @text skipOnRate
 * @desc skipOnRate
 * @type integer
 * @default 3
 * @min 1
 * 
 * @param normalRate
 * @text normalRate
 * @desc normalRate
 * @type integer
 * @default 1
 * @min 1
 * 
 */

/*:ja
 * @addondesc バトル速度UP
 * @author 角度ゆいえ（Radian）
 * @help バトルの速度をアップします。このアドオンは師走冬也様制作の「ReplaceCodesAddon」によるコード置換を前提としています。
 * 
 * @param execSwitchNo
 * @text 起動スイッチNo
 * @desc アドオンの効果を起動するスイッチNo（0の時は常時）
 * @type integer
 * @default 0
 * 
 * @param skipOnRate
 * @text スキップ時速度倍率
 * @desc 決定ボタンを押している時の倍率（既定：3倍）
 * @type integer
 * @default 3
 * @min 1
 * 
 * @param normalRate
 * @text 通常時速度倍率
 * @desc 通常時の倍率（既定：1倍）
 * @type integer
 * @default 1
 * @min 1
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
using RPGMaker.Codebase.Runtime.Battle.Window;
using Effekseer;
using RPGMaker.Codebase.Runtime.Common.Component.Hud.Character;
using RPGMaker.Codebase.Runtime.Battle.Sprites;

namespace RPGMaker.Codebase.Addon
{
    public class RDBattleSpeedUp
    {
        private static int execSwitchNo;
        private static int skipOnRate;
        private static int normalRate;

        public RDBattleSpeedUp(int execSwitchNo, int skipOnRate, int normalRate)
        {
            RDBattleSpeedUp.execSwitchNo = execSwitchNo;
            RDBattleSpeedUp.skipOnRate = skipOnRate;
            RDBattleSpeedUp.normalRate = normalRate;

        }

        public static int GetExecSwitchNo() {
            return execSwitchNo;
        }
        public static int GetSkipOnRate() {
            return skipOnRate;
        }
        public static int GetNormalRate() {
            return normalRate;
        }
    }
    public static class RDBattleSpeedUpInternal
    {
        public static bool IsFastForward() {
            if (InputHandler.OnPress(Runtime.Common.Enum.HandleType.Decide) ||
                InputHandler.OnPress(Runtime.Common.Enum.HandleType.LeftShiftDown))
            {
                return true;
            }
            return false;
        }

    }
}

