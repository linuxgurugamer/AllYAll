﻿// ALL Y'ALL v0.5
// By 5thHorseman
// License: CC SA
// v0.1: Added "Extend All" and "Retract All" to deployable solar panels.
// v0.2: Added "Extend All" and "Retract All" to deployable radiators.
// v0.3: Added "Perform All Science" to all science instruments.
// v0.4: Fixed Surface Sample bug and added ability to perform all experiments in a part with multiple experiments
// v0.5: Fixed Mystery Goo and Science Jr running when they had not been reset.
// v0.9: (proposed) Modify UI to lessen the number of lines in right-click menus.

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
        bool isDeployedAndRetractable = false;
        bool anyExtendable = false;
        bool moving = false;

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Extend All")]
        public void DoAllSolar()
        {
            foreach (Part eachPart in vessel.Parts)                                             //Cycle through each part on the vessel
            {
                var thisPart = eachPart.FindModuleImplementing<ModuleDeployableSolarPanel>();   //If it's a solar panel...
                if (thisPart != null)
                {
                    if (isDeployedAndRetractable)
                        thisPart.Retract();                                    //Retract it
                    else
                        thisPart.Extend();                                     //Deploy it
                }
            }
        }


        public void FixedUpdate()
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR)
                return;


            moving = false;
            anyExtendable = false;
            isDeployedAndRetractable = false;

            foreach (Part eachPart in vessel.Parts)                                          //Cycle through each part on the vessel
            {
                var thisPart = eachPart.FindModuleImplementing<ModuleDeployableSolarPanel>();
                if (thisPart != null)
                {
                    if (thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDING ||
                         thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTING)
                    {
                        moving = true;
                        break;
                    }

                    if (thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)
                        anyExtendable = true;
                    if (thisPart.retractable && thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDED)
                        isDeployedAndRetractable = true;
                }
            }

            if (moving)
            {
                Events["DoAllSolar"].active = false;
                return;
            }

            if (isDeployedAndRetractable)
            {
                Events["DoAllSolar"].guiName = "Retract all solar";
                Events["DoAllSolar"].active = true;
            }
            else
            {
                Events["DoAllSolar"].guiName = "Extend all solar";
                Events["DoAllSolar"].active = anyExtendable;
            }
        }
    }
}
