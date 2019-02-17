using System;
using System.Collections.Generic;// added when making a new dictionary.
using Casino;
using Casino.TwentyOne; //this was automatically added when we changed the 21 game.
using System.IO;
using System.Data.SqlClient; //ADDED FOR QUERIES
using System.Data; //Added for SQL data type


//using System.Collections.Generic;
//Using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO
//using TwentyOne.BaseClasses; //NOTICE I AM USING A BASE CLASS THAT WAS CHANGED IN GAME TO THIS NAMESPACE. Now it works after adding this line of code. but now ther things have issuses. This exemplifies using namespace. 

namespace TwentyOne
{
    class Program
    {
        //private static object sqlDbType;

        static void Main(string[] args)
        {
            const string casinoName = "Grand Hotel and Casino"; //this is an example of a constant. (cannot be changed). It won't make a difference in this code, but if we wanted to use this variable a lot with a fear of it being changed, that is when this is useful. If someone else changed the code of a constant that was used frequently it would screw up the whole program. 
                                                                // While this program is running, this var wll never change.
            //WELCOME MESSAGE
            System.Console.WriteLine("Hello! Welcome to the {0}. Let's start by telling me your name cowboy", casinoName); //If you tool using System out, you could use the whole path here and it would still work. You could also use using System.Console, and remove the console. part.
            string playerName = Console.ReadLine();
            if (playerName.ToLower() == "admin")
            {

                List<ExceptionEntity> Exceptions = ReadExceptions();
                foreach (var exception in Exceptions)
                {
                    Console.Write(exception.Id + " | ");
                    Console.Write(exception.ExceptionType + " | ");
                    Console.Write(exception.ExceptionMessage + " | ");
                    Console.Write(exception.TimeStamp + " | ");
                    Console.WriteLine();
                    
                }
                Console.Read();
                return; 

            }
            bool validAnswer = false;
            int bank = 0;
            while (!validAnswer)
            {
                Console.WriteLine("How much money did you bring today?");
                validAnswer = int.TryParse(Console.ReadLine(), out bank);
                if (!validAnswer || bank <= 0)
                {
                    Console.WriteLine("Wrong stupid! Try again. Whole numbers only and no negatives.");
                }
            }
            Console.WriteLine("Excellent {0}! Would you like to play a game right now?", playerName);
            string answer = Console.ReadLine().ToLower();

            if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "okay" || answer == "sure" || answer == "ya")
            {

                Player player = new Player(playerName, bank);   //CONSTRUCTOR
                player.Id = Guid.NewGuid();         //GUID //Thiink username and password. No one else can be this person
                using (StreamWriter file = new StreamWriter(@"C:\Users\micha\OneDrive\Desktop\Logs\id.txt", true)) //If it were false it would create a new file.. This is automatically disposed of after use by the using statement.
                { //Dont forget to use system IO
                    //Don't forget to rebuild the library
                    file.WriteLine(player.Id); //This will log the player ID to the log file on the computer.  


                }
                Game game = new TwentyOneGame();       //POLYMORPHISM
                game += player;
                player.isActivelyPlaying = true;
                while (player.isActivelyPlaying && player.Balance > 0)
                {
                    try
                    {
                        game.Play();
                    }
                    catch (FraudException ex) //This will only get hit where you specified that it was an Argument Exception. We then named it
                    {
                        Console.WriteLine(ex.Message);
                        UpdateDbWithException(ex);
                        Console.ReadLine();

                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There was an error in the program. Please contact your system administrator. ");
                        UpdateDbWithException(ex);
                        Console.ReadLine();

                        return;
                    }

                }

                game -= player;
                Console.WriteLine("Thanks for playing cowboy.");


            }


            Console.WriteLine("Feel free to look around the casino. Bye for now.");
            Console.ReadLine();
        }


