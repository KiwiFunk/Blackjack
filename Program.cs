// Create a 2D array to contain cards [x,y] x is the suit, y is the card
int[,] cardDeck = new int[,]
    { { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
    { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 } };

int totalCardsRemaining = 52;

Random cardSelect = new Random();

int currentTurn = 0;
string? returnResult;

int[] playerHand;
int[] dealerHand;
double bank = 00.00;

Console.WriteLine(drawCard(cardDeck));


/*
    Each player gets 2 cards. the dealer gets two cards (one remains face down). Player can chsoe to hit or stand. The dealer keeps drawing untill they 
    reach 17 or more. Or in this case, if the player stands, they will stop if their total is higher.
*/

int drawCard(int[,] cardDeck)
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
            totalCardsRemaining --;
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




