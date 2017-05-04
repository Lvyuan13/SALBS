using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class BoschCO2Recycler : CO2Recycler
    {

        #region Variables
        // Variables
        public double m_carbonProduced24Hrs { get; }
        public double m_nitrogenProduced24Hrs { get; }

        public double m_hyrdogenRequired24Hrs { get; }
        public double m_nitrogenRequired24Hrs { get; }


        // Constants
        
        // power requirement figures obtained from:
        // "Spaceflight life supprt and biospherics" by Peter Eckart

        
        // 54.9% Hydrogen
        // 45.4% CO2
        // 0.1% Nitrogen
        // At a mass flow rate of 0.145 [kg/day]
        protected const double boschCO2In = boschMassFlowRateIn * 0.454;        // [kg] / day
        protected const double boschHIn = boschMassFlowRateIn * 0.545;          // [kg] / day
        protected const double boschN2In = boschMassFlowRateIn * 0.001;           // [kg] / day

        // outflow consists of:
        // 0.009 kg/day of hydrogen
        // 0.034 kg/day of solid carbon
        // 0.102 kg/day of liquid hydrogen
        protected const double boschN2Out = 0.009;                                 // [kg] / day
        protected const double boschCout = 0.034;                                 // [kg] / day
        protected const double boschH2OOut = 0.102;                              // [kg] / day

        // The Power required for a Bosch reactor to do this:
        protected const double boschPower = 0.239;                                   // [kW] / day
        protected const double boschHeatGeneration = 0.313;                          // [kW] / day
        protected const double boschMassFlowRateIn = 0.145;                            // [kg] / day

        #endregion


        // Functions

        public BoschCO2Recycler(Crew crewGiven)
        {

            this.m_CO2GivenIn24Hrs = crewGiven.m_crewCO2ProductionPerDay;

            double scaleFactor = m_CO2GivenIn24Hrs / boschCO2In;

            this.m_H2OProduced24Hrs = scaleFactor * boschH2OOut;
            this.m_nitrogenProduced24Hrs = scaleFactor * boschN2Out;
            this.m_carbonProduced24Hrs = scaleFactor * boschCout;
            this.m_powerRequired24Hrs = scaleFactor * boschPower;
            this.m_hyrdogenRequired24Hrs = scaleFactor * boschHIn;
            this.m_nitrogenRequired24Hrs = scaleFactor * boschN2In;
            this.m_heatGenerated = scaleFactor * boschHeatGeneration;


            Console.WriteLine("\nBOSCH RECYCLER:");
            Console.WriteLine("It would use " + Math.Round(m_powerRequired24Hrs,2) + " kW of power per day");
            Console.WriteLine("CO2 input = " + Math.Round(m_CO2GivenIn24Hrs, 2) + " kg/24hrs");
            Console.WriteLine("Nitrogen Required = " + Math.Round(m_nitrogenRequired24Hrs, 2) + " kg/24hrs");
            Console.WriteLine("Hydrogen required = " + Math.Round(m_hyrdogenRequired24Hrs, 2) + " kg/24hrs\n");

            Console.WriteLine("It would produce " + Math.Round(m_H2OProduced24Hrs, 2) + " kg/24hrs of water");
            Console.WriteLine("In addition to " + Math.Round(m_carbonProduced24Hrs, 2) + " kg/24hrs of solid carbon");
            Console.WriteLine("and " + Math.Round(m_nitrogenProduced24Hrs, 2) + " kg/24hrs of Nitrogen");
            Console.WriteLine("It would also produce " + Math.Round(m_heatGenerated, 2) + " kW of heat");

            // sanity check (mass in = mass out)
            double inputSum = m_CO2GivenIn24Hrs + m_nitrogenRequired24Hrs + m_hyrdogenRequired24Hrs;
            double outputSum = m_H2OProduced24Hrs + m_carbonProduced24Hrs + m_nitrogenProduced24Hrs;
            Console.WriteLine("Input Sum = " + Math.Round(inputSum,2));
            Console.WriteLine("Output sum = " + Math.Round(outputSum, 2));


        }

    }
}