        private static void UpdateDbWithException(Exception ex)   ///You can do this process for more than just inserting. You can do it for read, delete, update, etc.
        {

            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwnetyOneGame;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string queryString = @"INSERT INTO Exceptions(ExceptionType, ExceptionMessage, TimeStamp) VALUES  
                                             (@ExceptionType, @ExceptionMessage, @TimeStamp)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@ExceptionType", SqlDbType.VarChar);
                command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar);
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime);

                command.Parameters["@ExceptionType"].Value = ex.GetType().ToString();  
                command.Parameters["@ExceptionMessage"].Value = ex.Message; 
                command.Parameters["@TimeStamp"].Value = DateTime.Now;

                //This is how we will sned it to the database. 

                connection.Open();   
                command.ExecuteNonQuery();   
                connection.Close();

                //Now we'll go back to main and call this method on each exception. 
                //You have classes that map closely or better to your database. 
                //
            }


        }

        private static List<ExceptionEntity> ReadExceptions()  //This will be telling the program to print out a list of the exceptions. 
        {

            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwnetyOneGame;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"; //Same as the one above. Sometimes you'll use a method to call information from your configuration. 
            //If your connection string changes, you will have to change it everywhere in your program. 
            //In bigger apps sometimes you won't have a connections string like this and it will instead be in the configuration file. 

            string queryString = @"Select Id, ExceptionType, ExceptionMessage, TimeStamp From Exceptions" /*THIS IS AN EXAMPLE OF PASSING IN A PERAMETER. where Id > @number"*/;

            List<ExceptionEntity> Exceptions = new List<ExceptionEntity>();

            using (SqlConnection connection = new SqlConnection(connectionString)) //Remember to use 'using' in order to ensure it gets closed.
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();   /////////OPENING CONNECTION//////////

                SqlDataReader reader = command.ExecuteReader();  //BEFORE WE EXECUTED A NON QUERY, HERE WE ARE EXECUTING A READER AND ASSIGNING IT TO A READER OBJECT. 

                while (reader.Read())  //this loops through each record that you're getting back and creats a new object.  then we will map what we care getting back. 
                {

                    ExceptionEntity exception = new ExceptionEntity();
                    exception.Id = Convert.ToInt32(reader["Id"]);
                    exception.ExceptionType = reader["ExceptionType"].ToString();
                    exception.ExceptionMessage = reader["ExceptionMessage"].ToString();
                    exception.TimeStamp = Convert.ToDateTime(reader["TimeStamp"]);
                    Exceptions.Add(exception);

                    //everytihng inside the curly braces is confined to those which is why the list was declared outside of them
                }
                connection.Close();    /////////CLOSING CONNECTION//////////


            }

            return Exceptions;



        }




    }

}

/*********************************************************************SQL IN C#****************************************************************************************/
//private static void UpdateDbWithException(Exception ex) //Remember all exception inherit from exception so this could take a fraud exception making it able to take both. It's taking in an exception. POLYMORPHISM!
//                                                        //This is a void and so does not return anything but often if you're adding to a database you'll want to return a bool or something else. (ex: bool isUpdated) 
//                                                        //This method is using ADO
//{

//    //First we need a connection string(long) that contains info about the database instance your trying to connect to(UN & Pass, Were it is(location), how to access it, etc.) . You always need this to connect to a database. 
//    string connectionString = @"Data Source = (localdb)\ProjectsV13; Initial Catalog = TwnetyOneGame; Integrated Security = True; Connect Timeout = 30; Encrypt = False; 
//                                        TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
//    //Now that this exists, we need to get the string from the SQL server: View -> SQL Server -> DB properties
//    //This was coppies from the properties area (Connection String of the DB)

//    //This is a query string  //You can't just directly insert the data users adds to a form becuase it could cause irreversible damage to your DB. They could enter drop database to it. this will automatically create a new code.
//    //To solve this we will do perametered queries
//    string queryString = @"INSERT INTO Exceptions(ExceptionType, ExceptionMessage, TimeStamp) VALUES  
//                                 (@ExceptionType, @ ExceptionMessage, @TimeStamp)";  //WHen we go to runt eh queries these will be repaced. //These are the place holders. It's saying these have to be these data types which avoids SQL injections. 

