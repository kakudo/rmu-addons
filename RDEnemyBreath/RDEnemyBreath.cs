/*:
 *@addondesc Enemy Breath
 * @author Kakudo Yuie (Radian)
 *@help During the battle, the image of the enemy moves up and down like breathing.
 * 
 */

/*:ja
 * @addondesc 敵息遣いアドオン
 * @author 角度ゆいえ（Radian）
 * @help 戦闘中、敵の画像が息遣いするように上下に動くようになる
 * 
 */

using UnityEngine;
using RPGMaker.Codebase.CoreSystem.Helper;
using UnityEngine.SceneManagement;

namespace RPGMaker.Codebase.Addon
{
    public class RDEnemyBreath
    {
        private static readonly string PREFAB_PATH = "RDEnemyBreath/RDEnemyBreath";
        private static GameObject m;

        public RDEnemyBreath()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Battle") return;
            m = Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            m.name = "RDEnemyBreath";
        }
    }
}
