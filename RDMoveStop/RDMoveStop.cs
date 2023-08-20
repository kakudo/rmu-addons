/*:
 * @addondesc Move Stop
 * @author Kakudo Yuie (Radian)
 * @help Stop normal input of Game by switch
 * 
 * @param moveStopSwitchNo
 * @text Move stop switch no
 * @desc Move stop switch no
 * @type integer
 * @default 2
 */

/*:ja
 * @addondesc 移動停止スイッチ
 * @author 角度ゆいえ（Radian）
 * @help 指定したスイッチをONにすると、ゲーム中の移動を含む通常の入力を全て抑止します。
 * このアドオンは師走冬也様制作の「ReplaceCodesAddon」によるコード置換を前提としています。
 * 
 * @param moveStopSwitchNo
 * @text 移動停止スイッチNo
 * @desc 移動を停止するスイッチNo
 * @type integer
 * @default 2
 * 
 */

using UnityEngine;
using System;
using System.Reflection;
using RPGMaker.Codebase.Runtime.Map;
using RPGMaker.Codebase.CoreSystem.Helper;
using RPGMaker.Codebase.Runtime.Common;

namespace RPGMaker.Codebase.Addon
{
    public class RDMoveStop
    {
        private static int moveStopSwitchNo;

        public RDMoveStop(int moveStopSwitchNo)
        {
            RDMoveStop.moveStopSwitchNo = moveStopSwitchNo;
        }

        public static int GetMoveStopSwitchNo()
        {
            return moveStopSwitchNo - 1;
        }
    }
}