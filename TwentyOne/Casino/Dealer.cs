using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Casino
{
    public class Dealer
    {
        public string Name { get; set; }
        public Deck Deck { get; set; }
        public int Balance { get; set; }


        public void Deal(List<Card> Hand)
        {
            Hand.Add(Deck.Cards.First());
            string card = string.Format(Deck.Cards.First().ToString() + "\n");
            // Console.WriteLine(Deck.Cards.First().ToString() + "\n");
            Console.WriteLine(card);
            using (StreamWriter file = new StreamWriter(@"C:\Users\micha\OneDrive\Desktop\Logs\log.txt", true)) //If it were false it would create a new file.. This is automatically disposed of after use by the using statement.
            {

                file.WriteLine(DateTime.Now); //This gives the exact time of this moment. 
                file.WriteLine(card); //This is logging every card that has been dealt. You culd log tons of stuff

            }
            Deck.Cards.RemoveAt(0);


        }


    }

}




//YOU WOULDN'T INHERIT DECK HERE BECUASE INHARITANCE IS A 'IS A' RELATIONSHIP, NOT A 'HAS A' RELATIONSHIP. TWENTYONE IS A GAME A DEALER HAS A DECK.

    //SO INSTEAD ITS A CLASS PROPERTY
    //AIR ON THE SIDE OF PROPERTIES 