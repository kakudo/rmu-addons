using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using RPGMaker.Codebase.Runtime.Map.Component.Character;

public static class RDFriendUtil
{
    public static bool IsFriend(EventOnMap eventComponent) {
        Regex reg = new Regex("<RD_FRIEND>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Match m = reg.Match(eventComponent.MapDataModelEvent.note);
        if (!m.Success) return false;
        return true;
    }
}
