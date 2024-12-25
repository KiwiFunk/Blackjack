using BlackjackGame;

Cards[,] cardDeck = new Cards[4, 13];

int totalCardsRemaining = 52;
Random cardSelect = new Random();                       //Init a random function for selecting a card.
string? returnResult;

List<Player> players = new List<Player>();

List<Cards>[] playerHand = new List<Cards>[0];          //Create an array of lists for player hand(s). Lists are more efficient to add and replace values
Cards[] dealerHand = Array.Empty<Cards>();              //Array for dealer hand. Could update to list in the future.

double[,] bank = new double[0, 0];                      //Each row(player) has 2 columns, Index 0 holds bank balance, 1 Holds the current bet amount.


///Game Start///

BuildDeck();
TitleScreen();                                          //Display Title Screen
InitializePlayers();                                    //Init Players
Betting();                                              //Take Bets for each Player in the game
DealCards();                                            //Deal Cards for each player. Two face up each. Deal Cards for dealer, 1 face up, 1 face down.
for (int i = 0; i < players.Count; i++)                 //Iterate though player loop, moving on to the next once they have either stood, or gone bust.
{
    do
    {
        //Console.Clear();
        Console.WriteLine($"Current player: {players[i].playerName}");
        ShowHand(i);
        Console.WriteLine("Do you want to Hit, or Stand?");
        returnResult = Console.ReadLine();
        if (returnResult != null)
        {
            if (returnResult.ToLower().Trim() == "stand")
            {
                Stand(i);
            }
            else if (returnResult.ToLower().Trim() == "hit")
            {
                Hit(i);
            }
            else Console.WriteLine("Invalid Input. Would you like to Hit or Stand?");
        }
    } while (players[i].inGame);                                //Repeat loop while player is not Bust, and hasn't Stood
}
DealerPlay();                                           //Once Every player has stood or busted, Dealer takes their turn
GameEnd();                                              //Prompt replay, new game, or quit.


///Methods///

void GamePause() => Thread.Sleep(500);                 //Allows for globally adjusting the pause duration.

int TotalValue(string dealerOrPlayer, int index = 0)    //Take input for which player
{
    int totalValue = 0;
    switch (dealerOrPlayer)
    {
        case "player":
            foreach (var card in playerHand[index])
            {
                totalValue += card.value;
            }
            return totalValue;

        case "dealer":
            foreach (var card in dealerHand)
            {
              totalValue += card.value;  
            }
            return totalValue;

        default:
            Console.WriteLine("You have a logic error, check your paramenters when calling this method.");
            return 0;
    }
}

void ShowHand(int currentPlayer) 
{
    Console.WriteLine($"Your hand is currently: ");
    for (int i = 0; i < playerHand[currentPlayer].Count(); i++)
    {
        Console.Write($"{playerHand[currentPlayer][i].CardName}({playerHand[currentPlayer][i].value}), ");
    }
    Console.WriteLine("");
    Console.WriteLine($"For a total of {TotalValue("player", currentPlayer)}");
}

