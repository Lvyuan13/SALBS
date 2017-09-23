using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;

namespace LunarNumericSimulator.Modules
{
    public class Human: Module
    {

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

        // values for human resources consumed and produced from "Desigining for a human presence in space, 1994, NASA"
        // https://ntrs.nasa.gov/archive/nasa/casi.ntrs.nasa.gov/19940022934.pdf

        // variables for set quantities of resource PRODUCTION
        // 2.28kg perspiration and respiration
        // 0.036kg food prep latent
        // 1.5kg urine
        // 0.5kg urine flush
        // 0.091kg feces water
        // 12.58kg hygiene water
        // 12.5kg clothes wash water
        // TOTAL = 29.49
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
        // TODO write these in..
        // 1.15kg in food
        // 0.76kg in food prep water
        // 1.62kg of drinking water
        // 0.35kg of metabolised water <- not currently implemented
        // 4.09kg of hand/face wash water
        // 2.73kg of shower water
        // 0.49kg of urinal flush
        // 12.5kg of clothes wash water
        // 5.45kg of dish wash water
        // TOTAL = 29.14kg water used
        private const double foodRehydrationH2OConsumed = 1.15; // kg per day
        private const double foodPrepH2OConsumed = 0.76; // kg per day
        private const double drinkingH2OConsumed = 1.62; // kg per day
        private const double handWashH2OConsumed = 4.09; // kg per day
        private const double showerH2OConsumed = 2.73; // kg per day
        private const double urinalFlushH2OConsumed = 0.49; // kg per day
        private const double clothesWashH2OConsumed = 12.5; // kg per day
        private const double dishWashH2OConsumed = 5.45; // kg per day


        public Human(Simulation sim, int moduleid) : base(sim,moduleid){
   
            randomPhaseShift = getRandom().NextDouble()*Math.PI; // random number between 0.0 and PI

            // set time to wash clothes
            clothesWashTime = 17 * 60 * 60;
            // set time to wash dishes
            dishWashTime = 18 * 60 * 60;
            // set random time for this human to shower daily
            showerTime = (uint)getRandom().Next((int)secondsHumanDayStart,(int)secondsHumanDayEnd);

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

        }

        public override void ModuleReady()
        {
        }

        public override List<Resources> getRegisteredResources(){
            return new List<Resources>() { 
                Resources.CO2,
                Resources.CH4,
                Resources.H2O,
                Resources.Heat,
                Resources.O,
                Resources.Food,
                Resources.N,
                Resources.Humidity
            };
        } 

        public override double getModuleVolume(){
            return 0;
        }

        public override string moduleName {
            get { return "Human"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Human"; }
        }


        // 1. urine goes into  WasteWaterStorage 
        // 2. urine gets treated with chemicals and flush water
        // 3. pretreated urine collected and put into wasteWaterStorage
        // 4. pretreated urine goes to UPA for additional processing (VCD), producing distillate
        // 5. distillate is stored in a tank for further processing.
        // 6. the distillate tank water is then processed by waterProcessing assembly(WPA) to produce potable water 
            
        // NOTE: Wastewater delivered to the WPA includes condensate from the Temperature and Humidity Control System,
        // distillate from the UPA, and Sabatier product water.

        // On ISS, wash water is not collected. Wash cloths used by the crew for hygiene are hung to dry. (water system architectures for moon and mars bases, 2015)
        // ASSUME: gravity drainage of wash water and shower water to pass on to WPA

        // TODO humidity control system must collect water in futre, it souldn't just magically appear in the wastewater tank
        // justification for this behaiviour at current time: negligable power usage to do so.
        


        public override List<string> requiresTanks()
        {
            return new List<string> {   "UrineTreatmentTank",
                                        "WasteWaterStorage"};

        }


        protected override void update(UInt64 clock){
            double intensity_time = clock % 86400;
            double intensity = -0.6988452409052097 + 0.00003324243435223318* intensity_time + (1.47732330889e-9)*Math.Pow(intensity_time, 2) - (4.3681e-14) * Math.Pow(intensity_time, 3) + (2.6e-19) * Math.Pow(intensity_time, 4);
            double airIntakeL = 0.1*(2*intensity + 1)*Math.PI*Math.Cos((2*Math.PI/5 )*clock + randomPhaseShift); // A sine function which estimates human breathing patterns
            // TODO: Factor in varying breathing rates for intensities
            var density = getAirState().Density;
            double airIntakeKG = Math.Abs(density * 0.001 * airIntakeL);

            if (airIntakeL < 0){
                double nitrogenIntake = getAtmosphericMassFraction(Resources.N) * airIntakeKG;
                double oxygenIntake = getAtmosphericMassFraction(Resources.O) * airIntakeKG;
                double co2Intake = getAtmosphericMassFraction(Resources.CO2) * airIntakeKG;
                consumeResource(Resources.N, nitrogenIntake);
                consumeResource(Resources.O, oxygenIntake);
                consumeResource(Resources.CO2, co2Intake);
            } else if (airIntakeL > 0){
                double nitrogenProduced = getAtmosphericMassFraction(Resources.N) * airIntakeKG; // TODO: Correct breathing calcs
                double oxygenProduced = getAtmosphericMassFraction(Resources.O) * airIntakeKG - 2*(9.096643518518519e-6); // This may help: http://www.madsci.org/posts/archives/2004-09/1096283374.En.r.html
                double co2Produced = getAtmosphericMassFraction(Resources.CO2) * airIntakeKG + 2*0.0000784;
                produceResource(Resources.N, nitrogenProduced);
                produceResource(Resources.O, oxygenProduced);
                produceResource(Resources.CO2, co2Produced);
            }

            double heatRelease = 118 * Math.Pow(10,-3); // Humans release heat at 118W, converting to kJ
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
            }

            // Day time / working activities only
            if (isHumanDay(clock))
            { 
                // TODO work schedule that changes metabolism
               
                if (getRandom().Next( (int)secondsIn12Hours / 4) == 1) // There is an 4 in 43200 chance that eating will occur, since a person has 4 meals in a day
                  eat();

                // TODO implement respiration and perspiration produced - related to metabolism?
                // TODO implement excrement water produced

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

        protected void flatulence(){
            produceResourceLitres(Resources.N, 0.0531F); // See Harry's notebook for details of these numbers
            produceResourceLitres(Resources.CO2, 0.0081F);
            produceResourceLitres(Resources.CH4, 0.0063F);
            produceResourceLitres(Resources.O, 0.0036F);
        }

        protected void eat(){
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
    }
}