using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Utility.Managers
{
    public class ScoreManager
    {
        public static event Action<List<TopScoresData>> TopScoresChangeEvent;
        public static event Action<int> ScoreChangeEvent;
        
        private const string ScoreDataFileName = "score_data.json";

        public GameData GameData { get; private set; }

        public List<TopScoresData> TopScores { get; private set; }
        
        private string FilePath => Path.Combine(Application.persistentDataPath, ScoreDataFileName);

        public ScoreManager()
        {
            GameData = new GameData();
            
            LoadScores();
        }

        public void NewGame()
        {
            GameData = new GameData();
            ScoreChangeEvent?.Invoke(GameData.CurrentScore);
        }
        
        public void AddToCurrentScore(int points)
        {
            GameData.CurrentScore += points;
            ScoreChangeEvent?.Invoke(GameData.CurrentScore);

            SaveCurrentScore();
        }
        
        public void SaveCurrentScore()
        {
            var existingItem = TopScores.FirstOrDefault(scoreData => scoreData.Id == GameData.Id);
            
            if (existingItem != null)
            {
                if (existingItem.Score > GameData.CurrentScore) return;
                
                existingItem.Score = GameData.CurrentScore;
            }
            else
            {
                TopScores.Add(new TopScoresData(GameData.Id, GameData.CurrentScore));
                TopScores = TopScores.OrderByDescending(score => score.Score).Take(10).ToList();
            }
            
            TopScoresChangeEvent?.Invoke(TopScores);

            SaveScores();
        }
        
        private void ResetScore()
        {
            GameData.CurrentScore = 0;
            ScoreChangeEvent?.Invoke(GameData.CurrentScore);
        }
        
        public void HardResetScores()
        {
            GameData.CurrentScore = 0;
            TopScores.Clear();
            SaveScores();
        }

        private void SaveScores()
        {
            string json = JsonConvert.SerializeObject(TopScores);
            
            File.WriteAllText(FilePath, json);
        }

        private void LoadScores()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var scoreData = JsonConvert.DeserializeObject<List<TopScoresData>>(json);
                TopScores = scoreData ?? new List<TopScoresData>();
                TopScoresChangeEvent?.Invoke(TopScores);
            }
            else
            {
                TopScores = new List<TopScoresData>();
            }
        }
    }
}