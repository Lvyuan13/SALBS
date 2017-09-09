using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;

namespace LunarNumericSimulator.Modules
{
    public class Habitat : Module
    {

        [NumericConfigurationParameter("Initial Hull Temperature", "double", false)]
        public double currentTemp { private get; set; }
        public static double moduleRadius = 2;
        public Habitat(Simulation sim, int moduleid) : base(sim, moduleid)
        {
        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.Heat
            };
        }

        public override void ModuleReady()
        {
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
        public override List<string> requiresTanks()
        {
            return new List<string>();
        }

        protected override void update(UInt64 clock)
        {
            // First we need to estimate the heat transfer coefficient
            var airState = getAirState();
            double k = airState.ThermalConductivity;
            double D = moduleRadius;
            double mu = airState.Viscosity;
            double air_Cp = airState.SpecificHeat;
            double airTemp = getAirState().Temperature;
            double n = 0.4;
            double volumetricFlowRate = getSystemVolume() * VentilationSystem.percentageVolumeFlowRate;
            double massFlowRate = airState.Density * volumetricFlowRate;
            if (currentTemp > airTemp)
            {
                n = 0.4;
            }
            else if (airTemp > currentTemp)
            {
                n = 0.33;
            }

            double j = (massFlowRate) / (Math.PI * Math.Pow(moduleRadius, 2)); // Mass Flux

            double h = 0.023 * Math.Pow(j * D / mu, 0.8) * Math.Pow(mu * air_Cp / k, n) * k / D; // Estimating heat transfer coefficient


            double V = getModuleVolume();
            double r = moduleRadius; // Radius of habitat must be sufficient to fit human (2 metre)
            double hab_length = V / (Math.PI * r * r);
            double A_s = ((2 / r) + (2/hab_length)) * V; // Derived by extracting relation between A_s and V
            double rho = 2780; // kg/m3 density
            double hull_Cp = 921;  // specific heat capacity in J/kg.K
            double wall_thickness = 0.007; // Hull wall thickness
            double outerHullVolume = Math.PI * Math.Pow(r + wall_thickness, 2) * (hab_length + 2 * wall_thickness);
            double hullMass = (outerHullVolume - V) * rho;

            double flux = Math.Abs(h * A_s * (currentTemp - airTemp));
            if (currentTemp > airTemp)
            {
                produceResource(Resources.Heat, flux*Math.Pow(10,-3));
            } else if (airTemp > currentTemp)
            {
                var currentEnergy = (currentTemp +273.15) * hull_Cp * hullMass;
                currentTemp = (((currentEnergy + flux) / hull_Cp) / hullMass) - 273.15;
            }

        }
    }
}