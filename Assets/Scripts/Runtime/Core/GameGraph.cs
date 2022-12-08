using System;
using Runtime.Data;
using Runtime.Enemies;
using Runtime.Player;
using Runtime.Screens;
using Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace Runtime.Core
{
    public class GameGraph : MonoBehaviour
    {
        public DataGraph DataGraph { get; set; }
        public IGameDataBase GameDataBase { get; set; }
        public IScreensManager ScreensManager { get; set; }

        private void Awake()
        {
            var screensManagerGameObject = GameObject.Find(GameConstants.ScreensManager);
            var ultimateJoystick = GameObject.Find(GameConstants.MainJoystick).GetComponent<UltimateJoystick>();
            var levelControllerGameObject = GameObject.Find(GameConstants.Level);
            var playerGameObject = GameObject.Find(GameConstants.Player);
            var enemiesGameObject = GameObject.Find(GameConstants.Enemies);

            DataGraph = GetComponent<DataGraph>();
            GameDataBase = new GameDataBase(DataGraph.gameData);
            ScreensManager = FindObjectOfType<ScreensManager>();
            LevelController levelController = levelControllerGameObject.GetComponent<LevelController>();
            IPlayerMonoBehaviour playerMonoBehaviour = playerGameObject.GetComponent<PlayerMonoBehaviour>();
            EnemiesController enemiesController = enemiesGameObject.GetComponent<EnemiesController>();

            GameDataBase.LoadData();
            levelController.Inject();
            playerMonoBehaviour.Inject(this, ultimateJoystick);
            enemiesController.Inject();

            ScreensManager.RegularScreen.UpdateMoneyCounter(DataGraph.gameData.money);
        }

        private void OnDisable()
        {
            GameDataBase.SaveData();
        }

#if UNITY_EDITOR
        [MenuItem("Project commands/Special Command %g")]
        private static void SpecialCommand()
        {
            PlayerPrefs.DeleteAll();
            EditorApplication.EnterPlaymode();
        }
#endif
    }
}