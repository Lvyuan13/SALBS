using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarNumericSimulator.Reporting
{
    public class SimulationProgressReport
    {
        public EnvironmentState GlobalState;
        public List<ModuleResourceLevels> ModuleStates;
        public Dictionary<string, double> TankStates;
        public double PowerLoad;
    }
}
