using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.TwentyOne
{
    public class TwnetyOneRules
    {

        //FIRST WE WILL MAKE A PRIVATE DICTIONARY OF CARD VALUES
        private static Dictionary<Face, int> _cardValues = new Dictionary<Face, int>()
        {

            [Face.Two] = 2,
            [Face.Three] = 3,
            [Face.Four] = 4,
            [Face.Five] = 5,
            [Face.Six] = 6,
            [Face.Seven] = 7,
            [Face.Eight] = 8,
            [Face.Nine] = 9,
            [Face.Ten] = 10,
            [Face.Jack] = 10,
            [Face.Queen] = 10,
            [Face.King] = 10,
            [Face.Ace] = 1  //Can't asign this 2 values, but will do logic later to give this a second value. 


        };

        private static int[] GetAllPossibleHandValues (List<Card> Hand)
        {

            int aceCount = Hand.Count(x => x.Face == Face.Ace);  //Lambda expressions are methods you can perform on lists.
            int[] result = new int[aceCount + 1];   // Remember with an array you have to state how many elements there are inside of them. (How big it is)
            int value = Hand.Sum(x => _cardValues[x.Face]);

            result[0] = value;

            if (result.Length == 1) return result; //NOTE THE NO CURLY BRACES.
            //{
            //    return result; 
            //}

            for (int i = 1; i < result.Length; i++)
            {

                value +=  (i * 10);    //value = value + (i * 10);
                result[i] = value;

            }
            return result;

        }
        public static bool CheckForBlackJack(List<Card> Hand)
        {
            int[] possibleValues = GetAllPossibleHandValues(Hand);
            int value = possibleValues.Max();
            if (value == 21) return true;
            else return false;
        }

        public static bool IsBusted(List<Card> Hand)
        {
            int value = GetAllPossibleHandValues(Hand).Min();
            if (value > 21) return true;
            else return false;
        }


        public static bool ShouldDealerStay(List<Card> Hand)
        {
            int[] possibleHandValues = GetAllPossibleHandValues(Hand);
            foreach (int value in possibleHandValues)
            {
                if(value > 16 && value < 22)
                {
                    return true;
                }
                
            }
            return false;
        }

        public static bool? CompareHands(List<Card> PlayerHand, List<Card> DealerHand)
        {
            int[] playerResults = GetAllPossibleHandValues(PlayerHand);
            int[] dealerResults = GetAllPossibleHandValues(DealerHand);

            int playerScore = playerResults.Where(x => x < 22).Max();
            int dealerScore = dealerResults.Where(x => x < 22).Max();

            if (playerScore < dealerScore) return false;
            else if (playerScore > dealerScore) return true;
            else return null; 
        }

    }
}
//This class is to give perameters. 
//Jesse has done this before. Don't let that intimidate you. He has already put the thought into it. 

//Private is used if something is only used within its own class. Marking it private is a good habit. 
//Public might cause conflict with similar names. static is so we don't have to create another 21 object.  
//if a class or method is static, that means it's called by the type itself, and not by an instance of the type.
// Face is the key here. int is the value.
// A naming convention in private casses is to use an underscore ahead of it.