//    using (SqlConnection connection = new SqlConnection(connectionString))  //USING IS A WAY OF CONTROLLING UNMANAGED CODE OR RECOURCES. we are passing in connection string. 
//    {
//        SqlCommand command = new SqlCommand(queryString, connection);
//        command.Parameters.Add("@ExceptionType", SqlDbType.VarChar);   //This is declaring the datatype. Remember Varchar is basically a string
//        command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar);
//        command.Parameters.Add("@TimeStamp", SqlDbType.DateTime);

////      command.Parameters["@ExceptionType"].Value = ex.GetType().ToString();    //GetType is a method that gives you the data type of what you're working with. Very useful at times. 
////It returns a datatype "type" not a "string" so it needs to be converted usting ToString(). 
//        command.Parameters["@ExceptionMessage"].Value = ex.Message;               //Remeber ex is the object. 
//                command.Parameters["@TimeStamp"].Value = DateTime.Now;

//This is how we will sned it to the database. 

//                connection.Open();            //This opens the conncetion
//                command.ExecuteNonQuery();    //This executes the command. This is a nonquery becuase we aren't looking for a specific query (select query)
//                connection.Close();

//                //Now we'll go back to main and call this method on each exception.


//}












//NOTES ON DATABASES AND ACCESS(SQL)
//SQL and C# are NOT the sae thing. Each should be treated appropriately. 
//When you want to access a SQL database from you C#(insert a string, pull record/map it, etc), you must ADO.NET
//ADO.NET is an obstraction layer for communicating with a database. You can write SQL directly into C# and execute them within the targeted database, then return results back to the application.
// !PATH! - View -> SQL server object explorer


//MORE NOTES ON EXCEPTIONS. 
//TRY CATCH IS NOT THE ONLY METHOD. YOU CAN ALSO USE TRYPARSE AND A WHILE LOOP
//USE A TIGHT WHILE LOOP TO KEEP TO ONE ANSWER. 
//TO HANDLE THE PROBLEM OF USERS PUTTING IN NEGATIVE NUMBERS AND GAINING MONEY IF THEY LOSE, YOU COULD EITHER USE AN IF(X <=0) OR YOU COULD USE A TRY CATCH BLOCK.
//YOU CAN EVEN PUT THE WHOLE PLAY METHOD IN A TRY CATCH BLOCK TO HAND ANY AND ALL ERRORS THAT HAPPEN WITHIN THAT METHOD.    
//IT'S GOOD TO BE AS SPECIFIC AS POSSIBLE WHEN CATCHING EXCEPTIONS, THAT WAY YOU CAN HANDLE THEM APPROPRIATELY.
//CATCHES ARE DONE IN ORDER IF YOU STACK THEM. 
//START WITH MORE SPECIFIC EXCEPTIONS ABOVE AND MORE SPECIFIC ONES BELOW.
//YOU CAN ALSO CREATE YOUR OWN EXCEPTIONS. SEE FRAUD EXCEPTION CLASS
//ALL EXCEPTIONS INHERIT FROM BASE EXEPTION. (Polymorphism)


//NOTES ON GUIDS   
//THERES A CLASS IN c# THAT ALLOWS YOU TO CREAT A 'GLOBAL UNIQUE IDENTIFIER' (GUID)
//The advantage of this is that it can identify specific things (especiaally people)
//You could look someone up by name using their GUID. 
//You can create as many GUIDs as you want. The odds of getting the same GUID is unlikely because of the way they're built. 
//EXAMPLE:
//Guid identifier = Guid.NewGuid();  //basically you can pass in values and the algorythm uses those values to creat a GUID. Note the syntax looking different than other instansiations. SQL even has a GUID method
//They are always unique. 


//NOTES ON CREATING A CONSTANT
// DECLARING A CONTANT JUST MEANS THAT IT CAN'T CHANGE GIVING YOU MORE CONTROLL. 
//



//NOTES ON DECLARING JUST A VAR AS A VARIABLE
//Rule of thumb: If the data tyoe is ever in question. DECLARE THE DATA TYPE for readability by the human. 
//This should only be used if the data type is obvious.
//Harder way of declaring this variable.
//Dictionary <string, string> newDictionary = new Dictionary<string, string>();
//Easier way
//var newDictionary = new Dictionary<string, string>();




