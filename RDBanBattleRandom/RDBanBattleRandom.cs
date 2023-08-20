/*:
 * @addondesc Ban Battle Random
 * @author Kakudo Yuie (Radian)
 * @help Ban selected random effect during battle.
 * 
 * @param fixEnemyActPattern
 * @text fixEnemyActPattern
 * @desc Fix the order of enemy battle actions in the registered order
 * @type boolean
 * @default true
 * 
 * @param banRandomOnActTarget
 * @text banRandomOnActTarget
 * @desc Fix the target of the enemy's battle actions to the target specified in the memo field of the skill.
 * @type boolean
 * @default true
 * 
 * @param fixInitialTp
 * @text fixInitialTp
 * @desc Fix the initial TP at the start of battle.
 * @type boolean
 * @default true
 * 
 * @param initialTp
 * @text initialTp
 * @desc Initial TP (only fixing the initial TP).
 * @type integer
 * @default 0
 * 
 * @param forceEscapeSuccess
 * @text forceEscapeSuccess
 * @desc Force battle escapes are successful regardless of agility
 * @type boolean
 * @default true
 * 
 * @param banRandomOnActOrder
 * @text banRandomOnActOrder
 * @desc Remove randomness from the battle action order during battle. It can be determined only by agility.
 * @type boolean
 * @default true
 * 
 * @param banSurprisePreemptive
 * @text banSurprisePreemptive
 * @desc Prevent Surprise and Preemptive at the start of battle.
 * @type boolean
 * @default true
 * 
 * @param target1stIdxIfConfuse
 * @text target1stIdxIfConfuse
 * @desc The attack target at the time of confusion / fascination to the first index.
 * @type boolean
 * @default true
 * 
 * @param fixDamageRecoverTp
 * @text fixDamageRecoverTp
 * @desc Fix recover TP when actor get damage.
 * @type boolean
 * @default true
 * 
 * @param damageRecoverTp
 * @text damageRecoverTp
 * @desc damage recover TP (only fixDamageRecoverTp is true).
 * @type integer
 * @default 0
 * 
 * @param turnRecoverTp
 * @text turnRecoverTp
 * @desc recover TP at end of turn.
 * @type integer
 * @default 10
 * 
 */

/*:ja
 * @addondesc バトルのランダム要素消去
 * @author 角度ゆいえ（Radian）
 * @help 指定したバトル中のランダム要素を消去します。
 * 
 * @param fixEnemyActPattern
 * @text 敵アクションパターン固定
 * @desc 敵の使用スキルの順番を登録された順番に固定する
 * @type boolean
 * @default true
 * 
 * @param banRandomOnActTarget
 * @text 敵アクションの対象を固定
 * @desc 敵の使用スキルの対象をスキルのメモ欄で指定した対象に固定する(<RD_TARGETORDER_TYPE:[indexfirst/indexlast/hphigh/hplow]>)。※<RD_BAN_BATTLE_RANDOM_PROTECTION_STATE>をメモに記載したステートを持っている対象を優先する。
 * @type boolean
 * @default true
 * 
 * @param fixInitialTp
 * @text 初期TP固定
 * @desc 戦闘開始時の初期TPを固定する
 * @type boolean
 * @default true
 * 
 * @param initialTp
 * @text 初期TP
 * @desc 初期TP(初期TPを固定している時のみ有効)
 * @type integer
 * @default 0
 * 
 * @param forceEscapeSuccess
 * @text 逃走絶対成功
 * @desc 敏捷に関わらず逃走が必ず成功するようにする
 * @type boolean
 * @default true
 * 
 * @param banRandomOnActOrder
 * @text 行動順のランダム性排除
 * @desc 戦闘時の行動順からランダム性を無くし敏捷のみで判定されるようにする
 * @type boolean
 * @default true
 * 
 * @param banSurprisePreemptive
 * @text 不意打ち・先制攻撃禁止
 * @desc 戦闘開始時に不意打ちも先制攻撃も起こらないようにする
 * @type boolean
 * @default true
 * 
 * @param target1stIdxIfConfuse
 * @text 混乱・魅了時の攻撃対象を先頭に固定
 * @desc 混乱・魅了時の攻撃対象は必ず先頭とする。<RD_BAN_BATTLE_RANDOM_PROTECTION_STATE>をメモに記載したステートを持っている対象を優先する。
 * @type boolean
 * @default true
 * 
 * @param fixDamageRecoverTp
 * @text 被ダメージ時の回復TPを固定
 * @desc 被ダメージ時のTP回復量を一定値にする
 * @type boolean
 * @default true
 * 
 * @param damageRecoverTp
 * @text 被ダメージ時のTP回復量
 * @desc 被ダメージ時のTP回復量 (被ダメージ時の回復TPを固定している時のみ).
 * @type integer
 * @default 0
 * 
 * @param turnRecoverTp
 * @text ターン終了時のTP回復量
 * @desc ターン終了時のTP回復量
 * @type integer
 * @default 10
 */

