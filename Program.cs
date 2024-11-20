
//create method for checking win conditions. this needs to be performed every time a hand is udated.


// Create a 2D array to contain cards [x,y] x is the suit, y is the card
int[,] cardDeck = new int[,]
    { { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
    { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 } };

int totalCardsRemaining = 52;
Random cardSelect = new Random();
string? returnResult;

string[] players = { };                           //Array to store players

List<int>[] playerHand = new List<int>[0];        //Create an array of lists for player hand(s). Lists are more efficient to add and replace values
int[] dealerHand = new int[0];

double[,] bank = new double[0, 0];                //Using a 2D array to store money. Each row(player) has 2 columns, Index 0 holds bank balance, 1 Holds the current bet amount.
int round = 0;

bool gameStart = false;
string playerNames = "";


//Potentially implement an option for player limit, with 5 or 7 being the max. 4 may be better for the sake of a computer game.
Console.WriteLine("Weclome to BlackJack!");
InitializePlayers();
Betting();
AssignCards("FirstRound");
AssignCards("dealer");
AssignCards("player");
//HitOrStand



void AssignCards(string currentTurn)
{
    switch (currentTurn)
    {
        case "FirstRound":

            dealerHand = new int[2];
            dealerHand[0] = DrawCard(cardDeck);
            dealerHand[1] = DrawCard(cardDeck);
            Console.WriteLine($"The dealer draws a {dealerHand[1]} and a face down card!");
            //evaluateWinConditions();

            playerHand = new List<int>[players.Length];
            for (int i = 0; i < playerHand.Length; i++)
            {
                playerHand[i] = new List<int> ();
                playerHand[i].Add(DrawCard(cardDeck));
                playerHand[i].Add(DrawCard(cardDeck));
                Console.WriteLine($"{players[i]} drew a {playerHand[i][0]} and {playerHand[i][1]} for a total of {(playerHand[i][0] + playerHand[i][1])}!");
                //evaluateWinConditions();
            }
            break;

        case "player":

            for (int i = 0; i <playerHand.Length; i++)
            {
                playerHand[i].Add(DrawCard(cardDeck));
                Console.WriteLine($"{players[i]} draws a {playerHand[i][playerHand.Length - 1]} for a total of {playerHand[i].Sum()}.");
                Console.WriteLine("Their current hand is:");
                //Use .List to print the whole array address as a string instead of a foreach loop?
                foreach (int card in playerHand[i])
                {
                    Console.Write($"{card}, ");
                }
                Console.WriteLine();
                //evaluateWinConditions();
            }
            break;

        case "dealer":

            Array.Resize(ref dealerHand, dealerHand.Length + 1);
            dealerHand[dealerHand.Length - 1] = DrawCard(cardDeck);        //Index is 0 based, so subtract 1
            Console.WriteLine($"The dealer pulls a {dealerHand[dealerHand.Length - 1]} for a total of {dealerHand.Sum()}");
            Console.WriteLine("Their current hand is:");
            foreach (int card in dealerHand)
            {
                Console.Write($"{card}, ");
            }
            Console.WriteLine();
            //evaluateWinConditions();
            break;
    }

}


void Betting()
{
    bool validEnty = false;
    double currentWager = 0.0;
    if (bank.Length == 0)            //Check to see if game is a new game, or continuation
    {
        double[,] bankInit = new double[players.Length, 2];
        bank = bankInit;
        for (int i = 0; i < players.Length; i++)
        {
            bank[i, 0] = 10.00;      //Starting Bank balance   
            bank[i, 1] = 0.00;       //Current Bet Amount
        }
        if (players.Length <= 1) Console.WriteLine("You have been given $10.00 as a new player bonus!");
        else Console.WriteLine("You have each been given a new player bonus of $10.00!");
    }
    for (int i = 0; i < players.Length; i++)
    {
        validEnty = false;
        Console.WriteLine($"Current Player: {players[i]}");
        Console.WriteLine($"You have ${bank[i, 0]:N2},how much do you want to wager?");
        do
        {
            returnResult = Console.ReadLine();
            if (double.TryParse(returnResult, out currentWager))
            {
                if (currentWager <= bank[i, 0])
                {
                    Console.WriteLine($"{players[i]} wagered ${currentWager:N2}! Good Luck!");
                    bank[i, 1] = currentWager;
                    validEnty = true;
                }
                else Console.WriteLine($"Your bank balance is only {bank[i, 0]:N2}, please wager an amount you can afford!");
            }
            else
            {
                Console.WriteLine("Please Enter a valid wager.");
            }
        } while (validEnty == false);
    }
    Console.WriteLine("All bets have been taken! Let the game begin!");
}


int DrawCard(int[,] cardArray)
{
    int suit = cardSelect.Next(1, 4);
    int card = cardSelect.Next(1, 13);
    int draw = cardArray[suit, card];
    bool validCard = false;

    do
    {
        if (draw != 0)
        {
            cardDeck[suit, card] = 0;
            totalCardsRemaining--;
            validCard = true;
        }
        else
        {
            suit = cardSelect.Next(1, 4);
            card = cardSelect.Next(1, 13);
            draw = cardArray[suit, card];
        }
    } while (validCard == false);
    return draw;
}


void InitializePlayers()
{
    Console.WriteLine("Please enter your Player Name(s) then hit return. Type Start to begin");
    do
    {
        returnResult = Console.ReadLine();
        if (returnResult != null && !String.IsNullOrEmpty(returnResult))
        {
            if (returnResult.ToLower().Trim() == "start")
            {
                if (playerNames == "")
                {
                    Console.WriteLine("You need to enter at least one name before starting!");
                }
                else
                {
                    //Create an array out of the playerNames list
                    players = playerNames.Trim().ToUpper().Split(" ");     //Trim whitespace from start and end, create an array from player names then set
                    gameStart = true;
                }
            }
            else
            {
                playerNames += returnResult.Trim() + " ";
                Console.WriteLine($"{returnResult} Registered. Enter another name, or type start to begin!");
            }
        }
        else Console.WriteLine("Please enter a valid name!");

    } while (gameStart == false);
}