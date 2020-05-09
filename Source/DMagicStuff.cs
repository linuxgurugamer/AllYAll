using System;
using System.Collections.Generic;
using System.Linq;
using DMagic.Part_Modules;

namespace AllYAll
{
    internal class DMagicStuff
    {
        static internal DMagicStuff fetch = null;

        private Type _tDMModuleScienceAnimate;

        internal DMagicStuff()
        {
            fetch = this;
            _tDMModuleScienceAnimate = DMagicFactory.getType("DMagic.Part_Modules.DMModuleScienceAnimate");
        }


        public bool inheritsFromOrIsDMModuleScienceAnimate(object o)
        {
            if (_tDMModuleScienceAnimate == null)
            {
                return false;
            }
            return ((o.GetType().IsSubclassOf(_tDMModuleScienceAnimate) || o.GetType() == _tDMModuleScienceAnimate));
        }



        internal bool RunExperiment(string sid, ModuleScienceExperiment exp, bool runSingleUse = true)
        {
            DMModuleScienceAnimate m = null;

            // If possible run with DMagic DMAPI

            IEnumerable<DMModuleScienceAnimate> lm2 = FlightGlobals.ActiveVessel.FindPartModulesImplementing<DMModuleScienceAnimate>().Where(x => inheritsFromOrIsDMModuleScienceAnimate(x) && x.experimentID == sid).ToList();

            if (lm2.Any())
            {
                m = lm2.FirstOrDefault(x =>
                {
                    return !x.Inoperable &&
                    ((int)x.Fields.GetValue("experimentLimit") > 1 ? DMagic.DMAPI.experimentCanConduct(x) : DMagic.DMAPI.experimentCanConduct(x) &&
                    (x.rerunnable || runSingleUse));
                });

                if (m != null)
                {
                    DMagic.DMAPI.deployDMExperiment(m, false); // maybe change this later
                    return true;
                }

            }


            return false;
        }
    }
}
