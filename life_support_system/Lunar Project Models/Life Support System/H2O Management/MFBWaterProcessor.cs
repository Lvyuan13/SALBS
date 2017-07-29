using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class MFBWaterProcessor : WaterProcessor
    {
        // Variables

        // Constants
        // Figures obtained from:
        // "Spaceflight life supprt and biospherics" by Peter Eckart
        // The multifiltration beds need replacing every 15 days..
        private const double m_designPower = 0.00038;       // [kW]
        private const double m_designInflow = 114;         // [kg / day]

        // Functions
        // From "International Space Station Water Balance Operations:"
        // TODO the water processor wil need to process additional water including from: Condensate water(sweat waste food etc.), from the sabatier reactor, rejected OGA water.
        public MFBWaterProcessor(Crew crewGiven, UrineProcessor UPAGiven)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("         ..Constructing MFB Water Processor..\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Urine from urine processing = " + UPAGiven.m_urineProcessedPerDay + " [kg/day]");

            Console.WriteLine("Total crew wash water: " + crewGiven.m_crewHygieneH2OUsedPerDay + " [kg/day]");



            double totalH20Inflow = UPAGiven.m_urineProcessedPerDay + crewGiven.m_crewHygieneH2OUsedPerDay;

            Console.WriteLine("Total inflow H2O = " + Math.Round(totalH20Inflow,2));

            double scaleFactor = totalH20Inflow / m_designInflow;

            Console.WriteLine("Scale Factor = " + Math.Round(scaleFactor,3));

            m_powerRequired = scaleFactor * m_designPower;

            Console.WriteLine("MFB Water Processor Power Required: " + Math.Round(m_powerRequired,3) + " kW (" + m_powerRequired + ") kW\n");

        }
    }
}
