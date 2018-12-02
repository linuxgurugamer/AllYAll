// ALL Y'ALL
// By 5thHorseman with much help from many others
// License: CC SA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.Localization;
using NearFutureSolar;

namespace AllYAll
{
    // ############# SOLAR PANELS ############### //

    public class AYA_Solar : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#AYA_ANTENNA_UI_SOLAR_EXTEND_ALL")]
        public void DoAllSolar()                                                                //This runs every time you click "extend all" or "retract all"
        {
            
            bool extended = true;                                                               //This is the check if we are extending or retracting all, default to retracting.
            var callingPart = this.part.FindModuleImplementing<ModuleDeployableSolarPanel>();   //Variable for the part doing the work.
            if (callingPart != null && callingPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)          //If the calling part is retracted...
            {
                extended = false;                                                               //...then it's not extended. Duh!
            }
            //
            // Same thing for the NearFuture Solar, if it's loaded
            //
            if (NFSPresent)
            {
                var callingPart2 = this.part.FindModuleImplementing<ModuleCurvedSolarPanel>();      //Variable for the part doing the work.
                if (callingPart2 != null && callingPart2.State == ModuleDeployablePart.DeployState.RETRACTED)               //If the calling part is retracted...
                {
                    extended = false;                                                               //...then it's not extended. Duh!
                }
            }

            Events["DoAllSolar"].active = false;
            AYA_PAW_Refresh.Instance.RefreshPAWMenu(this.part, AYA_PAW_Refresh.AYA_Module.solar, "DoAllSolar");

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

            //
            // Now do the nearFutureSolar
            //
            if (NFSPresent)
            {
                foreach (Part eachPart in vessel.Parts)                                             //Cycle through each part on the vessel
                {
                    var thisPart = eachPart.FindModuleImplementing<ModuleCurvedSolarPanel>();   //If it's a solar panel...
                    if (thisPart != null && thisPart.Deployable)                           //..and it has an animation (rules out ox-stats and the like)
                    {
                        if (extended)                                                               //then if the calling part was extended...
                        {
                            thisPart.Retract();                                                     //Retract it
                        }
                        else                                                                        //otherwise...
                        {
                            thisPart.Deploy();                                                      //Extend it
                        }
                    }
                }
            }

        }
        bool NFSPresent;
        public void Start()
        {
            NFSPresent = AssemblyLoader.loadedAssemblies.Any(a => a.assembly.GetName().Name == "NearFutureSolar");
            this.part.AddOnMouseEnter(OnMouseEnter());
        }
        Part.OnActionDelegate OnMouseEnter()
        {
            UpdatePAWMenu();
            return null;
        }
        public void OnDestroy()
        {
            this.part.RemoveOnMouseEnter(OnMouseEnter());
        }

        internal bool EventStatus(string e)
        {
            return Events[e].active;
        }

        public void UpdatePAWMenu()                                                               //This runs every second and makes sure the menus are correct.
        {
            if (HighLogic.LoadedScene != GameScenes.FLIGHT)
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
                    // Events["DoAllSolar"].guiName = "Extend all solar";                          //Set it to extend.
                    Events["DoAllSolar"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SOLAR_EXTEND_ALL");                         //Set it to extend.
                    Events["DoAllSolar"].active = true;
                }
                if (thisPart.retractable && thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDED)  //If it's extended AND retractable...
                {
                    // Events["DoAllSolar"].guiName = "Retract all solar";                         //set it to retract.
                    Events["DoAllSolar"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SOLAR_RETRACT_ALL");                         //set it to retract.
                    Events["DoAllSolar"].active = true;
                }
            }

            if (NFSPresent)
            {
                var thisPart2 = this.part.FindModuleImplementing<ModuleCurvedSolarPanel>();      //This is so the below code knows the part it's dealing with is a solar panel.
                if (thisPart2 != null && thisPart2.Deployable)                               //Verify it's actually a solar panel and has an animation (rules out ox-stats and the like)
                {
                    if (thisPart2.State == ModuleDeployablePart.DeployState.EXTENDING ||
                        thisPart2.State == ModuleDeployablePart.DeployState.RETRACTING)        //If it's extending or retracting...
                    {
                        Events["DoAllSolar"].active = false;                                        //...you don't get no menu option!
                    }

                    if (thisPart2.State == ModuleDeployablePart.DeployState.RETRACTED)         //If it's retracted...
                    {
                        // Events["DoAllSolar"].guiName = "Extend all solar";                          //Set it to extend.
                        Events["DoAllSolar"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SOLAR_EXTEND_ALL");                         //Set it to extend.
                        Events["DoAllSolar"].active = true;
                    }
                    if (thisPart2.State == ModuleDeployablePart.DeployState.EXTENDED)  //If it's extended AND retractable...
                    {
                        // Events["DoAllSolar"].guiName = "Retract all solar";                         //set it to retract.
                        Events["DoAllSolar"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SOLAR_RETRACT_ALL");                         //set it to retract.
                        Events["DoAllSolar"].active = true;
                    }
                }
            }

        }
    }
}