//NOTES ON CONSTRUCTOR CALL CHAIN
//A CONSTRUCTOR CALL CHAIN IS A WAY TO DEFINE MULTIPLE CONSTRUCTORS AND UTILIZE EARLIER ONES TO PREVENT WRTING THINGS MORE THAN ONCE.
//THIS IS ALMOST INHERITING THE PROPERTIES FROM ONE CONSTRUCTOR INTO ANOTHER.

//NOTES ON LIBRARIES
//Everything but the program in 21 was added to a new project/namespace called casino. 
//A referance was then added to the 21 game (The casino) and it is pulling data from the new namspace (Library). 
//This gives almost a template for someone to create off of. 
//This can be thought of as layers of a project.
//Casino here is a library of things we could use.
//Anytime you change the library, you have to REBUILD IT!!!!!!!


//ACCESSABILITY MODIFIERS 
//PUBLIC, PRIVATE, ETC
//PUBLIC MEANS YOU CAN ACCESS IT FROM ANYWHERE.
//PRIVATE MEANS YOU CAN ONLY ACCESS IT WITHIN THE CLASS IT'S WITHIN
//PRIVATE AND PUBLIC ARE THE MOST COMMON. 
//PROTECTED MEANS IT'S LIMITED TO JUST THAT CLASS AND ANY THAT DERIVE FROM IT
//INTERAL MEANS THAT THESE METHODS CAN ONLY BE ACCESSED TO METHODS OF THE SAME ASSEMBLY. (IN THIS CASE ONLY CLASSES WITHIN CASINO CAN ACCESS IT AND NOT THE TWENTYONE GAME)
//THESE ALL HELP TO CONTROL YOUR CODE. USE THE ONE THAT BEST FITS YOU NEED. DON'T JUST MAKE EVERYTHING PUBLIC
//

//NOTES ON ASSEMBLY
//.NET FRAMEWORK MAKES ANY CODE YOU WRITE COMPILE IT TWICE. IT WILL BE COMPILED INTO A INTERMEDIATE LANGUAGE USUALLY A .DLL OR .EXC. IT IS IN ESSANCE AN ESSEMBLY. IT THEN GETS COMPILED INTO WHATEVER MACHINE CODE IT WILL BE WORKING WITH BY THAT COMPUTER IT RUNS ON
//ASSEMBLY IS WHAT YOU GET AFTER YOU COMPILED YOUR c# CODE INTO INTERMEDIATE LANGUAGE.
//THIS WILL COMIPLE INTO A .EXC AND THAT WOULD BE AN ASSEMBLY.
//LIST IS PART OF THE .NET FRAMEWORK. IT'S AN ASSEMBLY.


//NOTES ON NAMESPACES.
//NAMESPACESES ARE WAYS OR ORGANIZING YOUR CODE. 
//YOU COULD HAVE ONE CLASS CALLED CELL AND ONE CALLED PRODUCT.
//IMAGINE YOUR AMAZON. YOU CAN ONLY HAVE ONE CLASS CALLED CHAIR, BUT SEVERAL NAMESPACES CALLED "LAZYBOY", "ROCKER", ETC
//


//NOTES ON FILE I/O:
//THIS IS USED TO PRINT SOMETHING TO A TEXT DOCUMENT OR FILE.
//A broad use is logging
//Logging means putting an entry into a file or database based on an action of the program.
//We will be adding logging to every card dealt. 


//NOTES ON LAMDA EXPRESSIONS
//1. This helps to shorten certain tasks like incrementation.
//2. Saves time.
//3. Format "x" in the above example could be anything. You can give it whatever title you'd like. 
//4. => is a distinct lamda expression that that essentially means to evaluate this to each item.
//5. They are very hard to debug compared to say a for loop. You can't go line by line through a Lambda expression. Especially when chaining them together.
//6. It can also make it hard to read. 



