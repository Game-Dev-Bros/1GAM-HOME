public static class Constants
{
    public static class Score
    {
        public const int WAVE_CLEARED_MULTIPLIER = 100;
        public const int ENEMY_SHIP_KILLED = 100;
    }

	public static class Waves
    {
		public static class Tags
		{
			public const string WAVE_NUMBER = "%WAVE%";
		}

		public const string WAVE_CLEARED = "Wave " + Tags.WAVE_NUMBER + " cleared!";
        public const string WAVE_START = "Wave "+ Tags.WAVE_NUMBER;
    }
}