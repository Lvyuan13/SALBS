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

        public class Atmosphere: List<Gas>
        {
            public float TotalPressure;
            public float Temperature;
            public float TotalMass;
            public float TotalEnthalpy;
        }

        public class Storage: List<StoredResource>
        {

        }

        public class StoredResource
        {
            public Resources Resource;
            public float Quantity;
            public StoredResource(ResourceManager<float> rm)
            {
                Resource = rm.managedResource;
                Quantity = rm.getLevel();
            }
        }

        public class Gas
        {
            public Resources Resource;
            public float Quantity;
            public float PartialPressure;
            public float Temperature;
            public float PartialEthalpy;
            public float PartialInternalEnergy;
            public float Density;
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
