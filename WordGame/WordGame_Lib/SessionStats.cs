namespace WordGame_Lib
{
    public class SessionStats
    {
        public SessionStats(bool iSuccess, int iNumGuesses, string iSecretWord)
        {
            Success = iSuccess;
            NumGuesses = iNumGuesses;
            SecretWord = iSecretWord;
        }

        public bool Success { get; }
        public int NumGuesses { get; }
        public string SecretWord { get; }
    }
}
