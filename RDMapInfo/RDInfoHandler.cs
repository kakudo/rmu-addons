using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGMaker.Codebase.Runtime.Map.Component.Character;
using System.Text.RegularExpressions;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Runtime;
using RPGMaker.Codebase.CoreSystem.Helper;
using UnityEngine.UI;
using RPGMaker.Codebase.CoreSystem.Knowledge.Enum;
using RPGMaker.Codebase.Runtime.Battle;
using RPGMaker.Codebase.Runtime.Map;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Enemy;
using System.Linq;
using RPGMaker.Codebase.Addon;
using RPGMaker.Codebase.Runtime.Common;
public class RDInfoHandler : MonoBehaviour
{    
    public List<RDInfo> infos;

    GameObject root;
    ActorOnMap actor;
    [SerializeField] Transform gameInfo;
    Text gameInfoText;

    string tmpContent;
    float alpha = 0f;

    readonly string CHANGE_LINE_REPLACED = "\n";
    readonly string CHANGE_LINE = "\\n";

    string tmpMapId = "";

    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        root = GameObject.Find("/Root");
        infos = new List<RDInfo>();
        gameInfoText = gameInfo.GetComponent<Text>();
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        MapChangeInit();
        if (gameInfo.gameObject.activeSelf)
        {
            if (BattleManager.IsBattle)
            {
                gameInfo.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!BattleManager.IsBattle)
            {
                gameInfo.gameObject.SetActive(true);
            }
        }
        if (BattleManager.IsBattle) return;

        if (actor == null)
        {
            infos = new List<RDInfo>();
            Transform children = root.GetComponentInChildren<Transform>();
            foreach (Transform child in children)
            {
                EventOnMap eventComponent = child.GetComponent<EventOnMap>();
                if (eventComponent != null)
                {
                    if (RDInfo.IsInfo(eventComponent))
                    {
                        RDInfo info = new RDInfo(eventComponent);
                        infos.Add(info);
                    }
                }
                else
                {
                    ActorOnMap actorComponent = child.GetComponent<ActorOnMap>();
                    if (actorComponent != null)
                    {
                        if (isTopActor(child))
                        {
                            actor = actorComponent;
                        }
                    }
                }
            }
        }
        if (actor == null) return;
        string content = null;

        if (active)
        {
            infos.ForEach(info =>
            {
                if (content != null) return;
                if (!info.isActive()) return;
                EventOnMap targetComponent = info.component;
                if (targetComponent != null)
                {
                    Vector2 actorPos = actor.GetCurrentPositionOnTile();
                    CharacterMoveDirectionEnum actorDir = actor.GetCurrentDirection();
                    Vector2 actorPosForward = actorPos + GetVectorOfDir(actorDir);
                    List<Vector2> tarPos = new List<Vector2>();
                    if (info.infoPlace == RDInfoPlaceEnum.forward || info.infoPlace == RDInfoPlaceEnum.onforward)
                    {
                        tarPos.Add(actorPosForward);
                    }
                    if (info.infoPlace == RDInfoPlaceEnum.up && actorDir == CharacterMoveDirectionEnum.Down)
                    {
                        tarPos.Add(actorPosForward);
                    }
                    if (info.infoPlace == RDInfoPlaceEnum.down && actorDir == CharacterMoveDirectionEnum.Up)
                    {
                        tarPos.Add(actorPosForward);
                    }
                    if (info.infoPlace == RDInfoPlaceEnum.right && actorDir == CharacterMoveDirectionEnum.Left)
                    {
                        tarPos.Add(actorPosForward);
                    }
                    if (info.infoPlace == RDInfoPlaceEnum.left && actorDir == CharacterMoveDirectionEnum.Right)
                    {
                        tarPos.Add(actorPosForward);
                    }
                    if (info.infoPlace == RDInfoPlaceEnum.on || info.infoPlace == RDInfoPlaceEnum.onforward)
                    {
                        tarPos.Add(actorPos);
                    }
                    tarPos.ForEach(tp =>
                    {
                        if (content != null) return;
                        if (tp == targetComponent.GetCurrentPositionOnTile())
                        {
                            content = info.content.Replace(CHANGE_LINE, CHANGE_LINE_REPLACED);
                            active = true;
                        }
                    });
                }
            });
        }

        if (content != null && tmpContent != content)
        {
            gameInfoText.text = content;
            tmpContent = content;
            alpha = 0;
            gameInfoText.color = new Color(gameInfoText.color.r, gameInfoText.color.g, gameInfoText.color.b, alpha);
        }
        else if (content == null && tmpContent != null)
        {
            tmpContent = null;
        }
        if (content != null && alpha <= 1)
        {
            alpha += 0.1f;
            gameInfoText.color = new Color(gameInfoText.color.r, gameInfoText.color.g, gameInfoText.color.b, alpha);
        }
        if (content == null && alpha > 0)
        {
            alpha -= 0.1f;
            gameInfoText.color = new Color(gameInfoText.color.r, gameInfoText.color.g, gameInfoText.color.b, alpha);
        }
    }

    private Vector2 GetVectorOfDir(CharacterMoveDirectionEnum dir)
    {
        if (dir == CharacterMoveDirectionEnum.Down)
        {
            return Vector2.down;
        }
        else if (dir == CharacterMoveDirectionEnum.Up)
        {
            return Vector2.up;
        }
        else if (dir == CharacterMoveDirectionEnum.Right)
        {
            return Vector2.right;
        }
        return Vector2.left;
    }
    private bool isTopActor(Transform actorObject)
    {
        if (actorObject.GetComponent<ActorOnMap>() == null) return false;
        if (actorObject.Find("Main Camera") == null) return false;
        return true;
    }
    private void MapChangeInit()
    {
        if (MapManager.CurrentMapDataModel == null) return;
        if (MapManager.CurrentMapDataModel.id == tmpMapId) return;
        tmpMapId = MapManager.CurrentMapDataModel.id;
        actor = null;
    }

}