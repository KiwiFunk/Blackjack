namespace BlackjackGame
{
    public class Cards
    {
        public string suit = string.Empty;
        public string name = string.Empty;
        public int value;
        public bool inPlay;
        public string CardName => $"{name} of {suit}";

        void CheckAceValue(int handTotal)
        {
            //take the hand total from TotalValue when calling this method
            //check it against args, use it to set a value of 11 or 1.
            if(handTotal <= 21 && name == "Ace") value = 11;
            else if(name == "Ace") value = 1;
        }
    }

}
