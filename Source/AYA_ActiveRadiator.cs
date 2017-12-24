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
    // ############# RADIATORS ############### //

    public class AYA_ActiveRadiator : PartModule
    {
        double turnOnTime = 0;

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#AYA_ANTENNA_UI_RADIATOR_ACTIVATE_ALL")]
        public void DoAllActivateRadiator()
        {
            double ACTIVATION_TIME = 0;
            var callingPart = this.part.FindModuleImplementing<ModuleActiveRadiator>();     //Variable for the part doing the work.
            bool isCooling = callingPart.IsCooling;
            foreach (Part eachPart in vessel.Parts)                                             //Cycle through each part on the vessel
            {
                var thisPart = eachPart.FindModuleImplementing<ModuleActiveRadiator>();     //If it's a radiator...
                if (thisPart != null)                           
                {
                    //bool retractable = true;
                    var extendablePart = eachPart.FindModuleImplementing<ModuleDeployableRadiator>();
                    //if (extendablePart != null && extendablePart.animationName != "")
                    //{
                    //    if (extendablePart.deployState == ModuleDeployablePart.DeployState.RETRACTED)
                    //        retractable = false;
                    //}
                   // if (retractable)
                    {
                        ACTIVATION_TIME++;
                        if (!isCooling)                          
                        {
                            if (extendablePart != null)
                                extendablePart.Extend();
                            thisPart.Activate();                 
                        }
                        else                                  
                        {
                            if (extendablePart != null)
                                extendablePart.Retract();
                            thisPart.Shutdown();                 
                        }
                    }
                }
            }
            turnOnTime = Planetarium.GetUniversalTime() + ACTIVATION_TIME;
        }

        public void FixedUpdate()
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR)
                return;

            if (Planetarium.GetUniversalTime() > turnOnTime)
            {
                var thisPart = this.part.FindModuleImplementing<ModuleActiveRadiator>();        //This is so the below code knows the part it's dealing with is a radiator.
                if (thisPart != null)                               //Verify it's actually a radiator
                {
                    if (!thisPart.IsCooling)
                    {
                        Events["DoAllActivateRadiator"].guiName = Localizer.Format("#AYA_ANTENNA_UI_RADIATOR_ACTIVATE_ALL");
                        Events["DoAllActivateRadiator"].active = true;
                    }
                    else
                    {
                        Events["DoAllActivateRadiator"].guiName = Localizer.Format("#AYA_ANTENNA_UI_RADIATOR_SHUTDOWN_ALL");
                        Events["DoAllActivateRadiator"].active = true;
                    }
                }
            }
            else
            {
                Events["DoAllActivateRadiator"].active = false;
            }
        }
    }
}
