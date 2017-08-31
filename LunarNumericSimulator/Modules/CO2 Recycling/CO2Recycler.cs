using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using LunarNumericSimulator.ResourceManagers;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.Modules
{
    public abstract class CO2Recycler : Module
    {
        protected PIDController m_pid = new PIDController(2, 0.1, 0);
        protected bool m_changeResources = true;


        public CO2Recycler(Simulation sim, int moduleid) : base(sim, moduleid)
        {

        }

        protected double updatePID()
        {
            double CO2Level = getAtmosphericFraction(Resources.CO2);
            double CO2mass = getResourceLevel(Resources.CO2);

            var result = m_pid.update(CO2Level - 0.005, 1); // check for acceptable CO2 level

            // set member variable as to whether the resources should be updated on this particular update call
            // true by default
            m_changeResources = true;

            if (result <= 0)
                m_changeResources = false;

            if (CO2mass - result < 0)
            {
                m_pid.removeWindup();
                m_changeResources = false;
            }

            // return the result of the pid controller
            return result;
        }



    }
}