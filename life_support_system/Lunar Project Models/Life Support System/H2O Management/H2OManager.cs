using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar_Project_Models
{
    class H2OManager
    {
        // Variables

        // Functions

        public H2OManager(Crew crewGiven)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("     ..Constructing H20 Manager..");
            Console.ForegroundColor = ConsoleColor.White;

            // Water that requires recycling comes in 3 forms:
            // Solid human waste
            // Liquid human waste
            // Human condensate

            VCDUrineProcessor UPA = new VCDUrineProcessor(crewGiven);
            MFBWaterProcessor WPA = new MFBWaterProcessor(crewGiven, UPA);

            // TODO implement constructor for H2O recycler



        }

    }
}
