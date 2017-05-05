using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class VCDUrineProcessor : UrineProcessor
    {
        // Variables

        // Constants
        // power requirement figures obtained from:
        // "Spaceflight life supprt and biospherics" by Peter Eckart
        // power used is for urine + flushwater in flow of 32.65 [kg/day]
        private const double m_designPowerRequired = 0.115;       // [kW]
        private const double m_designLiquidInFlow = 32.65;    // [kg/day]
        
        // Functions

        public VCDUrineProcessor(Crew crewGiven)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("         ..Constructing Urine Processor..\n");
            Console.ForegroundColor = ConsoleColor.White;
            // Get the amount of urine produced by crew.
            Console.WriteLine("Crew urine: " + crewGiven.m_crewUrineProducedPerDay);
            Console.WriteLine("Crew flushwater: " + crewGiven.m_crewUrineFlushPerDay);

            double givenInFlow = crewGiven.m_crewUrineProducedPerDay + crewGiven.m_crewUrineFlushPerDay;
            double scaleFactor = givenInFlow / m_designLiquidInFlow;

            Console.WriteLine("Total Crew Liquid Given: " + Math.Round(givenInFlow,2));
            Console.WriteLine("Scale Factor: " + Math.Round(scaleFactor,2));

            m_powerRequired = scaleFactor * m_designPowerRequired;
            Console.WriteLine("VCD requires " + Math.Round(m_powerRequired,2) + " kW to process the urine\n");
        }
    }
}
