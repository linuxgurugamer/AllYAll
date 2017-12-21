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

    // ############# SCIENCE BOX ############### //
    public class AYA_ScienceBox : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Reset All Science")]
        public void DoResetScience()
        {
            if (Events["DoResetScience"].guiName == "Reset All Science")
            {
                Events["DoResetScience"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SCIENCE_RESET_ALL");
            }
            bool haveScientist = false;
            foreach (ProtoCrewMember crewMember in vessel.GetVesselCrew())
            {
                if (crewMember.trait == "Scientist")
                {
                    haveScientist = true;
                }
            }
            foreach (Part eachPart in vessel.Parts)                                 //Cycle through each part on the vessel
            {
                foreach (ModuleScienceExperiment thisExperiment in eachPart.FindModulesImplementing<ModuleScienceExperiment>())
                //Cycle through each ModuleScienceExperiment module in the part
                {
                    if (thisExperiment != null)                                     //Only continue it if it's actually a ModuleScienceExperiment (which it should always be but hey)
                    {
                        if (thisExperiment.experimentID.Substring(0, 3) == "WBI") //If it's a WBI experiment, from M.O.L.E., don't do it becuase those are special.
                        {
                            // Do nothing
                        }
                        else if (thisExperiment.Deployed)
                        {
                            if (thisExperiment.Inoperable)
                            //                            if (thisExperiment.experimentActionName == "Observe Mystery Goo" || thisExperiment.experimentActionName == "Observe Materials Bay")
                            {
                                if (haveScientist)
                                {
                                    thisExperiment.ResetExperimentExternal();
                                }
                            }
                            else
                            {
                                thisExperiment.ResetExperimentExternal();
                            }
                        }
                        //else print ("AYA: Did not deploy experiment.");
                    }
                }
            }
        }
    }
}
