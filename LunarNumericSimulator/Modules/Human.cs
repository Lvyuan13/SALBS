using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;

namespace LunarNumericSimulator.Modules
{
    public class Human : Module
    {
        #region variables

        Random rnd = new Random();
        private double randomPhaseShift;

        // variables for setting time of day to day activities
        private uint clothesWashTime;
        private uint dishWashTime;
        private uint showerTime;
        private bool hasShowered;
        private uint urinationFrequency;
        private uint[] urinationTimes;
        private uint urinationCount;
        private uint excretionTime;
        private uint excretionFrequency;
        private bool hasExcreted;
        private uint[] drinkTimes;
        private uint drinkFrequency;
        private uint drinksCount;
        private bool needNewTaskList;
        int[,] jobs;

        //metabolic rates for different activities in [W/m^2]
        const int M1 = 65;             //resting
        const int M2 = 100;            //light work
        const int M3 = 165;            //moderate work
        const int M4 = 230;            //heavy work
        const int M5 = 290;            //very heavy work
        const double EE = 5.818;          // EE = Energetic Equivalent of a human = 5.818 [Wh/m^2]

        // density of CO2 at 25 degrees C and 1 atmosphere [kg/m^3]
        const double densityCO2 = 1.809;
        // density of O2 at 25 degrees C and 1 atmosphere [kg/m^3]
        const double densityO2 = 1.309;


        // values for human resources consumed and produced from "Desigining for a human presence in space, 1994, NASA"
        // https://ntrs.nasa.gov/archive/nasa/casi.ntrs.nasa.gov/19940022934.pdf

        // variables for set quantities of resource PRODUCTION
        private const double respAndPerspH2OProduced = 2.28; // kg per day
        private const double urineH2OProduced = 1.5; // kg per day
        private const double flushH2OProduced = 0.5; // kg per day
        private const double fecesH2Oproduced = 0.091; // kg per day
        private const double hygieneH2OProduced = 12.58; // kg per day
        private const double clothesWashH2OProduced = 12.5; // kg per day

        private const double humanSleepingIntensity = -0.4;
        private const double humanNormalIntensity = 0;
        private const double humanActiveIntensity = 1;

        // variables for set quantities of resource CONSUMPTION
        private const double foodRehydrationH2OConsumed = 1.15; // kg per day
        private const double foodPrepH2OConsumed = 0.76; // kg per day
        private const double drinkingH2OConsumed = 1.62; // kg per day
        private const double handWashH2OConsumed = 4.09; // kg per day
        private const double showerH2OConsumed = 2.73; // kg per day
        private const double urinalFlushH2OConsumed = 0.49; // kg per day
        private const double clothesWashH2OConsumed = 12.5; // kg per day
        private const double dishWashH2OConsumed = 5.45; // kg per day


        #endregion variables

        #region configuration parameters

        [NumericConfigurationParameter("Height [cm]", "190", "double", false)]
        public double height { get; set; } // time doing resting  [hrs]

        [NumericConfigurationParameter("Weight [kg]", "80", "double", false)]
        public double weight { get; set; } // time doing resting  [hrs]

        [NumericConfigurationParameter("Total time spent resting during day [hrs]", "1", "double", false)]
        public double tRestWorkDuration { get; set; } // time doing resting  [hrs]
        [NumericConfigurationParameter("Occurance of resting [frequency]", "1", "int", false)]
        public int tRestWorkOccurances { get; set; } // time doing resting  [hrs]

        [NumericConfigurationParameter("Total time spent doing light work during day [hrs]", "10", "double", false)]
        public double tLightWorkDuration { get; set; } // time doing light work [hrs]
        [NumericConfigurationParameter("Occurance of light work [frequency]", "4", "int", false)]
        public int tLightWorkOccurances { get; set; } // time doing light work [hrs]


        [NumericConfigurationParameter("Total time spent doing moderate work during day [hrs]", "2.5", "double", false)]
        public double tModerateWorkDuration { get; set; } // time doing moderate work [hrs]
        [NumericConfigurationParameter("Occurance of moderate work [frequency]", "1", "int", false)]
        public int tModerateWorkOccurances { get; set; } // time doing moderate work [hrs]

