namespace WordGame_Lib
{
    public class SessionStats
    {
        public SessionStats(bool iSuccess, int iNumGuesses, string iSecretWord, string iSecretWordDefinition)
        {
            Success = iSuccess;
            NumGuesses = iNumGuesses;
            SecretWord = iSecretWord;
            SecretWordDefinition = iSecretWordDefinition;
        }

        public bool Success { get; }
        public int NumGuesses { get; }
        public string SecretWord { get; }
        public string SecretWordDefinition { get; }
    }
}
