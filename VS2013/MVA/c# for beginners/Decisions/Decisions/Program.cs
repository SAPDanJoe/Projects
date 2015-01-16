using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions
{
    class Program
    {
        static void Main(string[] args)
        {
/*            Console.WriteLine("Enter a vaulue:");
            string userValue;
            userValue = Console.ReadLine();
            Console.WriteLine("You typed: " + userValue);
*/
            Console.WriteLine("Would you prefer what is behind door number 1, 2, or 3?");
            string userDoor = Console.ReadLine();

            //string message = "";
  
            //if (userDoor=="1")
            //    message = "You won a new car!!";
            //else if(userDoor =="2")
            //    message = "You Won a new boat!!";
            //else if (userDoor == "3")
            //    message = "You Won a new cat!!";
            //else
            //    message = "Sorry, "+ userDoor +"is not a door!  You Lose.";

            //Console.WriteLine(message);

            string message = (userDoor == "1") ? "boat" : "strand of lint";
            Console.WriteLine("You won a {0}", message);
            Console.ReadLine();

            


int userValue = 2;
string message2 = (userValue == 1) ? "boat" : "car";
Console.WriteLine("{1} - {0}", userValue, message2);
 
        }
    }
}
