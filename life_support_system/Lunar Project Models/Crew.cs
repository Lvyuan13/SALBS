using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class Crew
    {
        #region variables
        //Variables

        // list of humans that inhabit the lunar base
        List<Human> crewList;
        public int m_crewSize { get; }
        // total CO2 crew consumption in [kg] in 24hrs
        public double m_crewO2ConsumptionPerDay { get; }
        // total CO2 crew production in [kg] in 24hrs
        public double m_crewCO2ProductionPerDay { get; }
        // heat transfer from the crew in [W] 
        public double m_crewHeatGeneration { get; }
        // Water required in [kg] per 24hrs
        public double m_crewPotableH2ORequiredPerDay { get; }
        // Washing Water required in [kg] per 24hrs
        public double m_crewHygieneH2ORequiredPerDay { get; }
        // Urine produced per day in [kg]
        public double m_crewUrineProducedPerDay { get; }

        // Constants

        //this constructor just uses average astronaut height [cm] and weight [kg] values
        const double avgHeight = 190;
        const double avgWeight = 80;

        #endregion

        //Functions

        public Crew(int numberOfCrewMembers)
        {
            Console.WriteLine("\n..Constructing Crew..");
            this.m_crewSize = numberOfCrewMembers;
            //create a list of the human crew members
            crewList = new List<Human>();
            for (int i = 0; i < numberOfCrewMembers; i++)
            {
                crewList.Add(new Human(avgHeight, avgWeight));
            }

            // initialise values to zero.
            this.m_crewO2ConsumptionPerDay = 0;
            this.m_crewCO2ProductionPerDay = 0;
            this.m_crewHeatGeneration = 0;
            this.m_crewPotableH2ORequiredPerDay = 0;
            this.m_crewHygieneH2ORequiredPerDay = 0;
            this.m_crewUrineProducedPerDay = 0;
            

            for (int i = 0; i < numberOfCrewMembers; i++)
            {
                m_crewO2ConsumptionPerDay = m_crewO2ConsumptionPerDay + crewList.ElementAt(i).m_O2ConsumptionPerDay;
                m_crewCO2ProductionPerDay = m_crewCO2ProductionPerDay + crewList.ElementAt(i).m_CO2ProductionPerDay;
                m_crewPotableH2ORequiredPerDay = m_crewPotableH2ORequiredPerDay + crewList.ElementAt(i).m_potableH2ORequiredPerDay;
                m_crewHygieneH2ORequiredPerDay = m_crewHygieneH2ORequiredPerDay + crewList.ElementAt(i).m_hygieneH2ORequiredPerDay;
                m_crewUrineProducedPerDay = m_crewUrineProducedPerDay + crewList.ElementAt(i).m_urineProducedPerDay;
                m_crewHeatGeneration = m_crewHeatGeneration + crewList.ElementAt(i).m_heatGeneration;
            }
            
        }


    }
}
