using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variables
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            int x;
            int y;

            x = 7;
            y = x + 3;
            Console.WriteLine(y);
            */

//            string myFirstName;
//            myFirstName = "Dan-Joe";

//            string myFirstName = "Dan-Joe";

//            var myFirstName = "Dan-Joe";
//            var myFirstName = "";

//            string myfirstname;
//            myfirstname = "SomeGuy";

//            Console.WriteLine(myFirstName);

            int x = 7;
            string y = "bob";
            string yNum = "5";
            string myFirstTry = x + y;              //implicit string conversion
            string myNextTry = x.ToString() + y;    //explicit string conversion

//            int mySecondTry = x + y;                //error because you cannot implicitly convert string to int
//            int mySecondtry = x + int.Parse(y);     //Compile error: "Bob" cannot be converted to a number
            int mySecondtry = x + int.Parse(yNum);

            Console.WriteLine(myFirstTry);
            Console.WriteLine(mySecondtry);
            Console.ReadLine();
        }
    }
}
