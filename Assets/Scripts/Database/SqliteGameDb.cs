using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

namespace Database
{
    public class SqLiteGameDb : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void AddToDb(float score, bool isWin)
        {
            var dbPath = Path.Combine(Application.persistentDataPath, "game.db");
            using var conn = new SQLiteConnection(dbPath);
            conn.CreateTable<ScoreData>();
            var newScore = new ScoreData
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Core = score,
                IsWin = isWin
            };

            conn.InsertOrReplace(newScore);
        }
        public List<float> GetTop3Scores()
        {
            var dbPath = Path.Combine(Application.persistentDataPath, "game.db");
            using var conn = new SQLiteConnection(dbPath);
            conn.CreateTable<ScoreData>();
            var scores = conn.Table<ScoreData>()
                .OrderBy(s => s.Core)
                .Take(3)
                .Where(s => s.IsWin == true);
            var topScores = new List<float>();
            foreach (var score in scores)
            {
                topScores.Add(Mathf.Round(score.Core * 10f) / 10f);
            }
            return topScores;
        }
    }
}