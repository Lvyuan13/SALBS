using LunarNumericSimulator.ResourceManagers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static LunarNumericSimulator.ResourceManagers.AtmosphericResourceManager;

namespace LunarNumericSimulator {
    public abstract class Module {
        // A link to the simulation environment, should not be directly accessed outside of this class (even by subclasses)
        private Simulation Environment;
        int resourceCount = Enum.GetNames(typeof(Resources)).Length;
        // useful values for child classes to refer to
        protected const uint secondsIn12Hours = 43200;
        protected const uint secondsIn24Hours = 86400;
        protected const uint secondsInCompleteCycle = 5401;
        //protected const UInt64 secondsHumanDayStart = 25200;
        //protected const UInt64 secondsHumanDayEnd = 75600;
        protected const UInt64 secondsHumanDayStart = 1;
        protected const UInt64 secondsHumanDayEnd = 3601;
        protected UInt64 secondsInHumanDayTime;
        protected UInt64 secondsInHumanNightTime;
        protected UInt64 secondsInHumanDayCycle;

        protected static Dictionary<string, TankResourceManager> tanks = new Dictionary<string, TankResourceManager>();

        // An ID that is allocated to the module when the user loads it into the workspace
        public int ModuleID
        {
            get;
            protected set;
        }

        // Keeps track of resources used by the module since last update
        private double[] resourceReceipts;
        public Module(Simulation sim, int id) {
            Environment = sim;
            ModuleID = id;

            foreach (string s in requiresTanks())
                createTank(s);

            resourceReceipts = new double[resourceCount];

            // initialise day and night information
            UInt64 secondsInHumanDayTime = (secondsHumanDayEnd - secondsHumanDayStart);
            //assuming a day is 24 hours long
            UInt64 secondsInHumanNightTime = secondsInCompleteCycle - secondsInHumanDayTime;
            // TODO remove this, its set at 5400 seconds (1.5hrs ) for testing.
            //secondsInHumanNightTime = 5401 - secondsInHumanDayTime;
            secondsInHumanDayCycle = secondsInHumanDayTime + secondsInHumanNightTime;
        }

        abstract public void ModuleReady();

        // Called every tick, describes the behaviour of the module.
        abstract protected void update(UInt64 clock);

        // Resources used by the module must be provided by the function. Hardcoding a list is advised. Prevents bug from silly cross-resource mistakes
        abstract public List<Resources> getRegisteredResources();

        // A hardcoded name for the module. How will we recognise a Hab module from a science module?
        abstract public string moduleName { get; }

        // A hardcoded friendly name for the module. This is for user presentation only
        abstract public string moduleFriendlyName { get; }

        // Return the volume of the module in m3
        abstract public double getModuleVolume();

        // A list of string with required tank names
        abstract public List<string> requiresTanks();

        // Gets a TankResurceManager from the Shared tank database
        public TankResourceManager getTank(string name)
        {
            if (!requiresTanks().Contains(name))
                throw new Exception("Module hasn't declared access to this tank!");
            TankResourceManager tank;
            try
            {
                tanks.TryGetValue(name, out tank);
            } catch (Exception e)
            {
                throw e;
            }
            if (tanks == null)
                throw new Exception("Tank does not exist!");
            return tank;
        }

        // Creates a new tank in the database, should not be directly called by implementation code
        protected void createTank(string s)
        {
            if (tanks.ContainsKey(s))
                return;
            tanks.Add(s, new TankResourceManager(0));
        }

        public static Dictionary<string, double> getTankLevels()
        {
            var result = new Dictionary<string, double>();
            foreach (var rm in tanks)
                result.Add(rm.Key, rm.Value.getLevel());
            return result;
        }

        // Internal function which is called by the simulator, this function will trigger an update
        public void tick(UInt64 clock) {
            resourceReceipts = new double[resourceCount];
            update(clock);
            updateResources();
        }

        private void updateResources()
        {
            for (int i = 0; i < resourceReceipts.Length; i++)
            {
                foreach (ResourceManager<double> rm in Environment.getAllResourceManagers())
                    if (rm.managedResource == ((Resources)i))
                        if (resourceReceipts[i] < 0)
                        {
                            rm.consumeResource(-resourceReceipts[i]);
                        } else
                        {
                            rm.addResource(resourceReceipts[i]);
                        }
            }
        }

        // Callable by the simulator to determine the resources used by the module in the last update
        public double[] getResourceConsumption() {
            return resourceReceipts;
        }

        public double getSystemVolume()
        {
            return Environment.getSystemVolume();
        }

        // Not abstract, as the subclasses should access resources through this function
        protected bool consumeResource(Resources res, double quantity) {
            if (Math.Sign(quantity) == -1)
                produceResource(res, -quantity);
            if (res == Resources.ElecticalEnergy)
                throw new Exception("Use consumePower instead of consumeResource");
            if (!getRegisteredResources().Contains(res)) {
                throw new Exception("The module " + ModuleID + " has not declared access to this resource! ");
            }
            if (getResourceLevel(res) - quantity < 0)
                return false;
            resourceReceipts[(int)res] -= quantity;
            return true;
        }

