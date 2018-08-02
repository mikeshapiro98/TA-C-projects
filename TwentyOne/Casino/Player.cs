using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class Player
    {
        public Player(string name) : this(name, 100) //this is an example of a contructor chain. It's saying that the default start amount is 100, and now it doesn't need a beginning balance. It just defaults
        {
            //THERE DOESN'T NEED TO BE ANYTHING HERE BECAUSE WE ARE USING THE CONSTRUCTOR BELOW AND REUSING THE CODE IN IT. THE COLON SEEMS TO HAVE INHERITED MADE IT AN INHERITED PROPERTY.
        }
        public Player(string name, int beginningBalance)
        {
            Hand = new List<Card>();
            Balance = beginningBalance;

            Name = name;  

        }

        public List<Card> Hand { get; set; } = new List<Card>();
        public int Balance { get; set; }
        public string Name { get; set; }
        public bool isActivelyPlaying { get; set; }
        public bool Stay { get; set; }  // This would benefit from having a seperate player class for 21 players.
        public Guid Id { get; set; } //Example of a Guid.

        public bool Bet(int amount)
        {

            if (Balance - amount <= 0)
            {
                Console.WriteLine("You do not have enough to place a bet that size.");
                return false;
            }
            else
            {

                Balance -= amount; // Balance = Balance - amount;
                return true;

            }

        }
        public static Game operator +(Game game, Player player)
        {

            game.Players.Add(player); //This is a list and it must be instantiated
            return game;

        }
        public static Game operator -(Game game, Player player)
        {
            game.Players.Remove(player);
            return game;

        }

    }

}
//OVERLOAD OPPERATOR NOTES:
// this is the use of an operator (-, +, *, etc) to carry out a defined task using that operator.

    //GENERIC NOTES
//THESE ARE USED WITH A <T> to make the date type more broad. 
