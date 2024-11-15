// Create a 2D array to contain cards [x,y] x is the suit, y is the card
int[,] cardDeck = new int[,]
    { { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
    { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 } };

int totalCardsRemaining = 52;

Random cardSelect = new Random();

//int currentTurn = 0;
string? returnResult;


string[] players = {};                                //Array to store players
//int[,] playersHand;                                 //2D array to store current players hands. row= player, column = their cards init using players[].Length to decide x [x,]
//int[] dealerHand;
//double bank = 00.00;

bool gameStart = false;
string playerNames = "";


//Potentially implement an option for player limit, with 5 or 7 being the max. 4 may be better for the sake of a conputer game.
Console.WriteLine("Weclome to BlackJack!");
InitializePlayers();

foreach (string player in players)
{
    Console.WriteLine($"{player}");
    //call the dealcards option for each player and populate the playersHand[,] array.
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
    else Console.WriteLine ("Please enter a valid name!");

} while (gameStart == false);
}




//create method for checking win conditions. this needs ot be performed every time a hand is udated.

/*
void DealCards(string switchCase)
{
    switch(switchCase)
    {
        case "FirstRound":
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

*/







//init total players - How can we do this?

/*
Prompt user to enter player names, type start to begin.
For each string entered that isnt null, add to an array of players
We can then iterate through the player array in the main() loop

best to use a foreach as we dont know how many players will be playing

foreach (player in players)
{
    Game logic here
}

Basic blackjack rules

everyone gets two cards, the dealer shows one card, the other is face down
reveal the face down card once the player has folded


*/


//Promt enter player names




//Main - Player first time?

//if yes


// Console.WriteLine(drawCard(cardDeck));


/*
    Each player gets 2 cards. the dealer gets two cards (one remains face down). Player can chsoe to hit or stand. The dealer keeps drawing untill they 
    reach 17 or more. Or in this case, if the player stands, they will stop if their total is higher.
*/



