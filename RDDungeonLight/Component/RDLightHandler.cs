using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGMaker.Codebase.Runtime.Map.Component.Map;
using RPGMaker.Codebase.Runtime.Map;
using RPGMaker.Codebase.Runtime.Map.Component.Character;
using System.Linq;
using System.Text.RegularExpressions;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Runtime;
using RPGMaker.Codebase.CoreSystem.Helper;

public class RDLightHandler : MonoBehaviour
{
    [SerializeField] Transform spotLightOrigin;
    public List<RDLight> lights;
    GameObject root;
    EventOnMap friend;
    ActorOnMap actor;
    [SerializeField] Transform friendLightObject;
    [SerializeField] Transform actorLightObject;
    string tmpMapId = "";

    SpriteRenderer blackBackSprite;
    int duration;
    float difRed;
    float difGreen;
    float difBlue;

    public int defaultActorLightTraceMode;

    // Start is called before the first frame update
    void Start()
    {
        root = GameObject.Find("/Root");
        lights = new List<RDLight>();
        blackBackSprite = transform.Find("RD_BlackTop").GetComponent<SpriteRenderer>();

        actorLightObject.GetComponent<RDActorEye>().useMainCamera = defaultActorLightTraceMode != 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MapChangeInit();
        if (actor == null)
        {
            lights = new List<RDLight>();
            Transform children = root.GetComponentInChildren<Transform>();
            foreach (Transform child in children)
            {
                EventOnMap eventComponent = child.GetComponent<EventOnMap>();
                if (eventComponent != null)
                {
                    if (IsLight(eventComponent))
                    {
                        GameObject copiedLight = Instantiate(spotLightOrigin.gameObject);
                        copiedLight.transform.SetParent(transform);
                        copiedLight.transform.position = new Vector2(eventComponent.transform.position.x + 0.5f, eventComponent.transform.position.y + 0.5f);
                        RDLight rdLight = new RDLight(eventComponent, copiedLight);
                        lights.Add(rdLight);
                        if (rdLight.isActive())
                        {
                            copiedLight.gameObject.SetActive(true);
                        }
                        copiedLight.transform.localScale = new Vector3(rdLight.radius, rdLight.radius, 1.0f);
                    }
                    else if (RDFriendUtil.IsFriend(eventComponent))
                    {
                        friend = eventComponent;
                    }
                    
                } else
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
            if (actor != null)
            {
                if (friendLightObject.gameObject.activeSelf && friend == null)
                {
                    friendLightObject.gameObject.SetActive(false);
                }
                if (!friendLightObject.gameObject.activeSelf && friend != null)
                {
                    friendLightObject.gameObject.SetActive(true);
                }
            }
        }
        if (actor == null) return;
        lights.ForEach(l =>
        {
            if (l.OffSwitchNo != 0 || l.OnSwitchNo != 0)
            {
                if (l.lightObject.activeSelf && !l.isActive())
                {
                    l.lightObject.SetActive(false);
                }
                if (!l.lightObject.activeSelf && l.isActive())
                {
                    l.lightObject.SetActive(true);
                }
            }
            if (l.radius != l.tmpRadius)
            {
                l.lightObject.transform.localScale = new Vector3(l.radius, l.radius, 1.0f);
                l.tmpRadius = l.radius;
            }
            if (l.lightEvent.transform.position.x != l.lightObject.transform.position.x || l.lightEvent.transform.position.y != l.lightObject.transform.position.y)
            {
                l.lightObject.transform.position = new Vector2(l.lightEvent.transform.position.x + 0.5f, l.lightEvent.transform.position.y + 0.5f);
            }
        });
        if (duration > 0)
        {
            duration--;
            blackBackSprite.color = new Color(blackBackSprite.color.r + difRed, blackBackSprite.color.g + difGreen, blackBackSprite.color.b + difBlue);
        }
    }

    public static bool IsLight(EventOnMap eventComponent) {
        if (GetLightEventNotes(eventComponent) == null) return false;
        return true;
    }
    public static string[] GetLightEventNotes(EventOnMap eventComponent) {
        Regex reg = new Regex("<RD_MAP_LIT:(?<note>.*?)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Match m = reg.Match(eventComponent.MapDataModelEvent.note);
        if (!m.Success) return null;
        string note = m.Groups["note"].Value;
        string[] notes = note.Split(',');
        return notes;
    }
    private bool isTopActor(Transform actorObject) {
        if (actorObject.GetComponent<ActorOnMap>() == null) return false;
        if (actorObject.Find("Main Camera") == null) return false;
        return true;
    }
    private void MapChangeInit() {
        if (MapManager.CurrentMapDataModel == null) return;
        if (MapManager.CurrentMapDataModel.id == tmpMapId) return;
        tmpMapId = MapManager.CurrentMapDataModel.id;
        actor = null;
        DestroyAllLights();
    }

    public void DestroyAllLights() {
        lights.ForEach(l =>
        {
            Destroy(l.lightObject);
        });
        lights = new List<RDLight>();
        actor = null;
    }

    public void SetTargetColor(Color targetColor, int duration) {
        if (duration == 0) duration = 1;
        Color nowColor = blackBackSprite.color;
        if (nowColor == targetColor) return;
        difRed = (targetColor.r - nowColor.r) / duration;
        difGreen = (targetColor.g - nowColor.g) / duration;
        difBlue = (targetColor.b - nowColor.b) / duration;
        this.duration = duration;
    }

    public void SetActorTraceMode(int actorTraceMode) {
        var actorLight = actorLightObject.GetComponent<RDActorEye>();
        if (actorTraceMode == 0)
        {
            actorLight.useMainCamera = false;
        }
        else
        {
            actorLight.useMainCamera = true;
        }
    }

    public void SetActorLightRadius(float actorLightRadius, bool immediet) {
        var actorLight = actorLightObject.GetComponent<RDActorEye>();
        if (immediet)
        {
            actorLight.radiusImmediet = actorLightRadius;
        } else
        {
            actorLight.changeRadius(actorLightRadius);
        }
    }
}
