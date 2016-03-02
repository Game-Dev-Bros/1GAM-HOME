public static class Constants
{
    public static class Score
    {
        public const int WAVE_CLEARED_MULTIPLIER = 100;
        public const int ENEMY_SHIP_KILLED = 100;
    }

    public static class Levels
    {
        public const string MAIN_MENU = "MainMenu";
        public const string PAUSE_MENU = "PauseMenu";
        public const string LOSE_MENU = "LoseMenu";
        public const string GAME = "Gameplay";
    }

    public static class Strings
    {
        public const string HIGHSCORE_PREFIX = "Highscore: ";
        public const string GAME_OVER = "Your planet was invaded\n by the earthling scum.\nGame over!";
    }

	public static class Prefs
    {
        public const string HIGHSCORE = "HIGHSCORE";
    }

	public static class Waves
    {
		public static class Tags
		{
			public const string WAVE_NUMBER = "%WAVE%";
		}

		public const string WAVE_CLEARED = "Wave " + Tags.WAVE_NUMBER + " cleared!";
        public const string WAVE_START = "Wave "+ Tags.WAVE_NUMBER;

        public const int WAVE_MACHINEGUN_DROP = 3;
        public const int WAVE_ROCKETLAUNCHER_DROP = 5;
    }

    public static class Game
    {

        public static string PLAYER_NAME = "Cube";
       
    }
}