        [NumericConfigurationParameter("Total time spent doing heavy work during day [hrs]", "0.5", "double", false)]
        public double tHeavyWorkDuration { get; set; } // time doing heavy work [hrs]
        [NumericConfigurationParameter("Occurance of heavy work [frequency]", "1", "int", false)]
        public int tHeavyWorkOccurances { get; set; } // time doing heavy work [hrs]


        [NumericConfigurationParameter("Total time spent doing very heavy work during day [hrs]", "0.0", "double", false)]
        public double tVeryHeavyWorkDuration { get; set; } // time doing very heavy work [hrs]
        [NumericConfigurationParameter("Occurance of very heavy work [frequency]", "0", "int", false)]
        public int tVeryHeavyWorkOccurances { get; set; } // time doing very heavy work [hrs]

        #endregion configuration parameters



        public Human(Simulation sim, int moduleid) : base(sim, moduleid)
        {
            randomPhaseShift = getRandom().NextDouble() * Math.PI; // random number between 0.0 and PI

            // set time to wash clothes
            // TODO: this probably needs randomising
            clothesWashTime = 17 * 60 * 60;
            // set time to wash dishes
            dishWashTime = 18 * 60 * 60;
            // set random time for this human to shower daily
            showerTime = (uint)getRandom().Next((int)secondsHumanDayStart, (int)secondsHumanDayEnd);

            // initialise urination properties
            urinationCount = 0;
            urinationFrequency = 4;
            urinationTimes = new uint[4];

            // set excretion variables
            excretionTime = (uint)getRandom().Next((int)secondsHumanDayStart, (int)secondsHumanDayEnd);
            excretionFrequency = 1;
            hasExcreted = false;

            // initialise random times to urinate through the day time
            for (int i = 0; i < urinationFrequency; i++)
            {
                urinationTimes[i] = (uint)getRandom().Next((int)secondsHumanDayStart, (int)secondsHumanDayEnd);
            }

            // initialise water drinking properties
            drinksCount = 0;
            drinkFrequency = 4;
            drinkTimes = new uint[4];
            // initialise some random drinking times
            for (int i = 0; i < urinationFrequency; i++)
            {
                drinkTimes[i] = (uint)getRandom().Next((int)secondsHumanDayStart, (int)secondsHumanDayEnd);
            }

            // as we start the simulator, we need to indicate that a task order needs to be generated.
            needNewTaskList = true;

        }

        public override void ModuleReady()
        {
        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.CO2,
                Resources.CH4,
                Resources.H2O,
                Resources.Heat,
                Resources.O2,
                Resources.Food,
                Resources.N2,
                Resources.Humidity
            };
        }

        public override double getModuleVolume()
        {
            return 0;
        }

