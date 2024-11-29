using System.Linq;

// Create a 2D array to contain cards [x,y] x is the suit, y is the card
int[,] cardDeck = new int[,]
    { { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
    { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 } };

int totalCardsRemaining = 52;
Random cardSelect = new Random();
string? returnResult;

string[] players = { };                             //Array to store players
bool[] inGame = new bool[0];                        //Is the player currently in the game loop
List<int>[] playerHand = new List<int>[0];          //Create an array of lists for player hand(s). Lists are more efficient to add and replace values
int[] dealerHand = new int[0];
double[,] bank = new double[0, 0];                  //Using a 2D array to store money. Each row(player) has 2 columns, Index 0 holds bank balance, 1 Holds the current bet amount.
int round = 0;
bool validInput = false;                            

//Program start
Console.WriteLine("Weclome to BlackJack!");
Thread.Sleep(2000);
InitializePlayers();
Betting();
AssignCards("FirstRound");
AssignCards("dealer");
HitOrStand();

//Player loop where each player can decide to hit or stand (after the dealer has drawn if necessary), ends with incrementing the round number.)


void EvaluateWinConditions(int currentPlayer)               //Call from each HitOrStandLoop
{
    int totalScore = playerHand[currentPlayer].Sum();       //calculate sum of list at input array index
    Console.WriteLine(totalScore);

    if (totalScore == 21)           //if hand total is 21 and they have Blackjack
    {
        Console.WriteLine($"{players[currentPlayer]} has Blackjack!");
        validInput = true;
        //set a bool variable to break loop
        //Calculate payouts in a new method
    }
    else if (totalScore >= 22)      //If the player hand is over 21 and they have gone bust
    {
        Console.WriteLine($"{players[currentPlayer]} has gone bust with a total of {totalScore}!");
        validInput = true;
        //set a bool variable to break loop
        //Calculate payouts
    }
    else                            //If player doesnt have a total of 21 yet
    {
        // add something herelater
    }

}

void HitOrStand()           //add a loop until each player has reached a win condition
{
    for (int i = 0; i < playerHand.Length; i++)
    {
        if (inGame[i] == true)
        {
            Console.WriteLine($"{players[i]},");

            Console.WriteLine($"Your current total is {playerHand[i].Sum()}, do you want to stand or hit?");
            do
            {
                returnResult = Console.ReadLine();
                if (returnResult != null)
                {
                    if (returnResult.ToLower().Trim() == "stand")
                    {
                        Console.WriteLine($"{players[i]} has decided to stand! The total of their hand is {playerHand[i].Sum()}");
                        inGame[i] = false;
                        validInput = true;
                    }
                    else if (returnResult.ToLower().Trim() == "hit")
                    {
                        Console.WriteLine($"{players[i]} has decided to hit!");
                        AssignCards("player", i);
                        EvaluateWinConditions(i);
                    }
                    else Console.WriteLine("Invalid Input. Would you like to Hit or Stand?");
                }
            } while (validInput == false);
        }
    }
}


void AssignCards(string assignTo, int indexValue = 0)
{
    switch (assignTo)
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
                playerHand[i] = new List<int>();
                playerHand[i].Add(DrawCard(cardDeck));
                playerHand[i].Add(DrawCard(cardDeck));
                Console.WriteLine($"{players[i]} drew a {playerHand[i][0]} and {playerHand[i][1]} for a total of {(playerHand[i][0] + playerHand[i][1])}!");
                if (playerHand[i].Sum() >= 21)
                {
                    inGame[i] = false;
                    int total = playerHand[i].Sum();
                    Console.WriteLine($"Player was dealt {total}!");
                }
                //evaluateWinConditions();
            }
            break;

        case "player":

            playerHand[indexValue].Add(DrawCard(cardDeck));
            Console.WriteLine($"{players[indexValue]} draws a {playerHand[indexValue][playerHand.Length - 1]} for a total of {playerHand[indexValue].Sum()}.");
            Console.WriteLine("Their current hand is:");
            //Use .List to print the whole array address as a string instead of a foreach loop?
            foreach (int card in playerHand[indexValue])
            {
                Console.Write($"{card}, ");
            }
            Console.WriteLine();
            //evaluateWinConditions();
            break;

        case "dealer":
            //if the total of the dealer hand is 17 or more they must stand. If the total is 16 or under, they must take a card.
            //Work out conditions for if the dealer gets blackjack, or if the dealer does bust.


            if (dealerHand.Sum() >= 17)
            {
                Array.Resize(ref dealerHand, dealerHand.Length + 1);
                dealerHand[dealerHand.Length - 1] = DrawCard(cardDeck);        //Index is 0 based, so subtract 1

                //if draw a card but its not bust or blackjack.
                Console.WriteLine($"The dealer pulls a {dealerHand[dealerHand.Length - 1]} for a total of {dealerHand.Sum()}");
                Console.WriteLine("Their current hand is:");
                foreach (int card in dealerHand)
                {
                    Console.Write($"{card}, ");
                }
                Console.WriteLine();

                //else if its blackjack
                //Dealer has won, game end (check rules online)
                //dealer has blackjack, you lose!

                //else if dealer has gone bust
                //Dealer has bust proceed with win conditions (check rules online)
                //dealer has gone bust, players win!

                break;
            }
            else
            {
                //this shouldnt need to execute. put it in main loop that dealer method will only be called if their hand is 16 or under
                Console.WriteLine("Dealer has stood.");
                break;
            }
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
        Thread.Sleep(2000);
    }
    for (int i = 0; i < players.Length; i++)
    {
        validEnty = false;
        Console.Clear();
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
                    Thread.Sleep(2000);
                }
                else Console.WriteLine($"Your bank balance is only {bank[i, 0]:N2}, please wager an amount you can afford!");
            }
            else
            {
                Console.WriteLine("Please Enter a valid wager.");
            }
        } while (validEnty == false);
    }
    Console.Clear();
    Console.WriteLine("All bets have been taken! Let the game begin!");
    Thread.Sleep(2000);
    Console.Clear();
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
    Console.Clear();
    bool gameStart = false;
    string playerNames = "";
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
                    players = playerNames.Trim().ToUpper().Split(" ");      //Trim whitespace from start and end, create an array from player names then set
                    inGame = new bool[players.Length];                      //Init the boolean for each player
                    for (int i = 0; i < inGame.Length; i++)
                    {
                        inGame[i] = true;
                    }
                    gameStart = true;                                       //Start the game
                    Console.Clear();
                }
            }
            else
            {
                playerNames += returnResult.Trim() + " ";
                Console.Clear();
                Console.WriteLine($"{returnResult.ToUpper()} Registered. Enter another name, or type start to begin!");
                
            }
        }
        else Console.WriteLine("Please enter a valid name!");

    } while (gameStart == false);
}