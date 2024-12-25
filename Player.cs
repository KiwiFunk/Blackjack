namespace BlackjackGame
{
    public class Player
    {
        public string playerName = string.Empty;                //Player Name
        public bool inGame;                                     //Has the player stood or not
        public bool isBust;                                     //Is the player bust?
        public bool hasAce;                                     //Flag to run ace 1/11 logic. Saves checking every player.

        //Migrate bank into player class?

    }
}