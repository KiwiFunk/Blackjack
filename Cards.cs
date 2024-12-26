namespace BlackjackGame
{
    public class Cards
    {
        public string suit = string.Empty;
        public string name = string.Empty;
        public int value;
        public bool inPlay;
        public string CardName => $"{name} of {suit}";
    }

}
