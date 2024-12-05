using System;
using System.IO;
using GameManagementModule;
using UnityEngine;

namespace BoardManagementModule
{
    public class HighScoreManager : IDisposable
    {
        public static event Action<int> HighScoreChangeEvent;
        
        private int _highScore;
        private readonly string _filePath;

        public HighScoreManager()
        {
            _filePath = Application.persistentDataPath + "/highscore.json";
            LoadHighScore();
            
            ListenEvents();
        }

        private void ListenEvents()
        {
            GameController.ScoreChangeEvent += UpdatedScore;
        }
        
        private void UpdatedScore(int score)
        {
            if (score <= _highScore) return;

            _highScore = score;
            SaveHighScore();
            HighScoreChangeEvent?.Invoke(_highScore);
        }

        private void SaveHighScore()
        {
            string json = JsonUtility.ToJson(new HighScoreData(_highScore));
            File.WriteAllText(_filePath, json);
        }

        private void LoadHighScore()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
                _highScore = data.highScore;
                HighScoreChangeEvent?.Invoke(_highScore);
            }
            else
            {
                _highScore = 0;
            }
        }

        private void UnsubscribeFromEvents()
        {
            GameController.ScoreChangeEvent -= UpdatedScore;
        }
        
        public void Dispose()
        {
            UnsubscribeFromEvents();
        }
        
        [System.Serializable]
        private class HighScoreData
        {
            public int highScore;

            public HighScoreData(int highScore)
            {
                this.highScore = highScore;
            }
        }
    }
}

// with PlayerPrefs example
// public class HighScoreManager
// {
//     private const string HighScoreKey = "HighScore";
//     
//     public static event Action<int> HighScoreChangeEvent;
//
//     private int _highScore;
//
//     public HighScoreManager()
//     {
//         _highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
//         HighScoreChangeEvent?.Invoke(_highScore);
//     }
//
//     public void UpdatedScore(int score)
//     {
//         if (score <= _highScore) return;
//         
//         _highScore = score;
//         PlayerPrefs.SetInt(HighScoreKey, _highScore);
//         PlayerPrefs.Save();
//
//         HighScoreChangeEvent?.Invoke(_highScore);
//     }
// }