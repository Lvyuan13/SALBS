using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class Human
    {
        #region variables
        // Variables

        // height in [cm]
        private double m_height;
        // weight in [kg]
        private double m_weight;
        // O2 consumption in [kg] per 24 hrs
        public double m_O2ConsumptionPerDay { get; }
        // CO2 production in [kg] per 24 hrs
        public double m_CO2ProductionPerDay { get; }
        // Heat transfer from the human in [W]
        public double m_heatGeneration { get; }
        // Drinking Water required in [kg] per 24hrs
        public double m_potableH2ORequiredPerDay{ get; }
        // Washing Water required in [kg] per 24hrs
        public double m_hygieneH2OUsedPerDay { get; }
        // Urine produced per day in [kg]
        public double m_urineProducedPerDay { get; }
        // 
        public double m_urineFlushPerDay { get; }

        // 


        // Constants

        //metabolic rates for different activities in [W/m^2]
        const double M1 = 65;             //resting
        const double M2 = 100;            //light work
        const double M3 = 165;            //moderate work
        const double M4 = 230;            //heavy work
        const double M5 = 290;            //very heavy work
        const double EE = 5.818;          // EE = Energetic Equivalent of a human = 5.818 [Wh/m^2]

        // time spent on various duties, must sum to 24hrs
        const double t1 = 12;             // time resting [hrs]
        const double t2 = 10;             // time doing light work [hrs]
        const double t3 = 2;              // time doing moderate work [hrs]
        const double t4 = 0;              // time doing heavy work [hrs]
        const double t5 = 0;              // time doingn very heavy work [hrs] 

        // density of CO2 at 25 degrees C and 1 atmosphere [kg/m^3]
        const double densityCO2 = 1.809;
        // density of O2 at 25 degrees C and 1 atmosphere [kg/m^3]
        const double densityO2 = 1.309;

        #endregion

        // Functions

        // Height in cm, Weight in kg
        public Human(double heightGiven, double weightGiven)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("     ..Constructing Human..");
            Console.ForegroundColor = ConsoleColor.White;

            this.m_height = heightGiven;
            this.m_weight = weightGiven;

            // H2O Figure obtained from folling literature: 
            // Designing for Human Presence in Space: An Introduction to Environmental Control and Life Support Systems
            // 1994 
            // NASA used the figure of 2.3kg per day.
            // update:
            // Other sources such as Eckart: "spacefligth life support and biospherics" indicate this is a minimum value.
            // Therefore 3.6kg is used as required drinking water per day.
            m_potableH2ORequiredPerDay = 3.6;        // [kg/day]
            // values for hygiene water required and urine produced (maximum) are taken from Eckart:
            // "spacefligth life support and biospherics"
            m_hygieneH2OUsedPerDay = 9;         // [kg/day]
            m_urineProducedPerDay = 2.27;           // [kg/day]
            m_urineFlushPerDay = 0.5;               // [kg/day]




            // The following method for calculating volume of O2 consumed and CO2 produced is as outlined
            // in international standards:
            // " SA TS ISO 16976.1:2015" - Respiratory protective devices - Human factors,
            // Part 1: Metabolic rates and respiratory flow rates "

            // Find the surface area of a body 
            double areaDubois = 0.202 * Math.Pow(weightGiven, 0.425) * Math.Pow(heightGiven/100, 0.725);

            // volume of 02 required for this human in 24hrs
            double volumeO2Required = t1 * (M1 * areaDubois / EE) +
                                      t2 * (M2 * areaDubois / EE) +
                                      t3 * (M3 * areaDubois / EE) +
                                      t4 * (M4 * areaDubois / EE) +
                                      t5 * (M5 * areaDubois / EE);


            //  determine proportion of metablic rate in 24 hours and multiply it by the dubois area
            this.m_heatGeneration = (t1 / 24) * (M1 * areaDubois) +
                                          (t2 / 24) * (M2 * areaDubois) +
                                          (t3 / 24) * (M3 * areaDubois) +
                                          (t4 / 24) * (M4 * areaDubois) +
                                          (t5 / 24) * (M5 * areaDubois);


            // mass of oxygen required in [kg] over 24hrs. /1000 converts L to m^3
            double massO2Required = (volumeO2Required / 1000) * densityO2;

            // RQ = Respiratory quotient = volumeCO2Produced / volumeO2Required 
            double RQ = ( (EE / 5.88) - 0.78) / 0.23;

            // volume of 02 required for this human in 24hrs
            double volumeCO2Produced = RQ * volumeO2Required;

            // mass of CarbonDioxide produced in [kg] over 24hrs. /1000 converts L to m^3
            double massCO2Produced = (volumeCO2Produced / 1000) * densityCO2;

            // set member variables
            this.m_O2ConsumptionPerDay = massO2Required;
            this.m_CO2ProductionPerDay = massCO2Produced;

        }





    }
}