//NOTES ON STRUCTS 
//1. Every data type in C# is either a refernce type or a value type.
//2. Think of if you submitted an essay to your teacher and instead of turning it in, you put it on Google Drive so that when she made changes you could see it too. It's a solo address with 2 manipulators. It's the same document even though it seems different. 
//3. Any data type that stores value by referance is a referance data type. This includes all classes.
//4. In the example in number 2 the copy of the assignment vs. putting it on Google Drive would be a value type not a referance type.
//5. Structs are essentially classes except it's a value type rather than a referance type.
//6. Structs CANT be inherited.
//7. Value types cannot have a value of null because it is not a value

//REFERANCE TYPE:
//string
//card
//list


//VALUE TYPES: 
//int
//bool
//enums
//datetime

//NOTES THAT HAVE BEEN DELETED FROM PROGRAM

// var newPlayer = new Player("Jesse"); //AFTER ADDING THIS THE LIBRARY NEEDED TO BE REBUILT BEFORE IT RAN SUCCESSFULLY.


//  Console.WriteLine("How much money did you bring today?");
// int bank = Convert.ToInt32(Console.ReadLine());


//DateTime dateTime = new DateTime(1995, 5, 23, 8, 32, 45);  //DATE TIMES ARE STRUCTS(Value type) SO CANT BE NULL
//DateTime yearOfBirth = new DateTime(1995, 5, 23, 8, 32, 45);  //DATE TIMES ARE STRUCTS(Value type) SO CANT BE NULL
//DateTime yearOfGraduation = new DateTime(2013, 6, 1, 16, 34, 22);
//TimeSpan ageAtGraduation = yearOfGraduation - yearOfBirth;


//string text = "Here is some text";
//File.WriteAllText(@"C:\Users\micha\OneDrive\Desktop\Logs\log.txt", text); //The @ is an escape character which means read everything literally. otherwise you need a double back slash 
// string text = File.ReadAllText(@"C:\Users\micha\OneDrive\Desktop\Logs\log.txt");

//Deck deck = new Deck();

////deck.Shuffle(3);


////foreach (Card card in deck.Cards)
////{
////    Console.WriteLine(card.Face + " of " + card.Suit);

////}
////Console.WriteLine(deck.Cards.Count);
//Console.ReadLine();



// int count = deck.Cards.Count(x => x.Face == Face.Ace);     //LAMDA EXPRESSION EXAMPLE   X represents elements and could be called anything
//Think of this as count the elements of x if Face == Ace.
//List<Card> newList = deck.Cards.Where(x => x.Face == Face.King).ToList();
//this line should create a new list of just kings.
//   Where allows you to filter lists for specific things within it.    

//List<int> numberList = new List<int>() { 1, 2, 3, 535, 342, 23 };
//int sum = numberList.Sum(x => x + 5); //Same as sum + 5 for every number in the list (foreach (item in list  add 5))

//int sum = numberList.Max(); //Gets the highest number in the list
//int sum = numberList.Min(); // Gets the lowest number in the list.
//  are you getting the picture as to why this is easier?
//int sum = numberList.Where(X => X > 20).Sum(); //This says to creat a new list if the numbers in the referance list are over 20 and then add the sums together. 
//where creats a new lsit. //USING LAMDA EXPRESSION TO CONCATINATE METHODS TOGETHER.
//Console.WriteLine(sum);
//foreach (Card card in numberList)
//{ 
//    Console.WriteLine(Card.Face);
//}  




//Card Card1 = new Card(); //THIS EXEMPLIFYS STRUCT. It didn't make a new card when it was assigned to card 2 it only chaged its name. It's the same block of memory. this gives card 2 the address of card 1. 
//Card Card2 = Card1;
//Card1.Face = Face.Eight;
//            Card2.Face = Face.King; //This wouldn't work with a string because a string is immutable so can;t be changed. Only recreated.  
//            //AFTER MAKING THE CARD CLASS A STRUCT, the value changed from King to Eight because the struct is a value. Card 1 and card 2 are now completely seperate. 

//            Console.WriteLine(Card2.Face);



