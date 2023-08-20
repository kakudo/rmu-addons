using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGMaker.Codebase.Runtime.Battle;
using RPGMaker.Codebase.CoreSystem.Helper;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Runtime;
using RPGMaker.Codebase.Runtime.Common;
public class RDDungeonLightSwitch : MonoBehaviour
{
    [SerializeField] Transform lightHandler;

    public int enableSwitchNo = 0;
    private RuntimeSaveDataModel _runtimeSaveDataModel;

    private void Start() {
        _runtimeSaveDataModel = DataManager.Self().GetRuntimeSaveDataModel();
    }

    // Update is called once per frame
    void Update()
    {
        if ((enableSwitchNo <= 0 || !_runtimeSaveDataModel.switches.data[enableSwitchNo - 1]))
        {
            if (lightHandler.gameObject.activeSelf)
            {
                lightHandler.gameObject.SetActive(false);
            }
            return;
        }
        if (BattleManager.IsBattle && lightHandler.gameObject.activeSelf)
        {
            lightHandler.gameObject.SetActive(false);
        } else if (!BattleManager.IsBattle && !lightHandler.gameObject.activeSelf)
        {
            lightHandler.gameObject.SetActive(true);
        }
    }
}
