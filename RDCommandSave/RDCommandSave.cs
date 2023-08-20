/*:
 * @addondesc Command Save
 * @author Kakudo Yuie (Radian)
 * @help Save command on a flow
 * 
 */

/*:ja
 * @addondesc コマンドセーブ
 * @author 角度ゆいえ（Radian）
 * @help フローコマンド上でセーブができます
 * 
 * @command Save
 *      @text セーブ
 *      @desc 指定した番号にセーブします
 * 
 *  @arg number
 *      @text 番号
 *      @desc セーブする番号
 *      @type integer
 *      @default 1
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
using RPGMaker.Codebase.CoreSystem.Service.RuntimeDataManagement;

namespace RPGMaker.Codebase.Addon
{
    public class RDCommandSave
    {
        public RDCommandSave()
        {
        }

        public void Save(int number) {
            var runtimeDataManagementService = new RuntimeDataManagementService();
            var data = DataManager.Self().GetRuntimeSaveDataModel();
            runtimeDataManagementService.SaveSaveData(data, number);
        }
    }
}

