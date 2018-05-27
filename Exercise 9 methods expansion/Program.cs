using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_9_methods_expansion
{
    class Program
    {
        static void Main(string[] args)
        {

            addomerators mathMachine = new addomerators();

            Console.WriteLine("Enter a number bitch");
            int userInt = Convert.ToInt32(Console.ReadLine());

            mathMachine.original(userInt);

           Console.ReadLine();




        }
    }
}


//1. Create a class. In that class, create a method that will take in an integer, do a math operation to it and then 
//   return the answer as an integer.

//2. In the Main() method of the console app, instantiate the class and call the one method, passing in an integer. 3. Display the result to the screen.

//3. Add a second method to the class, with the same name, that will take in a decimal, do a different math
//   operation to it and then return the answer as an integer.

//4. In the Main() method of the console app, instantiate the class and call the second method, passing in a
//   decimal. Display the result to the screen.

//5. Add a third method to the class, with the same name, that will take in a string, convert it to an integer if 
//   possible, do a different math operation to it and then return the answer as an integer.

//6. In the Main() method of the console app, instantiate the class and call the third method, passing in a string
//   that equates to an integer.Display the result to the screen.