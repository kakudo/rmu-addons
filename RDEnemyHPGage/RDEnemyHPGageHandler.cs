using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGMaker.Codebase.Runtime.Battle.Sprites;
using System.Reflection;
using System;
using RPGMaker.Codebase.Runtime.Battle.Objects;
using UnityEngine.SceneManagement;

public class RDEnemyHPGageHandler : MonoBehaviour
{
    GameObject sideView;
    [SerializeField] Transform rDEnemyHPGageOrigin;
    List<SpriteEnemy> spEnemies;
    List<EnemyHPGage> hpGages;

    int imageLoadWaitCount = 10;

    // Start is called before the first frame update
    void Start()
    {
        var scene = SceneManager.GetSceneByName("Battle");
        SceneManager.MoveGameObjectToScene(gameObject, scene);

        spEnemies = new List<SpriteEnemy>();
        hpGages = new List<EnemyHPGage>();
        sideView = GameObject.Find("/Canvas/SpriteSetBattle/SideView");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (imageLoadWaitCount > 0)
        {
            imageLoadWaitCount--;
            return;
        }
        if (spEnemies.Count <= 0)
        {
            Transform itemLayer1 = sideView.GetComponentInChildren<Transform>();
            foreach(Transform layer1Items in itemLayer1) {
                Transform itemLayer2 = layer1Items.GetComponentInChildren<Transform>();
                foreach (Transform layer2Items in itemLayer2)
                {
                    Transform eneSpLayer = layer2Items.GetComponentInChildren<Transform>();
                    foreach (Transform eneSp in eneSpLayer)
                    {
                        SpriteEnemy eneSpComponent = eneSp.GetComponent<SpriteEnemy>();
                        if (eneSpComponent != null)
                        {
                            spEnemies.Add(eneSpComponent);
                            GameObject rDEnemyHPGageObj = Instantiate(rDEnemyHPGageOrigin.gameObject);
                            rDEnemyHPGageObj.transform.parent = eneSp.parent;
                            rDEnemyHPGageObj.transform.localScale = new Vector3(1, 1, 1);
                            float enemyWidth = layer2Items.transform.GetComponent<RectTransform>().sizeDelta.x;
                            float enemyHeight = layer2Items.transform.GetComponent<RectTransform>().sizeDelta.y;
                            var locateX = -1 * enemyWidth + (enemyWidth - 197f) / 2f - 70;
                            rDEnemyHPGageObj.transform.localPosition = new Vector3(locateX, -1 * enemyHeight * 0.5f + 72, 0);
                            rDEnemyHPGageObj.SetActive(true);
                            EnemyHPGage gage = rDEnemyHPGageObj.GetComponent<EnemyHPGage>();

                            gage.enemy = GetEnemy(eneSpComponent);
                            hpGages.Add(gage);
                        }
                    }
                }
            }
        }
        spEnemies.ForEach(spEne =>
        {
            GameEnemy enemy = GetEnemy(spEne);
            EnemyHPGage gage = GetGageObjFromEnemy(enemy);
            if (gage.gameObject.activeSelf && IsErase(enemy))
            {
                gage.gameObject.SetActive(false);
            }
            if (!gage.gameObject.activeSelf && !IsErase(enemy))
            {
                gage.gameObject.SetActive(true);
            }
        });
    }
    private GameEnemy GetEnemy(SpriteEnemy eneSpComponent) {
        Type eMapClassType = eneSpComponent.GetType();
        FieldInfo _privateEnemy = eMapClassType.GetField("_enemy", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

        return (GameEnemy) _privateEnemy.GetValue(eneSpComponent);
    }
    private bool IsErase(GameEnemy enemy) {
        if (!enemy.IsAlive()) return true;
        if (enemy.IsHidden()) return true;
        return false;
    }
    private EnemyHPGage GetGageObjFromEnemy(GameEnemy enemy) {
        return hpGages.Find(g => g.enemy == enemy);
    }
}
