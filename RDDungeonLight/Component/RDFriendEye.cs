using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGMaker.Codebase.Runtime.Map.Component.Character;
using RPGMaker.Codebase.CoreSystem.Knowledge.Enum;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Event;
using RPGMaker.Codebase.Runtime.Map;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Map;
using RPGMaker.Codebase.Runtime.Map.Component.Map;

public class RDFriendEye : MonoBehaviour
{
    ActorOnMap actor;
    EventOnMap friend;
    Vector2 friendEyeHosei = new Vector2(1, 1);
    Vector2 friendPos;
    Vector2 friendPosTmp;
    Vector2 friendEyeTarget;
    float friendAndEyeRate = 1.0f / 0.5f;
    GameObject friendEye;
    Vector2 friendEyeLine;
    Vector2 friendEyeLineTmp;
    GameObject root;
    [SerializeField] public float eyeDistance;
    float tmpEyeDistance;
    public static float INIT_RADIUS = 2.5f;
    public static float LAMP_RADIUS = 4f;
    public float radius = INIT_RADIUS;
    public float radiusImmediet = INIT_RADIUS;

    string tmpMapId = "";

    // Start is called before the first frame update
    void Start() {
        root = GameObject.Find("/Root");
        friendEye = this.gameObject;
        friendEyeTarget = friendEyeHosei;
        friendEyeTarget += friendPos * friendAndEyeRate;
        friendEyeTarget += friendEyeLine * eyeDistance;
        friendPosTmp = friendEyeTarget;
        friendEye.transform.position = friendEyeTarget;
        tmpEyeDistance = eyeDistance;
        friendEye.transform.localScale = new Vector3(radius, radius, 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate() {
        MapChangeInit();
        if (actor == null)
        {
            Transform children = root.GetComponentInChildren<Transform>();
            foreach (Transform child in children)
            {
                EventOnMap friendComponent = child.GetComponent<EventOnMap>();
                if (friendComponent != null)
                {
                    if (RDFriendUtil.IsFriend(friendComponent))
                    {
                        friendPos = new Vector2(child.position.x, child.position.y);
                        friendEyeLine = GetVectorOfDir(friendComponent.GetCurrentDirection());
                        friendEyeLineTmp = friendEyeLine;
                        friend = friendComponent;
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
        if (friend == null) return;
        friendPos = new Vector2(friend.transform.position.x, friend.transform.position.y);
        CharacterMoveDirectionEnum dir = friend.GetCurrentDirection();
        friendEyeLine = GetVectorOfDir(dir);

        if (friendPosTmp != friendPos || friendEyeLineTmp != friendEyeLine || eyeDistance != tmpEyeDistance)
        {
            friendEyeTarget = friendEyeHosei;
            friendEyeTarget += friendPos * friendAndEyeRate;
            friendEyeTarget += friendEyeLine * eyeDistance;
            friendPosTmp = friendPos;
            friendEyeLineTmp = friendEyeLine;
            tmpEyeDistance = eyeDistance;
        }
        if (radiusImmediet != radius)
        {
            friendEye.transform.localScale = new Vector3(radiusImmediet, radiusImmediet, 1.0f);
            radius = radiusImmediet;
        }
        if (friendEye.transform.localScale.x > radius)
        {
            friendEye.transform.localScale -= new Vector3(0.01f, 0.01f, 0);
        }
        if (friendEye.transform.localScale.x < radius)
        {
            friendEye.transform.localScale += new Vector3(0.01f, 0.01f, 0);
        }
        Vector2 nowEyePosition = new Vector2(friendEye.transform.position.x, friendEye.transform.position.y);
        float friendEyeDistance = Vector2.Distance(friendEyeTarget, nowEyePosition * friendAndEyeRate);
        if (friendEyeDistance > 0.01)
        {
            var eyeChangeSpeed = friendEyeDistance > 10 ? friendEyeDistance / 2 : friendEyeDistance / 10;
            Vector2 eyeMoveDir = friendEyeTarget - nowEyePosition;
            friendEye.transform.position = Vector2.MoveTowards(friendEye.transform.position, eyeMoveDir, eyeChangeSpeed);
        }
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

