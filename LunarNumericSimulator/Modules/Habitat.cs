using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;

namespace LunarNumericSimulator.Modules
{
    public class Habitat : Module
    {

        double currentTemp = 25;
        public Habitat(Simulation sim, int moduleid) : base(sim, moduleid)
        {
        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.Heat
            };
        }

        public override double getModuleVolume()
        {
            return 70;
        }

        public override string moduleName
        {
            get { return "Habitat"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Habitat"; }
        }

        protected override void update(UInt64 clock)
        {
            // TODO: Must model the heat transfer behaviour of the spacecraft hull in here
            double h = 5; // heat transfer coefficient
            double V = getModuleVolume();
            double r = 2; // Radius of habitat must be sufficient to fit human (2 metre)
            double hab_length = V / (Math.PI * r * r);
            double A_s = ((2 / r) + (2/hab_length)) * V; // Derived by extracting relation between A_s and V
            double rho = 2780; // kg/m3 density
            double c_p = 921;  // specific heat capacity in J/kg.K
            double wall_thickness = 0.007; // Hull wall thickness
            double outerHullVolume = Math.PI * Math.Pow(r + wall_thickness, 2) * (hab_length + 2 * wall_thickness);
            double hullMass = (outerHullVolume - V) * rho;

            double airTemp = getAirTemperature();

            double flux = Math.Abs(h * A_s * (currentTemp - airTemp));
            if (currentTemp > airTemp)
            {
                produceResource(Resources.Heat, flux*Math.Pow(10,-3));
            } else if (airTemp > currentTemp)
            {
                var currentEnergy = (currentTemp +273.15) * c_p * hullMass;
                currentTemp = (((currentEnergy + flux) / c_p) / hullMass) - 273.15;
            }

        }

        public override List<string> requiresTanks()
        {
            return new List<string>();
        }
    }
}