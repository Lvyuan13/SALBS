using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("How many crew members?");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            int crewSize = Convert.ToInt32(Console.ReadLine());

            Crew crew = new Crew(crewSize);

            LifeSupportSystem lifeSupport = new LifeSupportSystem(crew);
            

        }
    }
}
