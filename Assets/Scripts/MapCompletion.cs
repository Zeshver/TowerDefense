using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDefense
{
    public class MapCompletion : MonoSingleton<MapCompletion>
    {
        public const string filename = "completion.dat";        

        [Serializable]
        private class EpisodeScore
        {
            public Episode episode;
            public int score;
        }

        public static void SaveEpisodeResult(int levelScore)
        {
            if (Instance)
            {
                foreach (var item in Instance.completionData)
                {
                    if (item.episode == LevelSequenceController.Instance.CurrentEpisode)
                    {
                        if (levelScore > item.score)
                        {
                            Instance.totalScore += levelScore - item.score;
                            item.score = levelScore;
                            Saver<EpisodeScore[]>.Save(filename, Instance.completionData);
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"Episode complete with score {levelScore}");
            }
        }

        [SerializeField] private EpisodeScore[] completionData;
        private int totalScore;
        public int TotalScore => totalScore;

        private new void Awake()
        {
            base.Awake();
            Saver<EpisodeScore[]>.TryLoad(filename,ref completionData);
            foreach (var episodeScore in completionData)
            {
                totalScore += episodeScore.score;
            }
        }

        public int GetEpisodeScore(Episode m_Episode)
        {
            foreach(var data in completionData)
            {
                if (data.episode == m_Episode)
                {
                    return data.score;
                }
            }

            return 0;
        }
    }
}