        public override string moduleName
        {
            get { return "Human"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Human"; }
        }

        // TODO humidity control system must collect water in futre, it souldn't just magically appear in the wastewater tank
        // justification for this behaiviour at current time: negligable power usage to do so.

        public override List<string> requiresTanks()
        {
            return new List<string> {   "UrineTreatmentTank",
                                        "WasteWaterStorage"};

        }


        protected override void update(UInt64 clock)
        {
            if (needNewTaskList)
            {
                jobs = generateJobList();
                needNewTaskList = false;
            }
            
            int currentMetabolicRate = getCurrentMetabolicRate(clock);

            double areaDubois = 0.202 * Math.Pow(weight, 0.425) * Math.Pow(height / 100, 0.725);

            // volume of 02 required for this human in [L / day]
            double volumeO2Required = (currentMetabolicRate * areaDubois / EE) * 24;

            //  determine proportion of metablic rate and multiply it by the dubois area, then convert to kW
            double heatGeneration = (currentMetabolicRate * areaDubois) / 1000;

            // mass of oxygen required in [kg] over 24hrs. /1000 converts L to m^3
            double massO2ConsumedPerDay = (volumeO2Required / 1000) * densityO2;

            // RQ = Respiratory quotient = volumeCO2Produced / volumeO2Required 
            double RQ = ((EE / 5.88) - 0.78) / 0.23;

            // volume of 02 required for this human in 24hrs
            double volumeCO2ProducedPerDay = RQ * volumeO2Required;

            // mass of CarbonDioxide produced in [kg] over 24hrs. /1000 converts L to m^3
            double massCO2ProducedPerDay = (volumeCO2ProducedPerDay / 1000) * densityCO2;
            double massCO2ProducedPerSecond = massCO2ProducedPerDay / (60 * 60 * 24);
            double massO2ConsumedPerSecond = massO2ConsumedPerDay / (60 * 60 * 24);


            produceResource(Resources.CO2, massCO2ProducedPerSecond);
            consumeResource(Resources.O2, massO2ConsumedPerSecond);



            double heatRelease = 118 * Math.Pow(10, -3); // Humans release heat at 118W, converting to kJ
            produceResource(Resources.Heat, heatRelease);

            double sweatReleasedPerSecond = 1.388888888888889e-4;
            produceResourceLitres(Resources.Humidity, sweatReleasedPerSecond);


            if (getRandom().Next((int)secondsIn24Hours / 8) == 1) // There is an 8 in 86400 (24 hours) chance that flatulence will occur, since average person has flatulence 8 times per day
                flatulence();

            // if day has just ended
            if (clock % secondsHumanDayEnd == 0)
            {
                // tell human they should shower the next day and generate a new random time to shower
                hasShowered = false;
                Random random = new Random();
                showerTime = (uint)random.Next((int)secondsHumanDayStart, (int)secondsHumanDayEnd);

                // initialise random times to urinate through the day time
                urinationCount = 0;
                for (int i = 0; i < urinationFrequency; i++)
                {
                    urinationTimes[i] = (uint)random.Next((int)secondsHumanDayStart, (int)secondsHumanDayEnd);
                }

                // reset excretion time 
                excretionTime = (uint)random.Next((int)secondsHumanDayStart, (int)secondsHumanDayEnd);
                hasExcreted = false;

                // reset drinking times 
                drinksCount = 0;
                for (int i = 0; i < drinkFrequency; i++)
                {
                    drinkTimes[i] = (uint)random.Next((int)secondsHumanDayStart, (int)secondsHumanDayEnd);
                }

                // reorganise the jobs for the next day
                needNewTaskList = true;
            }

            // Day time / working activities only
            if (isHumanDay(clock))
            {

                if (getRandom().Next((int)secondsIn12Hours / 4) == 1) // There is an 4 in 43200 chance that eating will occur, since a person has 4 meals in a day
                    eat();


                // if its time to wash clothes
                if (clock % clothesWashTime == 0)
                {
                    // TODO does it use power to wash clothes?
                    consumeResource(Resources.H2O, clothesWashH2OConsumed);
                    getTank("WasteWaterStorage").addResource(clothesWashH2OProduced);
                }

                // if its time to wash dishes
                if (clock % dishWashTime == 0)
                {
                    // TODO does it use power to wash clothes?
                    consumeResource(Resources.H2O, dishWashH2OConsumed);
                    getTank("WasteWaterStorage").addResource(dishWashH2OConsumed);
                }

                // if its time to shower
                if (clock % showerTime == 0 && hasShowered == false)
                {
                    consumeResource(Resources.H2O, showerH2OConsumed);
                    getTank("WasteWaterStorage").addResource(showerH2OConsumed);
                    hasShowered = true;
                }

                // if human still has daily urinations remaining
                if (urinationCount < urinationFrequency)
                {
                    // if its time to urinate
                    if (clock % urinationTimes[urinationCount] == 0)
                    {
                        urinate();
                    }
                }

                // if human still has daily drinks remaining
                if (drinksCount < drinkFrequency)
                {
                    // if its time to drink
                    if (clock % drinkTimes[drinksCount] == 0)
                    {
                        drink();
                    }
                }

                // if its time to excrete
                if (clock % excretionTime == 0 && hasExcreted == false)
                {
                    excrete();
                }

            }
            // Night time / resting activites only
            else
            {
                // TODO lower metabolism whilst sleeping and resting

            }


        }

        protected void flatulence()
        {
            produceResourceLitres(Resources.N2, 0.0531F); // See Harry's notebook for details of these numbers
            produceResourceLitres(Resources.CO2, 0.0081F);
            produceResourceLitres(Resources.CH4, 0.0063F);
            produceResourceLitres(Resources.O2, 0.0036F);
        }

        protected void eat()
        {
            consumeResource(Resources.Food, 2);
            // TODO implement food rehydration h20 consumption?
        }

        protected void urinate()
        {
            getTank("UrineTreatmentTank").addResource(urineH2OProduced / urinationFrequency);
            getTank("UrineTreatmentTank").addResource(flushH2OProduced / urinationFrequency);
            urinationCount++;
            // wash hands 
            consumeResource(Resources.H2O, handWashH2OConsumed / (urinationFrequency + excretionFrequency));
        }

        private void excrete()
        {
            getTank("UrineTreatmentTank").addResource(fecesH2Oproduced);
            hasExcreted = true;
            // wash hands
            consumeResource(Resources.H2O, handWashH2OConsumed / (urinationFrequency + excretionFrequency));
        }

        private void drink()
        {
            consumeResource(Resources.H2O, drinkingH2OConsumed / drinkFrequency);
            drinksCount++;
        }

        int getCurrentMetabolicRate(ulong currentTime)
        {

            int[] taskTimes = new int[jobs.GetLength(0)];
            int runningSum = (int)secondsHumanDayStart;
            for (int i = 0; i < jobs.GetLength(0); i++)
            {
                taskTimes[i] = jobs[i, 0] + runningSum;
                runningSum = runningSum + jobs[i, 0];
            }


            ulong lowerBoundary = secondsHumanDayStart;
            for (int i = 0; i < taskTimes.Length; i++)
            {

                // if current time is greater than lower bounds AND less than current task time
                if ((currentTime >= lowerBoundary) && ((int)currentTime <= taskTimes[i]))
                {
                    return jobs[i, 1];
                }
                lowerBoundary = (ulong)taskTimes[i];
            }

            return M1;
        }

        int[,] generateJobList()
        {

            double totalHoursWorked = tRestWorkDuration + tLightWorkDuration + tModerateWorkDuration + tHeavyWorkDuration + tVeryHeavyWorkDuration;
            int totalJobs = (int)(tRestWorkOccurances + tLightWorkOccurances + tModerateWorkOccurances + tHeavyWorkOccurances + tVeryHeavyWorkOccurances);

            // TODO: total jobs MUST NOT be larger than the number of daytime hours available


            int[,] jobArray = new int[totalJobs, 2];     // arrayFormat is [job duration, metabolic rate]

            int jobCount = 0;

            for (int j = 0; j < (tRestWorkOccurances); j++)
            {
                jobArray[jobCount, 0] = (int)((tRestWorkDuration * 60 * 60) / tRestWorkOccurances);
                jobArray[jobCount, 1] = M1;
                jobCount++;
            }
            for (int j = 0; j < (tLightWorkOccurances); j++)
            {
                jobArray[jobCount, 0] = (int)((tLightWorkDuration * (double)60 * 60) / tLightWorkOccurances);
                jobArray[jobCount, 1] = M2;
                jobCount++;
            }
            for (int j = 0; j < (tModerateWorkOccurances); j++)
            {
                jobArray[jobCount, 0] = (int)((tModerateWorkDuration * (double)60 * 60) / tModerateWorkOccurances);
                jobArray[jobCount, 1] = M3;
                jobCount++;
            }
            for (int j = 0; j < (tHeavyWorkOccurances); j++)
            {
                jobArray[jobCount, 0] = (int)((tHeavyWorkDuration * (double)60 * 60) / tHeavyWorkOccurances);
                jobArray[jobCount, 1] = M4;
                jobCount++;
            }
            for (int j = 0; j < (tVeryHeavyWorkOccurances); j++)
            {
                jobArray[jobCount, 0] = (int)((tVeryHeavyWorkDuration * (double)60 * 60) / tVeryHeavyWorkOccurances);
                jobArray[jobCount, 1] = M5;
                jobCount++;
            }


            int[] sequence = GenerateShuffledIndexArray((int)totalJobs);


            int[,] shuffledJobArray = new int[totalJobs, 2]; // 
            for (uint i = 0; i < totalJobs; i++)
            {
                shuffledJobArray[i, 0] = jobArray[sequence[i], 0];
                shuffledJobArray[i, 1] = jobArray[sequence[i], 1];
            }

            return shuffledJobArray;

        }


        int[] GenerateShuffledIndexArray(int size)
        {

            int[] array = new int[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = i;
            }

            
            for (int i = array.Length; i > 0; i--)
            {
                int j = rnd.Next(i);
                int k = array[j];
                array[j] = array[i - 1];
                array[i - 1] = k;
            }
            return array;
        }
    }
}