        protected void consumePower(double quantity)
        {
            if (!getRegisteredResources().Contains(Resources.ElecticalEnergy))
                throw new Exception("The module " + ModuleID + " has not declared access to power! ");
            if (quantity < 0)
                throw new Exception("Cannot consume negative energy usage");
            foreach (ResourceManager<double> rm in Environment.getAllResourceManagers())
                if (rm.managedResource == Resources.ElecticalEnergy)
                    resourceReceipts[(int)Resources.ElecticalEnergy] -= quantity;
        }

        protected void consumeResourceLitres(Resources res, double quantity)
        {
            var kg_L = getResourceDensity(res) * 0.001;
            consumeResource(res, quantity * kg_L);
        }

        // Not abstract, as the subclasses should access resources through this function
        protected void produceResource(Resources res, double quantity) {
            if (Math.Sign(quantity) == -1)
                consumeResource(res, -quantity);
            if (!getRegisteredResources().Contains(res)) {
                throw new Exception("The module " + ModuleID + " has not declared access to this resource! ");
            }
            resourceReceipts[(int)res] += quantity;
        }

        protected void produceResourceLitres(Resources res, double quantity)
        {
            var kg_L = getResourceDensity(res) * 0.001;
            produceResource(res, quantity * kg_L);
        }

        protected double getResourceLevel(Resources res) {
            if (!getRegisteredResources().Contains(res)) {
                throw new Exception("The module " + ModuleID + " has not declared access to this resource! ");
            }
            foreach (ResourceManager<double> rm in Environment.getAllResourceManagers())
                if (rm.managedResource == res)
                    return rm.getLevel();
            throw new Exception("Resource not found!");
        }

        protected double getResourceDensity(Resources res)
        {
            if (!getRegisteredResources().Contains(res))
            {
                throw new Exception("The module " + ModuleID + " has not declared access to this resource! ");
            }
            foreach (ResourceManager<double> rm in Environment.getAllResourceManagers())
                if (rm.managedResource == res)
                    return ((AtmosphericResourceManager)rm).Density;
            throw new Exception("Resource not found!");
        }

        protected AirThermoState getAirState()
        {
            foreach (ResourceManager<double> rm in Environment.getAllResourceManagers())
                if (rm.managedResource == Resources.Heat)
                    return ((ThermodynamicEngine)rm).getAverageAirState();
            throw new Exception("Resource not found!");
        }

        protected double getAtmosphericMassFraction(Resources res)
        {
            foreach (ResourceManager<double> rm in Environment.getAllResourceManagers())
                if (rm.managedResource == Resources.Heat)
                    return ((ThermodynamicEngine)rm).getMassFraction(res);
            throw new Exception("Resource not found!");
        }

        protected double getAtmosphericMolarFraction(Resources res)
        {
            foreach (ResourceManager<double> rm in Environment.getAllResourceManagers())
                if (rm.managedResource == Resources.Heat)
                    return ((ThermodynamicEngine)rm).getMolarFraction(res);
            throw new Exception("Resource not found!");
        }

        public Random getRandom()
        {
            return Environment.random;
        }


        // return true if time given is lunar day time and false if lunar night time
        protected bool isLunarDay(UInt64 currentTime)
        {
            UInt64 lowestDayValue = currentTime;
            UInt64 secondsInLunarCycle = 2551442;
            UInt64 secondsInLunarDay = 1275721;

            while (lowestDayValue >= secondsInLunarCycle)
            {
                lowestDayValue = lowestDayValue - secondsInLunarCycle;
            }
            if (lowestDayValue <= secondsInLunarDay)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // return true if time given is human daytime, false if human night time.
        protected bool isHumanDay(UInt64 currentTime)
        {
            // assumes that human is active/working between 07:00 and 21:00 (14 hours)
            // assumes that the human sleeps/inactive from 21:00 to 07:00 (10 hours)
            UInt64 lowestDayValue = currentTime;


            //Console.WriteLine("seconds in a day = " + secondsInHumanDayTime + " (hours = ) " + ((double)secondsInHumanDayTime/3600) );
            //Console.WriteLine("seconds in a night = " + secondsInHumanNightTime + " (hours = ) " + ((double)secondsInHumanNightTime / 3600));
            //Console.WriteLine("seconds in a whole cycle = " + secondsInHumanDayCycle + " (hours = ) " + ((double)secondsInHumanDayCycle / 3600));

            while (lowestDayValue >= secondsInHumanDayCycle)
            {
                lowestDayValue = lowestDayValue - secondsInHumanDayCycle;
            }
            // if the time given is 
            if (lowestDayValue >= secondsHumanDayStart && lowestDayValue < secondsHumanDayEnd)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
        public class NumericConfigurationParameter: Attribute
        {
            public readonly Type ParameterType;
            public readonly string friendlyName;
            public readonly bool AllowNegative;
            public readonly string propertyName;
            public readonly string defaultValue;

            public NumericConfigurationParameter(string name, string defaultVal, string type, bool allowNeg, [CallerMemberNameAttribute]string property = null)
            {
                if (ParameterType == typeof(string))
                    throw new Exception("Cannot place string value in Numeric Configuration Parameter");
                AllowNegative = allowNeg;
                defaultValue = defaultVal;
                switch (type)
                {
                    case ("double"):
                        ParameterType = typeof(double);
                        break;
                    case ("int"):
                        ParameterType = typeof(int);
                        break;
                    default:
                        throw new Exception("Invalid type for configuration parameter");
                }
                friendlyName = name;
                propertyName = property;
            }
        }

    }
}