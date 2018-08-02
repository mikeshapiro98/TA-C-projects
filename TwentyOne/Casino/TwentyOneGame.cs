using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Interfaces; //Adding this allowed me to make a subclass of interfaces. This helps to keep track. 

namespace Casino.TwentyOne
{
   public class TwentyOneGame : Game, IWalkAway  //THIS NEEDS TO EMPLOY THE ABSTACT METHOD //A class can only inherit from ONE other class. Otherwise you must use a class and an interface.
    {


        public TwentyOneDealer Dealer { get; set;  }

        public override void Play() //MUST BE IMPLIMENTED SINCE IT'S INHERITING FROM AN ABSTRACT CLASS. 
        {

            Dealer = new TwentyOneDealer();
            
            foreach (Player player in Players)
            {

                player.Hand = new List<Card>();
                player.Stay = false; 

            }
            Dealer.Hand = new List<Card>();
            Dealer.Stay = false;
            Dealer.Deck = new Deck();
            Dealer.Deck.Shuffle();


            bool validAnswer = false;
            int bet = 0;
            while (!validAnswer)
            {
                Console.WriteLine("Place your bet!");
                validAnswer = int.TryParse(Console.ReadLine(), out bet);
                if (!validAnswer /*|| bet <= 0 ~would work, but is less specific.~*/)
                {
                    Console.WriteLine("Wrong stupid! Try again. Whole numbers only, and no negatives.");
                }
                if (bet <= 0)
                {
                    throw new FraudException("Security! Kick this person out.");
                }

            }
            foreach (Player player in Players)
            {

                //int bet = Convert.ToInt32(Console.ReadLine());
                bool successfullyBet = player.Bet(bet);
                if (!successfullyBet)  //SAME AS IF SUCCESSFULLYBET == FALSE.
                {
                     return;
                }
                else
                {
                    Bets[player] = bet; //Player is the key here, bet is the value.
                    //player.Balance = player.Balance -= bet;
                    Console.WriteLine("Your new balance is {0}", player.Balance);
                }
              

            }
            for (int i =0 ; i < 2; i++)
            {
                Console.WriteLine("Dealing...");
                foreach(Player player in Players)
                {
                    Console.Write("{0}: ", player.Name);
                    Dealer.Deal(player.Hand);
                    if (i == 1)
                    {

                        bool blackJack = TwnetyOneRules.CheckForBlackJack(player.Hand);
                        if (blackJack)
                        {
                            
                            player.Balance += Convert.ToInt32((Bets[player] * 1.5) + Bets[player]);
                            Console.WriteLine("Blackjack! {0} wins {1}. Your balance is {2}", player.Name, Bets[player], player.Balance);
                            //Bets.Remove(player);
                            return; 

                        }

                        
                    }

                }
                Console.Write("Dealer: ");
                Dealer.Deal(Dealer.Hand);
                if (i == 1)
                {

                    bool blackJack = TwnetyOneRules.CheckForBlackJack(Dealer.Hand);
                    if (blackJack)
                    {
                        Console.WriteLine("Dealer has BlackJack! Everyone loses!");  //Remember that a dictionary is a list of key vlaue pairs. since it's a list you can iterate through it.
                        foreach (KeyValuePair<Player, int> entry in Bets)
                        {
                            Dealer.Balance += entry.Value;
                        }
                        return;
                    }
                    
                }

            }

            foreach (Player player in Players)
            {
                while (!player.Stay)
                {
                    Console.WriteLine("Your cards are:");

                    foreach (Card card in player.Hand)
                    {
                        Console.Write("{0}", card.ToString());
                    }
                    Console.WriteLine("\n\nHit or stay?");


                    string answer = Console.ReadLine().ToLower();

                    if (answer == "stay")
                    {
                        player.Stay = true;
                        break;
                    }
                    else if (answer == "hit")
                    {
                        Dealer.Deal(player.Hand);
                    }

                    bool busted = TwnetyOneRules.IsBusted(player.Hand);

                    if (busted)
                    {
                        Dealer.Balance += Bets[player];
                        Console.WriteLine("{0} Busted! You lose your bet of {1}. Your balance is now {2}", player.Name, Bets[player], player.Balance);
                        Console.WriteLine("Your balance is now {0}", player.Balance);
                        Console.WriteLine("Do you want to play again?");
                        answer = Console.ReadLine().ToLower();
                        if (answer == "yes" || answer == "yeah")
                        {
                            player.isActivelyPlaying = true;
                            player.Stay = true;
                            return;
                        }
                        else
                        {
                            player.isActivelyPlaying = false;
                            return;
                        }
                    }      
                }
            }

            Dealer.isBusted = TwnetyOneRules.IsBusted(Dealer.Hand);
            Dealer.Stay = TwnetyOneRules.ShouldDealerStay(Dealer.Hand);

            while (!Dealer.Stay && !Dealer.isBusted)
            {
                Console.WriteLine("Dealer is hitting.");
                Dealer.Deal(Dealer.Hand);
                Dealer.isBusted = TwnetyOneRules.IsBusted(Dealer.Hand);
                Dealer.Stay = TwnetyOneRules.ShouldDealerStay(Dealer.Hand);


            }
            if(Dealer.Stay)
            {
                Console.WriteLine("Dealer is staying.");
            }
            if (Dealer.isBusted)
            {
                Console.Write("Dealer busted!");
                foreach(KeyValuePair<Player, int> entry in Bets)
                {
                    Console.WriteLine("{0} won {1}!", entry.Key.Name, entry.Value); //This is how to assign key value pairs. 
                    Players.Where(x => x.Name == entry.Key.Name).First().Balance += (entry.Value * 2);  //Where always produces a list.
                    Dealer.Balance -= entry.Value;
                }
               // return;
            }
            foreach (Player player in Players) //bool is a struct meaning it can never be null, however if it could, you could have 3 options: True, False, Or Null. 
            {

                bool? playerWon = TwnetyOneRules.CompareHands(player.Hand, Dealer.Hand);  //The question mark allows you to have a third null value. 

                if (playerWon == null)
                {
                    Console.WriteLine("Push! No one wins.");
                    player.Balance += Bets[player];
                }

                else if (playerWon == true)
                {

                   
                    player.Balance = player.Balance + (Bets[player] * 2);
                    
                    Dealer.Balance -= Bets[player];
                    Console.WriteLine("{0} won {1}! Your balance is now {2}", player.Name, Bets[player], player.Balance);
                    Console.WriteLine("Your balance is now {0}", player.Balance);
                }

                else
                {

                    Dealer.Balance += Bets[player];
                    Console.WriteLine("Dealer wins {0}! Your balance is now {1}", Bets[player], player.Balance);
                }


             


            }

            foreach (Player player in Players)
            {
                Console.WriteLine("Want to play again?");
                string answer = Console.ReadLine().ToLower();
                if (answer == "yes" || answer == "yeah")
                {
                    player.isActivelyPlaying = true;
                    player.Stay = true;
                    return;
                }
                else
                {
                    player.isActivelyPlaying = false;
                    return;
                }
            }

        }

        public override void ListPlayers()
        {
            Console.WriteLine("21 Players");
            base.ListPlayers();
        }


        public void WalkAway(Player player)
        {
            throw new NotImplementedException();

        }

    }
}

//Dictionary is a data type containing a collection of key value pairs
////To check for BlackJack we're going to need to get the value of a hand. We also need to know the value of each card. 
//// A layer is simply a class that exists outside of the other classes. The other classes have now knowledge of it. We are making a business logic layer called 21 rules.