void BuildDeck()
{
    for (int i = 0; i < cardDeck.GetLength(0); i++)     //Populate our deck of cards on program start.
    {
        string[] names = new string[13] { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
        string suit = string.Empty;

        switch (i)
        {
            case 0:                                     //Arrays are 0-based.
                suit = "Clubs";
                break;
            case 1:
                suit = "Diamonds";
                break;
            case 2:
                suit = "Hearts";
                break;
            case 3:
                suit = "Spades";
                break;
        }

        for (int j = 0; j < cardDeck.GetLength(1); j++)
        {
            Cards card = new Cards();                   //Create a new instance of the Cards class for each array address.

            card.suit = suit;
            card.name = names[j];

            if (j > 9) card.value = 10;                 //In BlackJack, face cards count as 10
            else card.value = j + 1;

            card.inPlay = false;

            cardDeck[i, j] = card;                      //Assign the new card instace to the array address.
        }
    }
}

void TitleScreen()
{
    //Console.Clear();
    Console.WriteLine(@"
    ______ _            _      ___            _    
    | ___ \ |          | |    |_  |          | |   
    | |_/ / | __ _  ___| | __   | | __ _  ___| | __
    | ___ \ |/ _` |/ __| |/ /   | |/ _` |/ __| |/ /
    | |_/ / | (_| | (__|   </\__/ / (_| | (__|   < 
    \____/|_|\__,_|\___|_|\_\____/ \__,_|\___|_|\_\
                                                   ");
    Console.WriteLine("\t\tPress Any Key To Begin");
    Console.ReadKey();
}

void InitializePlayers()
{
    //Console.Clear();
    bool gameStart = false;
    Console.WriteLine("Please enter your Player Name(s) then hit return. Type Start to begin");
    do
    {
        returnResult = Console.ReadLine();
        if (returnResult != null && !String.IsNullOrEmpty(returnResult) && !String.IsNullOrWhiteSpace(returnResult))
        {
            if (returnResult.ToLower().Trim() == "start")
            {
                if (players.Count == 0)
                {
                    Console.WriteLine("You need to enter at least one name before starting!");
                }
                else
                {
                    gameStart = true;                                       //Start the game
                    //Console.Clear();
                }
            }
            else
            {
                Player player = new Player();
                player.playerName = $"{returnResult.ToUpper().Trim()}";
                player.inGame = true;
                player.isBust = false;
                player.hasAce = false;
                players.Add(player);
                //Console.Clear();
                Console.WriteLine($"{returnResult.ToUpper()} Registered. Enter another name, or type start to begin!");
            }
        }
        else Console.WriteLine("Please enter a valid name!");
    } while (!gameStart);
}



void Betting()
{
    bool validEnty = false;
    double currentWager = 0.0;
    if (bank.Length == 0)                                                   //Check to see if game is a new game, or continuation
    {
        double[,] bankInit = new double[players.Count, 2];
        bank = bankInit;
        for (int i = 0; i < players.Count; i++)
        {
            bank[i, 0] = 10.00;                                             //Starting Bank balance   
            bank[i, 1] = 0.00;                                              //Current Bet Amount
        }
        if (players.Count <= 1) Console.WriteLine("You have been given $10.00 as a new player bonus!");
        else Console.WriteLine("You have each been given a new player bonus of $10.00!");
        GamePause();
    }

    for (int i = 0; i < players.Count; i++)
    {
        validEnty = false;
        //Console.Clear();
        Console.WriteLine($"Current Player: {players[i].playerName}");
        Console.WriteLine($"You have ${bank[i, 0]:N2},how much do you want to wager?");

        do
        {
            returnResult = Console.ReadLine();
            if (double.TryParse(returnResult, out currentWager))
            {
                if (currentWager <= bank[i, 0])
                {
                    //Console.Clear();
                    Console.WriteLine($"{players[i].playerName} wagered ${currentWager:N2}! Good Luck!");
                    bank[i, 1] = currentWager;
                    bank[i, 0] -= currentWager;                             //Remove wagered amount from bank balance.
                    validEnty = true;
                    GamePause();
                }
                else Console.WriteLine($"Your bank balance is only {bank[i, 0]:N2}, please wager an amount you can afford!");
            }
            else Console.WriteLine("Please Enter a valid wager.");

        } while (!validEnty);
    }

    //Console.Clear();
    Console.WriteLine("All bets have been taken! Let the game begin!");
    GamePause();
    //Console.Clear();
}

Cards DrawCard(Cards[,] cardArray)
{
    int suit = cardSelect.Next(1, 4);
    int card = cardSelect.Next(1, 13);
    Cards draw = cardArray[suit, card];
    bool validCard = false;

    do
    {
        if (!cardDeck[suit, card].inPlay)                                    //If the selected array address is valid, replace it with 0, decrement total cards.
        {
            cardDeck[suit, card].inPlay = true;
            totalCardsRemaining--;
            validCard = true;
        }
        else                                                                //If selected address has already been set to 0, select a new one.
        {
            suit = cardSelect.Next(1, 4);
            card = cardSelect.Next(1, 13);
            draw = cardArray[suit, card];
        }
    } while (!validCard);
    return draw;
}

void DealCards()
{
    //Deal Cards for each player. Two face up each. Deal Cards for dealer, 1 face up, 1 face down.
    dealerHand = new Cards[2];
    dealerHand[0] = DrawCard(cardDeck);
    dealerHand[1] = DrawCard(cardDeck);
    Console.WriteLine($"Dealer draws a {dealerHand[1].value} and a face down card!");
    Console.WriteLine();
    GamePause();

    playerHand = new List<Cards>[players.Count];
    for (int i = 0; i < playerHand.Length; i++)
    {
        playerHand[i] = new List<Cards>();
        playerHand[i].Add(DrawCard(cardDeck));
        playerHand[i].Add(DrawCard(cardDeck));
        Console.WriteLine($"{players[i].playerName} drew a {playerHand[i][0].value} and {playerHand[i][1].value} for a total of {TotalValue("player", i)}!");
        Console.WriteLine();
        GamePause();
    }
}

void Hit(int currentPlayer)
{
    Console.WriteLine($"{players[currentPlayer].playerName} has decided to hit!");
    playerHand[currentPlayer].Add(DrawCard(cardDeck));

    //Handle Bust condition. Remove player from current loop. Remove their bet from bank balance.
    if (TotalValue("player", currentPlayer) > 21)
    {
        Console.WriteLine($"BUST!! {players[currentPlayer].playerName} drew a {playerHand[currentPlayer].Last().value} for a total of {TotalValue("player", currentPlayer)}!");
        players[currentPlayer].inGame = false;
        players[currentPlayer].isBust = true;
        bank[currentPlayer, 0] -= bank[currentPlayer, 1];
        GamePause();
    }
    else
    {
        Console.WriteLine($"{players[currentPlayer].playerName} drew a {playerHand[currentPlayer].Last().value} for a total of {TotalValue("player", currentPlayer)}!");
        GamePause();
    }
}

void Stand(int currentPlayer)
{
    //Console.Clear();
    Console.WriteLine($"{players[currentPlayer].playerName} has decided to stand! The total of their hand is {TotalValue("player", currentPlayer)}.");
    players[currentPlayer].inGame = false;
    GamePause();
}

void DealerPlay()
{
    bool dealerPlaying = true;
    //Reveal the face down card.
    Console.WriteLine($"Dealer reveals their facedown card. Their cards are {dealerHand[0].CardName}({dealerHand[0].value}) and {dealerHand[1].CardName}({dealerHand[1].value}).");
    GamePause();
    do
    {
        //If dealer total is < 17, they hit. If its >= 17 they stand.
        if (TotalValue("dealer") < 17)
        {
            Array.Resize(ref dealerHand, dealerHand.Length + 1);
            dealerHand[dealerHand.Length - 1] = DrawCard(cardDeck);
            Console.WriteLine($"Dealer draws a {dealerHand[dealerHand.Length-1].CardName}({dealerHand[dealerHand.Length-1].value}) for a total of {TotalValue("dealer")}.");

            //If dealer busts, every player who STOOD and didn't bust win.
            if (TotalValue("dealer") > 21)
            {
                Console.WriteLine("Dealer has gone Bust!");
                bool playerCashedOut = false;
                for (int i = 0; i < players.Count; i++)
                {
                    if (!players[i].isBust)
                    {
                        bank[i, 0] += bank[i, 1] * 2;
                        Console.WriteLine($"Congrats {players[i].playerName}, you win! Your bank balance is now ${bank[i, 0]}!");
                        playerCashedOut = true;
                    }
                }
                if (!playerCashedOut) Console.WriteLine("All players went bust. Nobody wins this time.");
                dealerPlaying = false;
            }
        }
        else
        {
            //If dealer reaches a valid hand, total and compare to each valid players hand
            Console.WriteLine($"Dealer stands with a total of {TotalValue("dealer")}");
            dealerPlaying = false;
            Cashout();
        }
    } while (dealerPlaying);
}

void Cashout()
{
    //Console.Clear();
    int dealerTotal = TotalValue("dealer");
    Console.WriteLine($"Dealer total is {dealerTotal}.");
    for (int i = 0; i < players.Count; i++)
    {
        int playerTotal = TotalValue("player", i);
        //If player hand higher, they win. If equal, they only recieve their original bet amount. If less, they lose.
        if (playerTotal > dealerTotal && !players[i].isBust)
        {
            bank[i, 0] += bank[i, 1] * 2;
            Console.WriteLine($"Congrats {players[i].playerName}, you win! Your bank balance is now ${bank[i, 0]}!");
        }

        else if (playerTotal == dealerTotal && !players[i].isBust) Console.WriteLine($"Better than nothing {players[i].playerName}, you tie!");

        else
        {
            Console.WriteLine($"Sorry {players[i].playerName}, you lose...");
            bank[i, 0] -= bank[i, 1];
        }
    }
    GamePause();
}

void GameEnd()
{
    ////Console.Clear();
    bool gameEnd = false;
    Console.WriteLine("Thanks for playing! Type 1 to start a new round, 2 for a New Game, or 3 to Quit!");
    do
    {
        returnResult = Console.ReadLine();
        if (returnResult != null)
        {
            if (returnResult.Trim() == "1")
            {
                Console.WriteLine("Feature not implemented yet, check back later! Enter 3 to quit.");
            }
            else if (returnResult.Trim() == "2")
            {
                Console.WriteLine("Feature not implemented yet, check back later! Enter 3 to quit.");
            }
            else if (returnResult.Trim() == "3")
            {
                gameEnd = true;
            }
            else Console.WriteLine("Invalid Input!");
        }
    } while (!gameEnd);
}
