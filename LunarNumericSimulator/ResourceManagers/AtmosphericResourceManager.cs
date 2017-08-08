using CsvHelper;
using LunarNumericSimulator.Utilities;
using System;
using System.IO;
using System.Linq;

namespace LunarNumericSimulator.ResourceManagers
{
    public abstract class AtmosphericResourceManager : ResourceManager<float>
    {

        protected ThermoEntry[] thermoTable;
        RBF2D_Double tempPressureRBF;
        RBF2D_Double volumeEnthalpyRBF;
        protected float enthalpyPerUnitMass;
        protected float specificVolume;
        protected float pressure;
        protected float temperature;
        protected float totalResource;
        abstract public string thermoFile { get; }

        public AtmosphericResourceManager(float initialValue){
            totalResource = initialValue;

            // Load the thermodynamic tables for the compound
            StreamReader reader = new StreamReader(new FileStream(thermoFile, FileMode.Open));
            CsvReader csvReader = new CsvReader(reader);
            thermoTable = csvReader.GetRecords<ThermoEntry>().ToArray();

            // Select the required data from the table. NOTE: order is important
            var data = (from element in thermoTable
                        select new float[] { element.Temperature, element.Pressure, element.Enthalpy, element.SpecificVolume }).ToArray();

            // Fit an interpolated surface to the data
            tempPressureRBF = new RBF2D_Double(data);

            data = (from element in thermoTable
                    select new float[] { element.Enthalpy, element.SpecificVolume, element.Temperature, element.Pressure }).ToArray();

            volumeEnthalpyRBF = new RBF2D_Double(data);
        }

        public abstract override void addResource(float resource);

        public abstract override void consumeResource(float resource);

        public abstract override float getLevel();

        public void setState(float temp, float press, float enth, float specvol){
			temperature = temp;
			pressure = press;
			enthalpyPerUnitMass = enth;
            specificVolume = specvol;
		}

		public float getEnthalpyPerUnitMass(){
			return enthalpyPerUnitMass;
		}

        public float getSpecificVolume()
        {
            return specificVolume;
        }

        public float getTemperature(){
			return temperature;
		}

		public float getPressure(){
			return pressure;
		}

		public float getTotalEnthalpy(){
			return totalResource * enthalpyPerUnitMass;
		}

        public abstract float LitresToKG(float litres);

		public EnthalpyAndSpecVolume getEnthalpyAndSpecVolumeAtPoint(float temp, float pressure){
            var result = tempPressureRBF.queryPoint(temp, pressure);
            return new EnthalpyAndSpecVolume(result[0], result[1]);
        }

        public TempAndPressure getTempAndPressureAtPoint(float enthalpy, float specificVolume)
        {
            var result = volumeEnthalpyRBF.queryPoint(enthalpy, specificVolume);
            return new TempAndPressure(result[0], result[1]);
        }

        public class TempAndPressure
        {
            public TempAndPressure(double tmp, double prs)
            {
                Temperature = Convert.ToSingle(tmp);
                Pressure = Convert.ToSingle(prs);
            }
            public float Temperature;
            public float Pressure;
        }

        public class EnthalpyAndSpecVolume
        {
            public EnthalpyAndSpecVolume(double enth, double specvol)
            {
                Enthalpy = Convert.ToSingle(enth);
                Enthalpy = Convert.ToSingle(specvol);
            }
            public float Enthalpy;
            public float SpecificVolume;
        }
    }
}