//Card card = new Card();
//card.Suit = Suit.Clubs;
//int underlyingValue = (int)Suit.Diamonds; //COULD JUST AS EASILY DO CONVERT.TOINT32 (SUIT.DIAMONDS);
//Console.WriteLine(underlyingValue);



//Player<Card> player = new Player<Card>();
//player.Hand = new List<Card>();


//Game game = new TwentyOneGame();
//game.Players = new List<Player>();
//            Player player = new Player();
//player.Name = "Jesse";
//            game += player;  //game = game + player;
//            game -= player; //game = game - player;




//TwentyOneGame game = new TwentyOneGame();
//game.Players = new List<string>() { "Jesse", "Bill", "Bob" };
//game.ListPlayers();
//Console.ReadLine();

//ABSTRACT CLASSES NOTES
//Abstract essentially means that the class cannot ever be instantiated. It can only be inherited from.
//Game game = new Game(); //THIS CANNOT BE DONE BECAUSE GAME IS NOW ABSTRACT


//POLYMORPHISM NOTES 
//List<Game> games = new List<Game>();
//Game game = new TwentyOneGame(); //THIS IS POLYMORPHISM!
//games.Add(game);
//THIS IS USEFUL FOR IT THERE WERE MULTIPLE GAMES 
// PokerGame
// SolitaireGame 
// IMAGINE PUTTING ALL OF THE GAMES IN A LIST. 
// A LIST CAN ONLY TAKE ONE DATA TYPE. THIS ALLOWS FOR ALL OF THE GAMES TO BE LISTED AS "GAMES"
// FOR EXAMPLE:
// List<Game> games = new List<Game>();
// Game game = new TwentyOneGame();
// games.Add(game); 
// POLYMORPHISM IS THE ABILITY OF A CLASS TO MORPH INTO ITS INHERITING CLASS WHICH GIVES CERTAIN ADVANTAGES.


// Card card = new Card() { Face = "King", Suit = "Spades" }; ANOTHER WAY TO INTIALIZE A CLASS WITH A ASSIGNED VALUE

//TwentyOneGame game = new TwentyOneGame();
//game.Players = new List<string>() { "Jesse", "Bill", "Joe" };
//game.ListPlayers();
//game.Play();
//Console.ReadLine();
//WHEN YOU CALL FOR A METHOD FROM A CLASS THAT YOU'RE INHERITING FROM, IT IS CALLED A SUPER CLASS METHOD.



//Game game = new Game();
//game.Dealer = "Jesse";
//game.Name = "TwentyOne";



//int timesShuffled = 0;
// deck = Shuffle(deck);
// deck = Shuffle(deck: deck, times: 3); //deck: and times: are optional for readability
//deck = Shuffle(deck, out timesShuffled, 3);

//Console.WriteLine("Times Shuffled: {0}", timesShuffled);

//Console.WriteLine(deck.Cards[0].Face + " of " + deck.Cards[0].Suit);
//Console.WriteLine(deck.Cards[1].Face + " of " + deck.Cards[1].Suit);
//Console.WriteLine(deck.Cards[2].Face + " of " + deck.Cards[2].Suit);
//Console.WriteLine(deck.Cards[3].Face + " of " + deck.Cards[3].Suit);
//Console.WriteLine(deck.Cards[4].Face + " of " + deck.Cards[4].Suit);
//Console.WriteLine(deck.Cards[5].Face + " of " + deck.Cards[5].Suit);
//Console.WriteLine(deck.Cards[6].Face + " of " + deck.Cards[6].Suit);
//Console.WriteLine(deck.Cards[7].Face + " of " + deck.Cards[7].Suit);



//deck.Cards = new List<Card>();



//Card cardOne = new Card();
//cardOne.Face = "Queen";
//cardOne.Suit = "Spades";

//deck.Cards.Add(cardOne);

//Console.WriteLine(cardOne.Face + " of " + cardOne.Suit);



//public static Deck Shuffle(Deck deck, int times)
//{

//    for (int i =0; i<times; i++)
//    {
//        deck = Shuffle(deck);

//    }
//    return deck;

//}