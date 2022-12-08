using UnityEngine;

namespace Runtime.Data
{
    public interface IGameDataBase
    {
        void SaveData();
        void LoadData();
        void ClearData();
    }

    public class GameDataBase : IGameDataBase
    {
        private readonly GameData _gameData;

        public GameDataBase(GameData gameData)
        {
            _gameData = gameData;
        }

        public void SaveData()
        {
            var newJson = JsonUtility.ToJson(_gameData);

            PlayerPrefs.SetString("GameData", newJson);
        }

        public void LoadData()
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("GameData"), _gameData);

            if (_gameData.level == 0)
            {
                _gameData.firstLaunch = true;
                _gameData.level = 1;
            }
            else
            {
                _gameData.firstLaunch = false;
            }
        }

        public void ClearData()
        {
            PlayerPrefs.DeleteKey("GameData");
        }
    }
}