using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorsExpressionsStatements
{
    class Program
    {
        static void Main(string[] args)
        {
            int x, y, a, b;

            //Assignment Operator
            x = 3;
            y = 2;
            a = 1;
            b = 0;

            //There are many mathematical operators

            //Addition operator
            x = 3 + 4;

            //Subtration operator
            x = 4 - 3;
            
            //Multiplication operator
            x = 10 * 5;

            //division operator
            x = 10 / 5;


            //There are many operators used to evaluate values ...

            //Equality operator
            if (x == y)
            {
            }

            //grater than operator
            if (x > y)
            { 
            }

            //less than operator
            if (x < y)
            {
            }

            

            //conditional operator
            
            //conditional AND
            if ((x > y) && (a > b))
            { 
            }

            //conditional OR
            if ((x > y) || (a < b))
            { 
            }

            //also there is the in-line conditional operator
            string message = (x == 1) ? "Car" : "Boat";

            //method invocation operator
            Console.WriteLine("Hi");
        }
    }
}
