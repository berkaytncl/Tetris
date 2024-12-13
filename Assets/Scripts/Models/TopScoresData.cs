using System;

namespace Models
{
    [Serializable]
    public class TopScoresData
    {
        public Guid Id { get; private set; }

        public int Score;

        public TopScoresData(Guid id, int score)
        {
            Id = id;
            Score = score;
        }
    }
}