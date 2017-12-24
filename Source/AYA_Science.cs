// ALL Y'ALL
// By 5thHorseman with much help from many others
// License: CC SA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.Localization;

namespace AllYAll
{
    // ############# SCIENCE ############### //

    public class AYA_Science : PartModule
    {
        private IList<ModuleScienceExperiment> _DMModuleScienceAnimates;
        private IList<ModuleScienceExperiment> _DMModuleScienceAnimateGenerics;


        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#AYA_ANTENNA_UI_SCIENCE_PERFORM_ALL")]
        public void DoAllScience()
        {
            var DMagic = new DMagicFactory();
#if false
            if (Events["DoAllScience"].guiName == "Perform All Science")
            {
                Events["DoAllScience"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SCIENCE_PERFORM_ALL");
            }
#endif
            foreach (Part eachPart in vessel.Parts)                                 //Cycle through each part on the vessel
            {
                foreach (ModuleScienceExperiment thisExperiment
                    in eachPart.FindModulesImplementing<ModuleScienceExperiment>()) //Cycle through each ModuleScienceExperiment module in the part
                {
                    if (thisExperiment != null)                                     //Only continue it if it's actually a ModuleScienceExperiment (which it should always be but hey)
                    {
                        if (thisExperiment.experimentActionName == "Take Surface Sample") //If it's a surface sample, we need to make sure it's not locked out.
                        {
                            if (ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.ResearchAndDevelopment) > 0) // Are you allowed to do surface samples? NOTE: 0 is tier 1. 0.5 is tier 2. 1 is tier 3. These could change if more tiers are added.
                            {
                                if (!thisExperiment.Deployed)
                                {
                                    thisExperiment.DeployExperiment(); //Deploy the experiment if it's not already deployed
                                                                       //print ("AYA: Deployed Surface Sample that had not yet been deployed");
                                }
                                //else print ("AYA: Did not deploy Surface Sample as it had previously been deployed.");
                            }
                            //else print ("AYA: Did not deploy Surface Sample as R&D is the lowest tier.");
                        }
                        else if (thisExperiment.experimentID.Substring(0, 3) == "WBI") //If it's a WBI experiment, from M.O.L.E., don't do it becuase those are special.
                        {
                            // Do nothing
                        }
                        else if (!thisExperiment.Deployed)
                        {
                            thisExperiment.DeployExperiment(); //Deploy the experiment if it's not already deployed
                                                               //print ("AYA: Deployed experiment that had not been previously deployed.");
                        }
                        //else print ("AYA: Did not deploy experiment.");
                    }
                }

            }


            Vessel v = FlightGlobals.ActiveVessel;
            _DMModuleScienceAnimates = null;
            _DMModuleScienceAnimateGenerics = null;
            if (v != null && HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                _DMModuleScienceAnimates = v.FindPartModulesImplementing<ModuleScienceExperiment>().Where(x => DMagic.inheritsFromOrIsDMModuleScienceAnimate(x)).ToList();
                _DMModuleScienceAnimateGenerics = v.FindPartModulesImplementing<ModuleScienceExperiment>().Where(x => DMagic.inheritsFromOrIsDMModuleScienceAnimateGeneric(x)).ToList();
            }

            // If possible run with DMagic new API
            if (_DMModuleScienceAnimateGenerics != null && _DMModuleScienceAnimateGenerics.Count > 0)
            {
                DMModuleScienceAnimateGeneric NewDMagicInstance = DMagic.GetDMModuleScienceAnimateGeneric();
                if (NewDMagicInstance != null)
                {
                    IEnumerable<ModuleScienceExperiment> lm;

                    lm = _DMModuleScienceAnimateGenerics.Where(x => (
                       !x.Inoperable &&
                       ((int)x.Fields.GetValue("experimentLimit") > 1 ? NewDMagicInstance.canConduct(x) : NewDMagicInstance.canConduct(x) && (x.rerunnable))
                       ));
                    if (lm != null)
                    {
                        ModuleScienceExperiment m = null;
                        for (int i = 0; i < lm.Count(); i++)
                        {
                            m = lm.ElementAt(i);

                            if (m != null)
                            {
                                //_logger.Debug("Running DMModuleScienceAnimateGenerics Experiment " + m.experimentID + " on part " + m.part.partInfo.name);
                                NewDMagicInstance.gatherScienceData(m);

                            }
                        }
                    }
                }
            }


            // If possible run with DMagic DMAPI
            if (_DMModuleScienceAnimates != null && _DMModuleScienceAnimates.Count > 0)
            {
                DMAPI DMAPIInstance = DMagic.GetDMAPI();
                if (DMAPIInstance != null)
                {
                    IEnumerable<ModuleScienceExperiment> lm;

                    lm = _DMModuleScienceAnimates.Where(x =>
                   {
                       return !x.Inoperable &&
                       ((int)x.Fields.GetValue("experimentLimit") > 1 ? DMAPIInstance.experimentCanConduct(x) : DMAPIInstance.experimentCanConduct(x) && (x.rerunnable));
                   });

                    ModuleScienceExperiment m = null;

                    for (int i = 0; i < lm.Count(); i++)
                    {
                        m = lm.ElementAt(i);
                        if (m != null)
                        {
                            //_logger.Trace("Running DMModuleScienceAnimates Experiment " + m.experimentID + " on part " + m.part.partInfo.name);
                            DMAPIInstance.deployDMExperiment(m);
                        }
                    }

                }
            }


        }

    }
}
