using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Exercise_8_Classes_and_Objects
{
    class Program
    {
        static void Main(string[] args)
        {

        
            packet packet = new packet();
            bool sleep = false;
            Console.WriteLine("Hello! This porgram will ask you for a whole number. It will then perform 3 mathmatical methods on that number \nand return the answers. Press enter to start");
            Console.ReadLine();

            while (sleep == false)
            {

                Console.WriteLine("\nPlease enter a whole number or type exit to end the program.");

                string entered = Console.ReadLine();

                try
                { 

                if (entered == "exit")
                {
                    sleep = true;
                }

                else
                {
                    int enteredNum = Convert.ToInt32(entered);
                    packet.adding(enteredNum);
                    packet.subtract(enteredNum);
                    packet.multiply(enteredNum);


                }

                }
                 catch (Exception /*stix*/)
                {

                    Console.WriteLine("\nERROR \nPlease enter only whole numbers");
                    //Console.WriteLine(stix.Message);
                    //Console.ReadLine();
                }



               



            }


            Console.WriteLine("\nThank you for completing this program. Goodbye!");
            Console.ReadLine();

        }
    }
}
