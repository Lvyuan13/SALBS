using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    abstract class CO2Recycler
    {
        #region Variables
        // Variables
        public double m_CO2GivenIn24Hrs { get; set; }
        public double m_H2OProduced24Hrs { get; set; }
        public double m_powerRequired24Hrs { get; set; }
        public double m_heatGenerated { get; set; }

        #endregion

        // Functions

        // This function takes in a mass of CO2 in kg per 24 hrs 
        // It outputs the power required to extract it from the atmosphere
        protected double powerReqToExtractCO2Per24Hrs(Crew crewGiven)
        {
            // Using data obtained from:
            // "Spaceflight life supprt and biospherics" by Peter Eckart
            // For 3 people, using a 4 Bed Molecular Sieve would require 0.535kW of power to extract the CO2
            // For 1 person it is assumed that this value can be scaled down lineraly by 3.
            const double powerReqForCO2Extraction = 0.535 / 3;

            return crewGiven.m_crewSize * powerReqForCO2Extraction;
        }

      

    }
}
