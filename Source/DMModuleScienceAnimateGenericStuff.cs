using System;
using System.Collections.Generic;
using System.Linq;
using DMModuleScienceAnimateGeneric;

namespace AllYAll
{
    public class DMagic_SciAnimGenFactory
    {
        static internal DMagic_SciAnimGenFactory fetch = null;


        private Type _tDMModuleScienceAnimate;
        private Type _tDMModuleScienceAnimateGeneric;


        internal DMagic_SciAnimGenFactory()
        {
            fetch = this;
            _tDMModuleScienceAnimate = DMagicFactory.getType("DMagic.Part_Modules.DMModuleScienceAnimate");
            _tDMModuleScienceAnimateGeneric = DMagicFactory.getType("DMModuleScienceAnimateGeneric_NM.DMModuleScienceAnimateGeneric");
        }


        public bool inheritsFromOrIsDMModuleScienceAnimate(object o)
        {
            if (_tDMModuleScienceAnimate == null)
            {
                return false;
            }
            return ((o.GetType().IsSubclassOf(_tDMModuleScienceAnimate) || o.GetType() == _tDMModuleScienceAnimate));
        }


        public bool inheritsFromOrIsDMModuleScienceAnimateGeneric(object o)
        {
            if (_tDMModuleScienceAnimateGeneric == null)
                return false;
            return ((o.GetType().IsSubclassOf(_tDMModuleScienceAnimateGeneric) || o.GetType() == _tDMModuleScienceAnimateGeneric));
        }


        public IEnumerable<DMModuleScienceAnimateGeneric_NM.DMModuleScienceAnimateGeneric> FindDMAnimateGenericsForExperiment(string experimentId)
        {
            return FlightGlobals.ActiveVessel.FindPartModulesImplementing<DMModuleScienceAnimateGeneric_NM.DMModuleScienceAnimateGeneric>().Where(x => inheritsFromOrIsDMModuleScienceAnimateGeneric(x) && x.experimentID == experimentId).ToList();
        }


        internal bool RunExperiment(string sid, ModuleScienceExperiment exp, bool runSingleUse = true)
        {
            IScienceDataContainer m = null;

            // If possible run with DMagic new API
            IEnumerable<DMModuleScienceAnimateGeneric_NM.DMModuleScienceAnimateGeneric> lm = FindDMAnimateGenericsForExperiment(sid);
            if (lm != null && lm.Any())
            {
                m = lm.FirstOrDefault(x =>
                    (int)x.Fields.GetValue("experimentsLimit") > 1 ? DMSciAnimAPI.experimentCanConduct(x) : DMSciAnimAPI.experimentCanConduct(x) &&
                    (x.rerunnable || runSingleUse));

                if (m != null)
                {
                    DMSciAnimAPI.deployDMExperiment(m, false);
                }

                return true;
            }

            return false;
        }


    }
}
