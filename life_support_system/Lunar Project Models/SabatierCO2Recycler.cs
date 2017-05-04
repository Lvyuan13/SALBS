using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class SabatierCO2Recycler : CO2Recycler
    {
        #region Variables
        // Variables
        public double m_hyrdogenRequired24Hrs { get; }
        public double m_methaneProduced24Hrs { get; }

        // Constants

        // power requirement figures obtained from:
        // "Spaceflight life supprt and biospherics" by Peter Eckart

        // Inflow gas composed of 
        // 84.6% Hydrogen
        // 15.4% CO2
        // At a mass flow rate of 0.196 [kg/day]
        protected const double sabatierCO2In = sabatierMassFlowRateIn * 0.154;      // [kg] / day
        protected const double sabatierH2In = sabatierMassFlowRateIn * 0.846;      // [kg] / day

        // Outflow consists of 
        // 0.136 kg/day of Hydrogen
        // 0.06 kg/day of methane
        protected const double sabatierH2OOut = 0.136;                             // [kg] / day
        protected const double sabatierCH4Out = 0.06;                             // [kg] / day

        // The Power required for a Sabatier reactor to do this is
        protected const double sabatierPower = 0.05;                                  // [kW] / day
        protected const double sabatierHeatGeneration = 0.268;                          // [kW] / day
        protected const double sabatierMassFlowRateIn = 0.196;                          // [kg] / day


        #endregion


        // Functions

        public SabatierCO2Recycler(Crew crewGiven)
        {
            Console.WriteLine("     ..Constructing Sabatier CO2 Recycler..");
            // set member variable to CO2 in from crew
            this.m_CO2GivenIn24Hrs = crewGiven.m_crewCO2ProductionPerDay;

            // determine the scale factor for the current sabatier reactor stats
            double scaleFactor = m_CO2GivenIn24Hrs / sabatierCO2In;

            // scale all member variables to match CO2 input
            this.m_H2OProduced24Hrs = scaleFactor * sabatierH2OOut;
            this.m_powerRequired24Hrs = scaleFactor * sabatierPower;
            this.m_hyrdogenRequired24Hrs = scaleFactor * sabatierH2In;
            this.m_methaneProduced24Hrs = scaleFactor * sabatierCH4Out;
            this.m_heatGenerated = scaleFactor * sabatierHeatGeneration;

            // Include the power required to extract CO2 from the atmosphere
            double powerForCO2Extraction = powerReqToExtractCO2Per24Hrs(crewGiven);
            m_powerRequired24Hrs = m_powerRequired24Hrs + powerForCO2Extraction;

            // print output
            Console.WriteLine("\nSABATIER CO2 RECYCLER SYSTEM:");
            Console.WriteLine("Inputs: ");
            Console.WriteLine("CO2 input = " + Math.Round(m_CO2GivenIn24Hrs, 2) + " kg/24hrs");
            Console.WriteLine("Hydrogen input = " + Math.Round(m_hyrdogenRequired24Hrs, 2) + " kg/24hrs\n");

            Console.WriteLine("It would produce ");
            Console.WriteLine("H2O: " + Math.Round(m_H2OProduced24Hrs, 2) + " kg/24hrs");
            Console.WriteLine("CH4: " + Math.Round(m_methaneProduced24Hrs,2) + " kg/24hrs )");
            Console.WriteLine("Heat: " + Math.Round(m_heatGenerated, 2) + " kW");
            Console.WriteLine("It would use " + Math.Round(m_powerRequired24Hrs, 2) + " kW of power per day ( " + Math.Round(powerForCO2Extraction, 2) + " kW for CO2 extraction )");

            // sanity check (mass in = mass out)
            double inputSum = m_CO2GivenIn24Hrs +  m_hyrdogenRequired24Hrs;
            double outputSum = m_H2OProduced24Hrs + m_methaneProduced24Hrs;
            Console.WriteLine("Input Sum (" + Math.Round(inputSum, 2) + ") == Output sum (" + Math.Round(outputSum, 2) + ")?\n");

        }






    }
}
