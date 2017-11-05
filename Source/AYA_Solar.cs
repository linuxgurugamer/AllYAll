// ALL Y'ALL
// By 5thHorseman with much help from many others
// License: CC SA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AllYAll
{
    // ############# SOLAR PANELS ############### //

    public class AYA_Solar : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Extend All")]
        public void DoAllSolar()                                                                //This runs every time you click "extend all" or "retract all"
        {
            bool extended = true;                                                               //This is the check if we are extending or retracting all, default to retracting.
            var callingPart = this.part.FindModuleImplementing<ModuleDeployableSolarPanel>();   //Variable for the part doing the work.
            if (callingPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)          //If the calling part is retracted...
            {
                extended = false;                                                               //...then it's not extended. Duh!
            }
            foreach (Part eachPart in vessel.Parts)                                             //Cycle through each part on the vessel
            {
                var thisPart = eachPart.FindModuleImplementing<ModuleDeployableSolarPanel>();   //If it's a solar panel...
                if (thisPart != null && thisPart.animationName != "")                           //..and it has an animation (rules out ox-stats and the like)
                {
                    if (extended)                                                               //then if the calling part was extended...
                    {
                        thisPart.Retract();                                                     //Retract it
                    }
                    else                                                                        //otherwise...
                    {
                        thisPart.Extend();                                                      //Extend it
                    }
                }
            }
        }

        public void FixedUpdate()                                                               //This runs every second and makes sure the menus are correct.
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR)
                return;

            var thisPart = this.part.FindModuleImplementing<ModuleDeployableSolarPanel>();      //This is so the below code knows the part it's dealing with is a solar panel.
            if (thisPart != null && thisPart.animationName != "")                               //Verify it's actually a solar panel and has an animation (rules out ox-stats and the like)
            {
                if (thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDING ||
                    thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTING)        //If it's extending or retracting...
                {
                    Events["DoAllSolar"].active = false;                                        //...you don't get no menu option!
                }

                if (thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)         //If it's retracted...
                {
                    Events["DoAllSolar"].guiName = "Extend all solar";                          //Set it to extend.
                    Events["DoAllSolar"].active = true;
                }
                if (thisPart.retractable && thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDED)  //If it's extended AND retractable...
                {
                    Events["DoAllSolar"].guiName = "Retract all solar";                         //set it to retract.
                    Events["DoAllSolar"].active = true;
                }
            }
        }
    }
}