using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Enemy;
using RPGMaker.Codebase.Runtime.Battle;
using RPGMaker.Codebase.Runtime.Battle.Objects;
using RPGMaker.Codebase.Runtime.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.State;

namespace RPGMaker.Codebase.Addon
{
    public class RDBanBattleRandom
    {
        private static bool fixEnemyActPattern;
        private static bool banRandomOnActTarget;
        private static bool fixInitialTp;
        private static int initialTp;
        private static bool forceEscapeSuccess;
        private static bool banRandomOnActOrder;
        private static bool banSurprisePreemptive;
        private static bool target1stIdxIfConfuse;
        private static bool fixDamageRecoverTp;
        private static int damageRecoverTp;
        private static int turnRecoverTp;

        public RDBanBattleRandom(bool fixEnemyActPattern, bool banRandomOnActTarget, bool fixInitialTp, int initialTp, bool forceEscapeSuccess,
            bool banRandomOnActOrder, bool banSurprisePreemptive, bool target1stIdxIfConfuse, bool fixDamageRecoverTp, int damageRecoverTp, int turnRecoverTp) {
            RDBanBattleRandom.fixEnemyActPattern = fixEnemyActPattern;
            RDBanBattleRandom.banRandomOnActTarget = banRandomOnActTarget;
            RDBanBattleRandom.fixInitialTp = fixInitialTp;
            RDBanBattleRandom.initialTp = initialTp;
            RDBanBattleRandom.forceEscapeSuccess = forceEscapeSuccess;
            RDBanBattleRandom.banRandomOnActOrder = banRandomOnActOrder;
            RDBanBattleRandom.banSurprisePreemptive = banSurprisePreemptive;
            RDBanBattleRandom.target1stIdxIfConfuse = target1stIdxIfConfuse;
            RDBanBattleRandom.fixDamageRecoverTp = fixDamageRecoverTp;
            RDBanBattleRandom.damageRecoverTp = damageRecoverTp;
            RDBanBattleRandom.turnRecoverTp = turnRecoverTp;

        }

        public static bool IsFixEnemyActionPattern()
        {
            return fixEnemyActPattern;
        }
        public static bool IsBanRandomOnActionTarget()
        {
            return banRandomOnActTarget;
        }
        public static bool IsFixInitialTp()
        {
            return fixInitialTp;
        }
        public static int GetInitialTp()
        {
            return initialTp;
        }
        public static bool IsEscapeMustSuccess()
        {
            return forceEscapeSuccess;
        }
        public static bool IsBanRandomOnActionOrder()
        {
            return banRandomOnActOrder;
        }
        public static bool IsBanSurpriseAndPreemptive()
        {
            return banSurprisePreemptive;
        }
        public static bool IsTargetFiestIndexWhenConfuse()
        {
            return target1stIdxIfConfuse;
        }
        public static bool IsFixDamageRecoverTp() {
            return fixDamageRecoverTp;
        }
        public static int GetDamageRecoverTp() {
            return damageRecoverTp;
        }
        public static int GetTurnRecoverTp() {
            return turnRecoverTp;
        }
    }
    public static class RDBanBattleRandomInternal
    {
        public static SkillTargetOrderType GetSkillTargetOrderType(string memo)
        {
            Regex reg = new Regex("<RD_TARGETORDER_TYPE:(?<note>.*?)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(memo);
            if (!m.Success) return SkillTargetOrderType.indexfirst;
            string note = m.Groups["note"].Value;
            return GetEnumFromString<SkillTargetOrderType>(note);
        }

        public static bool IsProtectionState(StateDataModel state) {
            Regex reg = new Regex("<RD_BAN_BATTLE_RANDOM_PROTECTION_STATE>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(state.note);
            if (!m.Success) return false;
            return true;
        }

        public static bool IsProtectionAllState(StateDataModel state) {
            Regex reg = new Regex("<RD_BAN_BATTLE_RANDOM_PROTECTION_ALL_STATE>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(state.note);
            if (!m.Success) return false;
            return true;
        }
        private static T GetEnumFromString<T>(string targetTypeStr)
        {
            return (T)Enum.Parse(typeof(T), targetTypeStr);
        }
    }
    public enum SkillTargetOrderType
    {
        indexfirst,
        indexlast,
        hphigh,
        hplow,
    }
}
