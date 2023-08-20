using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGMaker.Codebase.Runtime.Battle.Objects;

public class EnemyHPGage : MonoBehaviour
{
    public GameEnemy enemy;
    [SerializeField] Transform maskHp;
    [SerializeField] Transform textObj;
    [SerializeField] Transform hpBar;
    Text textHp;
    int tmpHp = -1;

    RectTransform maskHpRect;
    RectTransform hpBarRect;

    float targetMaskX = -1;
    float nowMaskX;

    private void Start() {
        textHp = textObj.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null) return;
        if (targetMaskX == -1)
        {
            maskHpRect = maskHp.GetComponent<RectTransform>();
            hpBarRect = hpBar.GetComponent<RectTransform>();
            float hpRate = 1.0f * enemy.Hp / enemy.Mhp;
            targetMaskX = 210 * hpRate;
            maskHpRect.sizeDelta = new Vector2(targetMaskX, 20);
            hpBarRect.anchoredPosition = new Vector3(GetHpBarHose(targetMaskX), 0, 0);
        }
        int nowHp = enemy.Hp;
        if (nowHp != tmpHp)
        {
            tmpHp = nowHp;
            textHp.text = tmpHp.ToString();
            float hpRate = 1.0f * enemy.Hp / enemy.Mhp;
            targetMaskX = 210 * hpRate;
            nowMaskX = maskHpRect.sizeDelta.x;
        }
        if (targetMaskX != maskHpRect.sizeDelta.x)
        {
            float sabun = targetMaskX - maskHpRect.sizeDelta.x;
            if (sabun < 1 && sabun > -1)
            {
                nowMaskX = targetMaskX;
            } else
            {
                if (nowMaskX < targetMaskX)
                {
                    nowMaskX += 1;
                } else
                {
                    nowMaskX -= 1;
                }
            }
            maskHpRect.sizeDelta = new Vector2(nowMaskX, 20);
            hpBarRect.anchoredPosition = new Vector3(GetHpBarHose(nowMaskX), 0, 0);
        }
    }
    private float GetHpBarHose (float maskX) {
        return (1.0f - (maskX / 210)) * 210 / 2;
    }
}
