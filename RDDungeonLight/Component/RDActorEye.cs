using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGMaker.Codebase.Runtime.Map.Component.Character;
using RPGMaker.Codebase.CoreSystem.Knowledge.Enum;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Event;
using RPGMaker.Codebase.Runtime.Map;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Map;
using RPGMaker.Codebase.Runtime.Map.Component.Map;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Runtime;

public class RDActorEye : MonoBehaviour
{
    ActorOnMap actor;
    Vector2 actorEyeHosei = new Vector2(1, 1);
    Vector2 acterPos;
    Vector2 acterPosTmp;
    Vector2 actorEyeTarget;
    float actorAndEyeRate = 1.0f / 0.5f;
    GameObject actorEye;
    Vector2 actorEyeLine;
    Vector2 actorEyeLineTmp;
    GameObject root;
    [SerializeField] public float eyeDistance;
    float tmpEyeDistance;
    public static float INIT_RADIUS = 2.5f;
    public static float LAMP_RADIUS = 4f;
    public float radius = INIT_RADIUS;
    public float radiusImmediet = INIT_RADIUS;

    Transform mainCamera;
    public bool useMainCamera = true;

    string tmpMapId = "";

    // Start is called before the first frame update
    void Start()
    {
        root = GameObject.Find("/Root");
        actorEye = this.gameObject;
        actorEyeTarget = actorEyeHosei;
        actorEyeTarget += acterPos * actorAndEyeRate;
        actorEyeTarget += actorEyeLine * eyeDistance;
        acterPosTmp = actorEyeTarget;
        actorEye.transform.position = actorEyeTarget;
        tmpEyeDistance = eyeDistance;
        actorEye.transform.localScale = new Vector3(radius, radius, 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MapChangeInit();
        if (actor == null)
        {
            Transform children = root.GetComponentInChildren<Transform>();
            foreach (Transform child in children)
            {
                ActorOnMap actorComponent = child.GetComponent<ActorOnMap>();
                if (actorComponent != null)
                {
                    if (isTopActor(child))
                    {
                        mainCamera = child.Find("Main Camera");
                        actor = actorComponent;
                        if (useMainCamera)
                        {
                            acterPos = new Vector2(mainCamera.position.x - 0.5f, mainCamera.position.y - 0.5f);
                        } else
                        {
                            acterPos = new Vector2(mainCamera.position.x - 0.5f, mainCamera.position.y - 0.5f);
                            //acterPos = new Vector2(child.position.x, child.position.y);
                        }
                        actorEyeLine = GetVectorOfDir(actorComponent.GetCurrentDirection());
                        actorEyeLineTmp = actorEyeLine;
                        break;
                    }
                }
            }
        }
        if (actor == null) return;
        if (useMainCamera)
        {
            acterPos = new Vector2(mainCamera.position.x - 0.5f, mainCamera.position.y - 0.5f);
        }
        else
        {
            acterPos = new Vector2(actor.transform.position.x, actor.transform.position.y);
        }

        CharacterMoveDirectionEnum dir = actor.GetCurrentDirection();
        actorEyeLine = GetVectorOfDir(dir);

        if (acterPosTmp != acterPos || actorEyeLineTmp != actorEyeLine || eyeDistance != tmpEyeDistance)
        {
            changeEyeTarget();
            tmpEyeDistance = eyeDistance;
        }
        if (radiusImmediet != radius)
        {
            actorEye.transform.localScale = new Vector3(radiusImmediet, radiusImmediet, 1.0f);
            radius = radiusImmediet;
        }
        if (actorEye.transform.localScale.x > radius)
        {
            actorEye.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            if (actorEye.transform.localScale.x < radius)
            {
                actorEye.transform.localScale = new Vector3(radius, radius, 1.0f);
            }
        }
        
        if (actorEye.transform.localScale.x < radius)
        {
            actorEye.transform.localScale += new Vector3(0.1f, 0.1f, 0);
            if (actorEye.transform.localScale.x > radius)
            {
                actorEye.transform.localScale = new Vector3(radius, radius, 1.0f);
            }
        }
        Vector2 nowEyePosition = new Vector2(actorEye.transform.position.x, actorEye.transform.position.y);
        float actorEyeDistance = Vector2.Distance(actorEyeTarget, nowEyePosition * actorAndEyeRate);
        if (actorEyeDistance > 0.01)
        {
            var eyeChangeSpeed = actorEyeDistance > 10 ? actorEyeDistance / 2 : actorEyeDistance / 10;
            Vector2 eyeMoveDir = actorEyeTarget - nowEyePosition;
            actorEye.transform.position = Vector2.MoveTowards(actorEye.transform.position, eyeMoveDir, eyeChangeSpeed);
        }
    }
    public void changeEyeTarget() {
        actorEyeTarget = actorEyeHosei;
        actorEyeTarget += acterPos * actorAndEyeRate;
        actorEyeTarget += actorEyeLine * eyeDistance;
        acterPosTmp = acterPos;
        actorEyeLineTmp = actorEyeLine;
    }
    public void changeRadius(float radius) {
        this.radius = radius;
        radiusImmediet = radius;
    }
    private Vector2 GetVectorOfDir(CharacterMoveDirectionEnum dir) {
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
    }
}
