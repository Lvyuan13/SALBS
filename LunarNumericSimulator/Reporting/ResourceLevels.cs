using LunarNumericSimulator.ResourceManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarNumericSimulator.Reporting
{
    public class EnvironmentState
    {
        public Atmosphere Atmospheric;
        public Storage Stored;
        public UInt64 clock;

        public class Atmosphere: List<Gas>
        {
            public double TotalPressure;
            public double Temperature;
            public double TotalMass;
            public double TotalEnthalpy;
        }

        public class Storage: List<StoredResource>
        {

        }

        public class StoredResource
        {
            public Resources Resource;
            public double Quantity;
            public StoredResource(ResourceManager<double> rm)
            {
                Resource = rm.managedResource;
                Quantity = rm.getLevel();
            }
        }

        public class Gas
        {
            public Resources Resource;
            public double Quantity;
            public double PartialPressure;
            public double Temperature;
            public double PartialEthalpy;
            public double PartialInternalEnergy;
            public double Density;
            public Gas(AtmosphericResourceManager rm)
            {
                Resource = rm.managedResource;
                Quantity = rm.getLevel();
                PartialPressure = rm.getPressure();
                Temperature = rm.getTemperature();
                PartialEthalpy = rm.getEnthalpyPerUnitMass();
                PartialInternalEnergy = rm.getInternalEnergyPerUnitMass();
                Density = rm.getDensity();
            }
            public Gas()
            {

            }
        }
    }


}
