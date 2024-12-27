namespace BlackjackGame
{
    public class Player
    {
        public string playerName = string.Empty;                //Player Name
        public bool inGame;                                     //Has the player stood or not
        public bool isBust;                                     //Is the player bust?
        public bool hasAce;                                     //Flag to run ace 1/11 logic. Saves checking every player.
        public bool isBankrupt;                                 //Is the player bankrupt?
        public List<Cards> hand = new List<Cards>();            //Player's hand
        public double[] bank = new double[2];                   //Index 0 holds bank balance, 1 Holds the current bet amount.

        //Constructor
        public Player(string playerName, bool inGame = true, bool isBust = false, bool hasAce = false, bool isBankrupt = false, List<Cards>? hand = null, double[]? bank = null)
        {
            this.playerName = playerName;
            this.inGame = inGame;
            this.isBust = isBust;
            this.hasAce = hasAce;
            this.isBankrupt = isBankrupt;
            this.hand = hand ?? new List<Cards>();              // Provide a default value if hand is null
            this.bank = bank ?? new double[2];                  // Provide a default value if bank is null
        }

        public void ResetPlayerState()
        {
            inGame = true;
            isBust = false;
            hasAce = false;
            hand.Clear();
            bank[1] = 0.00;                                    //Reset the current bet amount   
        }

    }
}