using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class LifeSupportSystem
    {

        public LifeSupportSystem(Crew crewGiven)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n..Constructing Life Support System..");
            Console.ForegroundColor = ConsoleColor.White;
            SabatierCO2Recycler base_SabatierCO2Recycler = new SabatierCO2Recycler(crewGiven);
            //Console.WriteLine("----");
            //BoschCO2Recycler BoschCO2 = new BoschCO2Recycler(crewGiven);
            H2OManager base_H2ORecycler = new H2OManager(crewGiven);
            O2Generator base_O2Generator = new O2Generator(crewGiven, base_H2ORecycler, base_SabatierCO2Recycler);

        }




    }
}
