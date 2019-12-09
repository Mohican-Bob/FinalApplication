using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FinalApplication
{
    class Program

    {
        //************************************************************
        //Project: Final Application
        //Course: CIT 110
        //Creater: Joshua Paul
        //Date Created: 12/1/19
        //Last Modified: 12/9/19
        //*************************************************************
        //
        // Menu script from https://www.youtube.com/watch?v=1ydSw4afA1o
        // Youtube user darahz
        // Found and access 12/7/19
        //

        //creates variable to represent menu chosen menu item
        private static int index = 0;
        



        private static void Main(string[] args)
        {
            List<Cards> dealtCards = DealtCards();
            List<Cards> dealerCards = DealtCards();
            List<Cards> cards = ShuffleShoe();
            DisplayScreenHeader("\t\tWelcome to Blackjack 2.0!");
            DisplayContinuePrompt();
            MainMenu(cards, dealtCards, dealerCards);

        }
        private static void MainMenu(List<Cards> cards, List<Cards> dealtCards, List<Cards> dealerCards)
        {
            double cash = 1000;
            double loss = 0;
            double wager = 0;
            // player game menu
            //
            // Menu script from https://www.youtube.com/watch?v=1ydSw4afA1o
            // Youtube user darahz
            // Found and accessed 12/7/19
            // I added in notes describing how it works
            //
           
            //creates list to index for menu choices
            List<string> menuItems = new List<string>() {
                "Start Game",
                "Load Last Game",
                "Exit"
            };
            // turns off cursor
            Console.CursorVisible = false;
            while (true)
            {
                // Loop that calls the methods inside menu
                DisplayScreenHeader("\tMain Menu");
                string selectedMenuItem = drawMenu(menuItems);
                if (selectedMenuItem == "Start Game")
                {

                      
                        GameStart(cards, dealtCards, dealerCards, cash, loss, wager);

                }
                else if (selectedMenuItem == "Load Last Game")
                {

                    cash = LoadGame();
                    MainMenu(cards, dealtCards, dealerCards);
                }
                else if (selectedMenuItem == "Exit")
                {
                    //command to exit application
                    QuitApp();
                }
            }
        }

        static void GameStart(List<Cards> cards, List<Cards> dealtCards, List<Cards> dealerCards, double cash, double loss, double wager)
        {



            bool play = true;
            double playerTotal;
            double dealerActual;
            do
            {
                ClearDeck(cards, dealtCards, dealerCards);
                InitializeHand(cards, dealtCards, dealerCards);
                wager = Betting(cash, loss);
                loss = wager;
                DisplayScreenHeader("Black Jack");
                Console.WriteLine();
                Console.WriteLine("Press H to hit -- S to Stand -- V to Save -- Q to Quit");


                Console.WriteLine("Cash: $" + (cash - wager));
                Console.WriteLine("You bet $" + wager);
                Console.WriteLine();


                Console.WriteLine("dealer has " + dealerCards[0].Name + " of " + dealerCards[0].Suit);
                double dealerTotal = (int)dealerCards[0].value;


                Console.WriteLine();
                Console.WriteLine("You have " + dealtCards[0].Name + " of " + dealtCards[0].Suit);

                Console.WriteLine("You have " + dealtCards[1].Name + " of " + dealtCards[1].Suit);
                playerTotal = (int)dealtCards[0].value + (int)dealtCards[1].value;
                Console.WriteLine();
                Console.WriteLine("Dealer Has: " + dealerTotal + "  You have: " + playerTotal);
                Console.WriteLine();
                bool valid = false;
                do
                {
                    ConsoleKeyInfo ckey = Console.ReadKey(true);
                    switch (ckey.Key)
                    {
                        case ConsoleKey.H:
                            
                            Console.WriteLine(PlayerHit(cards, dealtCards));
                            playerTotal += (int)dealtCards[dealtCards.Count - 1].value;
                            Console.WriteLine("New Total: " + playerTotal);
                            if (playerTotal > 21)
                            {
                                valid = true;
                            }
                            else if (playerTotal < 21)
                            {
                                valid = false;
                            }
                            break;
                        case ConsoleKey.S:
                            Console.WriteLine("You Stayed");
                            valid = true;
                            break;
                        case ConsoleKey.V:
                            WriteToDataFile(cash);
                            break;
                        case ConsoleKey.Q:
                            QuitApp();
                            break;
                        default:
                            valid = true;
                            break;
                    }
                } while (!valid);
                Console.WriteLine("dealer has " + dealerCards[0].Name + " of " + dealerCards[0].Suit);
                Console.WriteLine("dealer has " + dealerCards[1].Name + " of " + dealerCards[1].Suit);
                dealerActual = (int)dealerCards[0].value + (int)dealerCards[1].value;
                bool dealer = true;
                if (dealerActual < 17)
                {
                    while (dealer)
                    {
                        Console.WriteLine(DealerHit(cards, dealerCards));
                        dealerActual = dealerActual + DealerActual(dealerCards, dealerActual);
                        if (dealerActual > 16 && dealerActual < 22)
                        {
                            dealer = false;
                        }
                        if (dealerActual > 21)
                        {
                            dealer = false;
                        }
                    }
                }
                Console.WriteLine("Dealer has: " + dealerActual);
                if (dealerActual > 21 && playerTotal < 22)
                {
                    Console.WriteLine();
                    Console.WriteLine("Dealer Busted! You Win!");
                    Console.WriteLine("You Won: " + wager);
                    Console.WriteLine("New Total: " + cash);
                    Console.WriteLine();
                    cash += wager;
                    DisplayContinuePrompt();
                }
                else if (playerTotal > 21)
                {
                    Console.WriteLine();
                    Console.WriteLine("You Bust");
                    cash -= wager;
                    Console.WriteLine("You lost: " + wager);
                    Console.WriteLine("New Total: " + cash);
                    DisplayContinuePrompt();



                }
                else if (playerTotal < dealerActual)
                {

                    Console.WriteLine("Dealer Beat You");
                    cash -= wager;
                    Console.WriteLine("You lost: " + wager);
                    Console.WriteLine("New Total: " + cash);
                    DisplayContinuePrompt();



                }
                else if (playerTotal == dealerActual)
                {
                    Console.WriteLine("You Pushed");
                    DisplayContinuePrompt();


                }
                else if (playerTotal > dealerActual && playerTotal < 22)
                {

                    Console.WriteLine("You Won!");
                    cash += wager;
                    Console.WriteLine("You lost: " + wager);
                    Console.WriteLine("New Total: " + cash);
                    DisplayContinuePrompt();

                }
            }
            while (play);







        }
        static double Betting(double cash, double loss)
        {
            // validate user input!!

            string betAsString;
            double bet;
            bool valid = false;
            double wager;
            do
            {

                DisplayScreenHeader("Anti Up!");
                Console.WriteLine();
                Console.WriteLine("you have: " + cash);
                Console.WriteLine("how much would you like to bet?");


                betAsString = Console.ReadLine();
                if (Double.TryParse(betAsString, out bet))
                {
                    valid = false;
                }
                if (bet > cash)
                {
                    Console.WriteLine("You cannot bet higher than you have");
                    DisplayContinuePrompt();
                    valid = true;
                }
                if (bet <= 0)
                {
                    Console.WriteLine("you cannot bet 0 or less");
                    DisplayContinuePrompt();
                    valid = true;
                }
                if (!Double.TryParse(betAsString, out bet))
                {
                    Console.WriteLine("Please enter a valid number");
                    DisplayContinuePrompt();
                    valid = true;
                }
            } while (valid);
            wager = Convert.ToDouble(betAsString);
            Console.WriteLine("You bet: $" + wager);
            DisplayContinuePrompt();
            return wager;
        }
        // 
        // New Code
        //
        //static double Loss(double loss, double cash)
        //{
        //    cash = cash - loss;
        //    return cash;

        static double NewCash(double cash, double loss)
        {

            cash = cash - loss;

            return cash;
        }
        static double Win(double cash, double wager)
        {
            cash = cash + wager;
            return cash;
        }
        static double Lose(double cash, double wager)
        {
            cash = cash - wager;
            return cash;
        }
        static void ClearDeck(List<Cards> cards, List<Cards> dealtCards, List<Cards> dealerCards)
        {
            dealerCards.Clear();
            dealtCards.Clear();
            cards.Clear();
            string[] datastring = File.ReadAllLines("Data\\Data.txt");


            foreach (string newCards in datastring)
            {
                //
                // get individual properties
                //
                string[] cardArray = newCards.Split(',');

                //
                // create monster
                //
                Enum.TryParse(cardArray[0], out Cards.CardName Name);


                Enum.TryParse(cardArray[1], out Cards.Value value);


                Enum.TryParse(cardArray[2], out Cards.Type suit);

                Cards newDeck = new Cards(Name, value, suit);
                //
                // add new monster to list
                //
                cards.Add(newDeck);
            }
        }

        static string DealerHit(List<Cards> cards, List<Cards> dealerCards)
        {

            dealerCards.Add(Dealer(cards));
            cards.Remove(Dealer(cards));            
            string x = "Dealer got a " + dealerCards[dealerCards.Count - 1].Name + " of " + dealerCards[dealerCards.Count - 1].Suit;
            return x;
        }
        static string PlayerHit(List<Cards> cards, List<Cards> dealtCards)
        {

            dealtCards.Add(Dealer(cards));
            cards.Remove(Dealer(cards));
            string x = "You got a " + dealtCards[dealtCards.Count -1].Name + " of " + dealtCards[dealtCards.Count -1].Suit;
            return x;
        }
        // initialize hand for the table
        public static List<Cards> DealerCards()
        {
            {
                List<Cards> dealerCards = new List<Cards>();
                return dealerCards;
            }
        }
        public static List<Cards> DealtCards()
        {
            {
                List<Cards> dealtCards = new List<Cards>();
                return dealtCards;
            }
        }
        // starts hand for game
        static void InitializeHand(List<Cards> cards, List<Cards> dealtCards, List<Cards> dealerCards)
        {

            dealerCards.Add(Dealer(cards));
            cards.Remove(Dealer(cards));
            dealerCards.Add(Dealer(cards));
            cards.Remove(Dealer(cards));
            dealtCards.Add(Dealer(cards));
            cards.Remove(Dealer(cards));
            dealtCards.Add(Dealer(cards));

        }
        static double DealerActual(List<Cards> dealerCards, double dealerActual)
        {
            dealerActual += (int)dealerCards[dealerCards.Count - 1].value;

            return dealerActual;

        }
        static double PlayerTotal(List<Cards> dealtCards, double playerTotal)
        {
            
            playerTotal += (int)dealtCards[dealtCards.Count - 1].value;

            return playerTotal;
        }
        static Cards Dealer(List<Cards> cards)
        {
            var dealer = new Random();

            return cards[dealer.Next(cards.Count)];

        }


        static List<Cards> ShuffleShoe()
        {
            List<Cards> cards = new List<Cards>();

            //
            // read all lines in the file
            //
            string[] datastring = File.ReadAllLines("Data\\Data.txt");

            //
            // create monster objects and add to the list
            //
            foreach (string newCards in datastring)
            {
                //
                // get individual properties
                //
                string[] cardArray = newCards.Split(',');

                //
                // create monster
                //
                Enum.TryParse(cardArray[0], out Cards.CardName Name);


                Enum.TryParse(cardArray[1], out Cards.Value value);


                Enum.TryParse(cardArray[2], out Cards.Type suit);

                Cards newDeck = new Cards(Name, value, suit);
                //
                // add new monster to list
                //
                cards.Add(newDeck);
            }


            return cards;
        }
        static double LoadGame()
        {
            

            //
            // read all lines in the file
            //
            string[] datastring = File.ReadAllLines("Data\\Cash.txt");

            //
            // create monster objects and add to the list
            //
            double.TryParse(datastring[0], out double load);
            Console.WriteLine("Game has Loaded");
            DisplayContinuePrompt();


            return load;
        }
        static void WriteToDataFile(double cash)
        {
            
            string[] cashSave = new string[1];
            cashSave[0] = cash.ToString();

            File.WriteAllLines("Data\\Cash.txt", cashSave);

            Console.WriteLine("Your cash has been saved!");
            DisplayContinuePrompt();
            
        }

        #region Menu Methods
        private static string drawMenu(List<string> items)
        {
            // method to highlight options in menu
            // returns WriteLine at corresponding index of List
            for (int i = 0; i < items.Count; i++)
            {
                if (i == index)
                {
                    // highlights option in menu
                    // rewrites list each keystroke and changes color of list item according to index
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;

                    Console.WriteLine(items[i]);
                }
                else
                {
                    Console.WriteLine(items[i]);
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo ckey = Console.ReadKey();
            // reads keystrokes to scroll through menu list index
            if (ckey.Key == ConsoleKey.DownArrow)
            {
                // adds one to the index by the down arrow
                // if index gets to the max, do nothing. else add one to index
                if (index == items.Count - 1)
                {

                }
                else { index++; }
            }

            else if (ckey.Key == ConsoleKey.UpArrow)
            {
                // lowers one to the index by the up arrow
                // if index is equal to 0 do nothing. else subrtact from index
                if (index <= 0)
                {

                }
                else { index--; }
            }
            else if (ckey.Key == ConsoleKey.Enter)
            {
                return items[index];
            }
            else if (ckey.Key != ConsoleKey.Enter || ckey.Key != ConsoleKey.UpArrow || ckey.Key != ConsoleKey.DownArrow)
            {
                // added in this to protect against false keystrokes
                // do nothing is not the set three keystrokes
            }
            else
            {
                return "";
            }

            Console.Clear();
            return "";
        }
        static void QuitApp()
        {
            DisplayScreenHeader("Quit App");
            Console.WriteLine("Thank you for Playing");
            DisplayContinuePrompt();
            Environment.Exit(0);
        }
        #endregion
        //static List<Cards> InitializeDeck()
        //{

        //    List<Cards> dealtCards = new List<Cards>()
        //    {
        //        //clubs
        //        new Cards(Cards.CardName.Two,Cards.Value.two,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Three,Cards.Value.three,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Four,Cards.Value.four,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Five,Cards.Value.five,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Six,Cards.Value.six,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Seven,Cards.Value.seven,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Eight,Cards.Value.eight,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Nine,Cards.Value.nine,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Ten,Cards.Value.ten,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Jack,Cards.Value.ten,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Queen,Cards.Value.ten,Cards.Type.clubs),
        //        new Cards(Cards.CardName.King,Cards.Value.ten,Cards.Type.clubs),
        //        new Cards(Cards.CardName.Ace,Cards.Value.eleven,Cards.Type.clubs),
        //        //spades
        //        new Cards(Cards.CardName.Two,Cards.Value.two,Cards.Type.spades),
        //        new Cards(Cards.CardName.Three,Cards.Value.three,Cards.Type.spades),
        //        new Cards(Cards.CardName.Four,Cards.Value.four,Cards.Type.spades),
        //        new Cards(Cards.CardName.Five,Cards.Value.five,Cards.Type.spades),
        //        new Cards(Cards.CardName.Six,Cards.Value.six,Cards.Type.spades),
        //        new Cards(Cards.CardName.Seven,Cards.Value.seven,Cards.Type.spades),
        //        new Cards(Cards.CardName.Eight,Cards.Value.eight,Cards.Type.spades),
        //        new Cards(Cards.CardName.Nine,Cards.Value.nine,Cards.Type.spades),
        //        new Cards(Cards.CardName.Ten,Cards.Value.ten,Cards.Type.spades),
        //        new Cards(Cards.CardName.Jack,Cards.Value.ten,Cards.Type.spades),
        //        new Cards(Cards.CardName.Queen,Cards.Value.ten,Cards.Type.spades),
        //        new Cards(Cards.CardName.King,Cards.Value.ten,Cards.Type.spades),
        //        new Cards(Cards.CardName.Ace,Cards.Value.eleven,Cards.Type.spades),
        //        //hearts
        //        new Cards(Cards.CardName.Two,Cards.Value.two,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Three,Cards.Value.three,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Four,Cards.Value.four,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Five,Cards.Value.five,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Six,Cards.Value.six,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Seven,Cards.Value.seven,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Eight,Cards.Value.eight,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Nine,Cards.Value.nine,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Ten,Cards.Value.ten,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Jack,Cards.Value.ten,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Queen,Cards.Value.ten,Cards.Type.hearts),
        //        new Cards(Cards.CardName.King,Cards.Value.ten,Cards.Type.hearts),
        //        new Cards(Cards.CardName.Ace,Cards.Value.eleven,Cards.Type.hearts),
        //        //diamons
        //        new Cards(Cards.CardName.Two,Cards.Value.two,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Three,Cards.Value.three,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Four,Cards.Value.four,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Five,Cards.Value.five,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Six,Cards.Value.six,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Seven,Cards.Value.seven,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Eight,Cards.Value.eight,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Nine,Cards.Value.nine,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Ten,Cards.Value.ten,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Jack,Cards.Value.ten,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Queen,Cards.Value.ten,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.King,Cards.Value.ten,Cards.Type.diamonds),
        //        new Cards(Cards.CardName.Ace,Cards.Value.eleven,Cards.Type.diamonds)
        //    };
        //    return dealtCards;
        //}

        #region CONSOLE HELPER METHODS
        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you Playing!");
            Console.WriteLine();

            DisplayContinuePrompt();
            
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.CursorVisible = false;
            Console.WriteLine();
            Console.Write("\tPress any key to continue.");
            Console.ReadKey();
            Console.CursorVisible = true;
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
