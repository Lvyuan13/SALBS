using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class O2Generator
    {
        // Variables

  
        // Constants

        // Values from "Advancing the Oxygen Generation Assembly Design to
        // Increase Reliability and Reduce Costs for a Future Long
        // Duration Mission" - Kevin C. Takada, Ahmed E. Ghariani and Steven Van Keuren.
        // assume the 
        private const double m_maxPower = 3955;                 // [W]
        private const double m_standbyPower = 497;              // [W]
        private const double m_maxO2PerDay = 9.2;               // [kg/day]
        private const double m_standbyO2PerDay = 2.3;           // [kg/day]
        private const double m_massO2toH2O = 1.125;             // [ (2 * 18) / 32 ] [kgH2O/day]
        private const double m_massH2OtoO2 = 0.88888889;        // [ 32 / (2 * 18 ]) [kgO2/day]


        // Functions

        // constructor takes in H2O produced in a 24 hour period
        public O2Generator(Crew crewGiven, H2OManager H2ORecyclerGiven, CO2Recycler CO2RecyclerGiven)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("     ..Constructing O2 Generator..");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("You gave me " + crewGiven.m_crewSize + " crew members" );
            Console.WriteLine("the CO2 recycler produces " + Math.Round(CO2RecyclerGiven.m_H2OProduced24Hrs,2) + " kg/day of H2O");
            Console.WriteLine("O2 production of crew is: " + Math.Round(crewGiven.m_crewO2ConsumptionPerDay,2) + " kg/day");

            double massO2Created = CO2RecyclerGiven.m_H2OProduced24Hrs * m_massH2OtoO2;
            Console.WriteLine("This means we produce " + Math.Round(massO2Created, 2) + " kg/day of O2");
            



        }

    }
}
