/*:
 * @addondesc Global Volume Add-on
 * @author Kakudo Yuie (Radian)
 * @help Change initial volume. And set the volume change tick 20 to 10.
 * 
 * @param rate
 * @text global volume rate
 * @desc global volume rate(%). Specify in multiples of 20.
 * @type integer
 * @default 20
 * @min 0
 * @max 200
 */

/*:ja
 * @addondesc 全体音量調整
 * @author 角度ゆいえ（Radian）
 * @help 初期の音量を調整します。ついでに音量調節の刻みを10にします。
 * このアドオンは師走冬也様制作の「ReplaceCodesAddon」によるコード置換を前提としています。
 * 
 * @param rate
 * @text 初期音量
 * @desc 初期音量（％）。20の倍数で指定してください。
 * @type integer
 * @default 20
 * @min 0
 * @max 100
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
    public class RDGlobalVolume
    {
        private static int glovalVolumeRate = 0;
        private static bool glovalVolumeFlag = false;

        public RDGlobalVolume(int rate)
        {
            glovalVolumeRate = rate;
            glovalVolumeFlag = true;
        }

        public static int GetGlovalVolumeRate()
        {
            return glovalVolumeRate;
        }
        public static bool IsGlovalVolumeOn() {
            return glovalVolumeFlag;
        }
    }
}