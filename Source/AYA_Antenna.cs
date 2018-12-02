﻿// ALL Y'ALL
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

    // ############# ANTENNAE ############### //

    public class AYA_Antenna : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#AYA_ANTENNA_UI_ANTENNA_EXTEND_ALL")]
        public void DoAllAntenna()                                                                //This runs every time you click "extend all" or "retract all"
        {
            bool extended = true;                                                               //This is the check if we are extending or retracting all, default to retracting.
            var callingPart = this.part.FindModuleImplementing<ModuleDeployableAntenna>();   //Variable for the part doing the work.
            if (callingPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)          //If the calling part is retracted...
            {
                extended = false;                                                               //...then it's not extended. Duh!
            }

            Events["DoAllAntenna"].active = false;
            AYA_PAW_Refresh.Instance.RefreshPAWMenu(this.part, AYA_PAW_Refresh.AYA_Module.antenna, "DoAllAntenna");


            if (extended)         //If it's retracted...
            {
                // Events["DoAllAntenna"].guiName = "Extend all antennae";                          //Set it to extend.
                Events["DoAllAntenna"].guiName = Localizer.Format("#AYA_ANTENNA_UI_ANTENNA_EXTEND_ALL");    //Set it to extend.
                Events["DoAllAntenna"].active = true;
            }
            else
            {
                if (callingPart.retractable)
                {

                    // Events["DoAllAntenna"].guiName = "Retract all antennae";                         //set it to retract.
                    Events["DoAllAntenna"].guiName = Localizer.Format("#AYA_ANTENNA_UI_ANTENNA_RETRACT_ALL");      //set it to retract.
                    Events["DoAllAntenna"].active = true;
                }
            }


            foreach (Part eachPart in vessel.Parts)                                             //Cycle through each part on the vessel
            {
                var thisPart = eachPart.FindModuleImplementing<ModuleDeployableAntenna>();   //If it's an antenna...
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

        public void Start()
        {
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

            var thisPart = this.part.FindModuleImplementing<ModuleDeployableAntenna>();      //This is so the below code knows the part it's dealing with is an antenna.
            if (thisPart != null && thisPart.animationName != "")                               //Verify it's actually an antenna and has an animation (rules out ox-stats and the like)
            {
                if (thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDING ||
                    thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTING)        //If it's extending or retracting...
                {
                    Events["DoAllAntenna"].active = false;                                        //...you don't get no menu option!
                }

                if (thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)         //If it's retracted...
                {
                    // Events["DoAllAntenna"].guiName = "Extend all antennae";                          //Set it to extend.
                    Events["DoAllAntenna"].guiName = Localizer.Format("#AYA_ANTENNA_UI_ANTENNA_EXTEND_ALL");    //Set it to extend.
                    Events["DoAllAntenna"].active = true;
                }
                if (thisPart.retractable && thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDED)  //If it's extended AND retractable...
                {
                    // Events["DoAllAntenna"].guiName = "Retract all antennae";                         //set it to retract.
                    Events["DoAllAntenna"].guiName = Localizer.Format("#AYA_ANTENNA_UI_ANTENNA_RETRACT_ALL");      //set it to retract.
                    Events["DoAllAntenna"].active = true;
                }
            }
        }
    }
}
