using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGMaker.Codebase.Runtime.Battle.Sprites;
using System;
using UnityEngine.SceneManagement;

public class EnemyBreath : MonoBehaviour
{
    GameObject sideView;
    List<EnemyBreathModel> spEnemies = new List<EnemyBreathModel>();

    int imageLoadWaitCount = 10;

    void Start()
    {
        var scene = SceneManager.GetSceneByName("Battle");
        SceneManager.MoveGameObjectToScene(gameObject, scene);
        sideView = GameObject.Find("/Canvas/SpriteSetBattle/SideView");
    }

    void FixedUpdate()
    {
        if (spEnemies.Count <= 0)
        {
            Transform itemLayer1 = sideView.GetComponentInChildren<Transform>();
            foreach (Transform layer1Items in itemLayer1)
            {
                Transform itemLayer2 = layer1Items.GetComponentInChildren<Transform>();
                foreach (Transform layer2Items in itemLayer2)
                {
                    Transform eneSpLayer = layer2Items.GetComponentInChildren<Transform>();
                    foreach (Transform eneSp in eneSpLayer)
                    {
                        SpriteEnemy eneSpComponent = eneSp.GetComponent<SpriteEnemy>();
                        if (eneSpComponent != null)
                        {
                            if (imageLoadWaitCount > 0)
                            {
                                imageLoadWaitCount--;
                                return;
                            }
                            EnemyBreathModel enemyBreath = new EnemyBreathModel(eneSpComponent);
                            spEnemies.Add(enemyBreath);
                        }
                    }
                }
            }
        }
        spEnemies.ForEach(spEnemy =>
        {
            spEnemy.updateBreath();
        });
    }

    private class EnemyBreathModel
    {
        int breathCount;
        readonly int MAX_BREATH_COUNT = 120;
        readonly double MAX_BREATH_DEPTH = 0.02;

        RectTransform parentRect;
        double baseY;
        double baseHeight;

        public EnemyBreathModel(SpriteEnemy spEnemy) {
            breathCount = UnityEngine.Random.Range(0, MAX_BREATH_COUNT);

            var parentObj = spEnemy.transform.parent;
            baseY = parentObj.localPosition.y;
            parentRect = parentObj.GetComponent<RectTransform>();
            baseHeight = parentRect.sizeDelta.y;
        }
        public void updateBreath() {
            double scale = Math.Sin(Math.PI / 180 * breathCount * 3) * MAX_BREATH_DEPTH;
            double nextY = scale * baseHeight / 2;
            parentRect.anchoredPosition = new Vector2(parentRect.anchoredPosition.x, (float) (baseY + nextY));
            parentRect.localScale = new Vector3(parentRect.localScale.x, (float) scale + 1, parentRect.localScale.z);

            breathCount++;
            if (breathCount >= 120)
            {
                breathCount = 0;
            }
        }
    }
}
