using System;
using System.Collections.Generic;
using UnityEngine;
using RPGMaker.Codebase.Runtime.Map.Component.Character;
using RPGMaker.Codebase.Runtime.Map.Component.Map;
using RPGMaker.Codebase.Runtime.Map;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Runtime;
using RPGMaker.Codebase.CoreSystem.Helper;
using RPGMaker.Codebase.Runtime.Common;
public class RDLight
{
    public GameObject lightEvent;
    public EventOnMap eventComponent;

    public GameObject lightObject;

    public int radius;
    public int tmpRadius;
    public int OnSwitchNo = 0;
    public int OffSwitchNo = 0;

    public RDLight (EventOnMap lightEventComponent, GameObject lightObject) {
        eventComponent = lightEventComponent;
        lightEvent = lightEventComponent.gameObject;
        this.lightObject = lightObject;
        string[] notes = RDLightHandler.GetLightEventNotes(lightEventComponent);
        radius = int.Parse(notes[0]);
        tmpRadius = radius;
        if (notes.Length >= 2 && notes[1] != "" && notes[1] != "0")
        {
            OnSwitchNo = int.Parse(notes[1]);
        }
        if (notes.Length >= 3 && notes[2] != "" && notes[2] != "0")
        {
            OffSwitchNo = int.Parse(notes[2]);
        }
    }
    public bool isActive() {
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
