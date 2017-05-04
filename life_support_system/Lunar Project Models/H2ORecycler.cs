using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class H2ORecycler
    {
        // Variables
        private double m_urineInput24Hrs;
        private double m_wetSolidInput24Hrs;

        // Functions

        public H2ORecycler(Crew crewGiven)
        {
            // Water that requires recycling comes in 3 forms:
            // Solid human waste
            // Liquid human waste
            // Human condensate
            Console.WriteLine("     ..Constructing H2O Recycler..");
            
            
            // TODO implement constructor for H2O recycler



        }

        private double recoverLiquidH20(double urineIn)
        {
            return 0;
        }
        private double recoverSolidH20(double solidIn)
        {
            return 0;
        }

    }
}
