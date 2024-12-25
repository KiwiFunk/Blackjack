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
            //Start with hand value of 0
            //Count aces as 11 initially. Ace most valuable when counted as 11. Can be adjusted later
            //If hand is over 21, check for aces if the ace flag is present.
            //For each ace while hand is over 21, reduce value from 11 to 1

            if(handTotal <= 21 && name == "Ace") value = 11;
            else if(name == "Ace") value = 1;
        }
    }

}
