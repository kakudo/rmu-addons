/*:
 *@addondesc Enemy HP Gage
 * @author Kakudo Yuie (Radian)
 *@help Enemy HP will be visible as a bar and value during battle.
 * 
 */

/*:ja
 * @addondesc 敵HP表示
 * @author 角度ゆいえ（Radian）
 * @help 戦闘中に敵のHPがバーと値で見えるようになる
 * 
 */

using UnityEngine;
using RPGMaker.Codebase.CoreSystem.Helper;
using UnityEngine.SceneManagement;

namespace RPGMaker.Codebase.Addon
{
    public class RDEnemyHPGage
    {
        private static readonly string PREFAB_PATH = "RDEnemyHPGage/RDEnemyHPGage";
        private static GameObject m;

        public RDEnemyHPGage()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Battle") return;
            m = Object.Instantiate(Resources.Load<GameObject>(PREFAB_PATH));
            m.name = "RDEnemyHPGage";
        }
    }

}
