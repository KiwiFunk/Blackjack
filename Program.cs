
//create method for checking win conditions. this needs ot be performed every time a hand is udated.


// Create a 2D array to contain cards [x,y] x is the suit, y is the card
int[,] cardDeck = new int[,]
    { { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
    { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 } };

int totalCardsRemaining = 52;
Random cardSelect = new Random();
string? returnResult;

string[] players = { };                             //Array to store players
int[,] playersHand;                               //2D array to store current players hands. row= player, column = their cards init using players[].Length to decide x [x,]
int[] dealerHand;
double[,] bank = new double[0,0];                   //Using a 2D array to store money. Each row(player) has 2 columns, Index 0 holds bank balance, 1 Holds the current bet amount.
int round = 0;

bool gameStart = false;
string playerNames = "";


//Potentially implement an option for player limit, with 5 or 7 being the max. 4 may be better for the sake of a computer game.
Console.WriteLine("Weclome to BlackJack!");
InitializePlayers();
Betting();

/*
foreach (string player in players)
{
    Console.WriteLine($"{player}");
    //call the dealcards option for each player and populate the playersHand[,] array.
}
*/


void AssignCards(string switchCase)
{
    switch(switchCase)
    {
        case "FirstRound":
        for (int i = 0; i < players.Length; i++)
        {
            //create new 

        }
        
        
        //If the first round is called, we need to give everyone TWO cards, and then make sure that one of the dealers is hidden (The dealer only reveals the hidden card after the players all fold.)
        break;

        case "player":
        //Same as the dealer logic, only with an additional check to make sure that the correct player hand is updated
        break;

        case "dealer":
        //Most basic Logic. get a card, add to hand, run the evaluateWinConditions() method.
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


int DrawCard(int[,] cardDeck)
{
    int suit = cardSelect.Next(1, 4);
    int card = cardSelect.Next(1, 13);
    int draw = cardDeck[suit, card];
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
            draw = cardDeck[suit, card];
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
