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
        const double m_maxPower = 3955;                 // [W]
        const double m_standbyPower = 497;              // [W]
        const double m_maxO2PerDay = 9.2;               // [kg/day]
        const double m_standbyO2PerDay = 2.3;           // [kg/day]

        // Functions

        // constructor takes in H2O produced in a 24 hour period
        public O2Generator(Crew crewGiven, H2ORecycler H2ORecyclerGiven, CO2Recycler CO2RecyclerGiven)
        {
            Console.WriteLine("     ..Constructing O2 Generator..\n");
            Console.WriteLine("You gave me " + crewGiven.m_crewSize + " crew members" );
            Console.WriteLine("the CO2 recycler produces " + Math.Round(CO2RecyclerGiven.m_H2OProduced24Hrs,2) + " kg/day of H2O");
            Console.WriteLine("O2 production of crew is: " + Math.Round(crewGiven.m_crewO2ConsumptionPerDay,2) + " kg/day");
            //CO2RecyclerGiven.
            // TODO I need to know how much water is required to produce the oxygen rates stated

        }

    }
}
