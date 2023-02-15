namespace TowerDefense
{
    public enum Sound
    {
        Arrow = 0,
        ArrowHit = 1,
        Magic = 2,
        EnemyDie = 3,
        EnemyWin = 4,
        PlayerWin = 5,
        PlayerLose = 6,
        BGM = 7, 
        BGMwin = 8,
        ButtonSound = 9
    }
    public static class SoundExtensions
    {
        public static void Play(this Sound sound)
        {
            SoundPlayer.Instance.Play(sound);
        }
    }
}