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

    public class AYA_Radiator : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#AYA_ANTENNA_UI_SAS_ACTIVATE_ALL")]
        public void DoAllRadiator()
        {
            bool extended = true;
            var callingPart = this.part.FindModuleImplementing<ModuleDeployableRadiator>();     //Variable for the part doing the work.
            if (callingPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)          //If the calling part is retracted...
            {
                extended = false;                                                               //...then it's not extended. Duh!
            }

            Events["DoAllRadiator"].active = false;
            AYA_PAW_Refresh.Instance.RefreshPAWMenu(this.part, AYA_PAW_Refresh.AYA_Module.radiator, "DoAllRadiator");

            if (HighLogic.LoadedSceneIsEditor)
            {
                foreach (Part eachPart in EditorLogic.fetch.ship.Parts)                                             //Cycle through each part on the vessel
                {
                    DoIt(eachPart, extended);
                }
            }
            else
            {
                foreach (Part eachPart in vessel.Parts)                                             //Cycle through each part on the vessel
                {
                    DoIt(eachPart, extended);
                }
            }
        }
        void DoIt(Part eachPart, bool extended)
        {
            var thisPart = eachPart.FindModuleImplementing<ModuleDeployableRadiator>();     //If it's a radiator...
            if (thisPart != null && thisPart.animationName != "")                           //..and it has an animation (rules out passive radiators)
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

        public void UpdatePAWMenu()
        {
            if (HighLogic.LoadedScene != GameScenes.FLIGHT)
                return;

            var thisPart = this.part.FindModuleImplementing<ModuleDeployableRadiator>();        //This is so the below code knows the part it's dealing with is a radiator.
            if (thisPart != null && thisPart.animationName != "")                               //Verify it's actually a radiator and has an animation (rules out passive radiators)
            {
                if (thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDING ||
                    thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTING)        //If it's extending or retracting...
                {
                    Events["DoAllRadiator"].active = false;                                     //...you don't get no menu option!
                }

                if (thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)         //If it's retracted...
                {
                    // Events["DoAllRadiator"].guiName = "Extend all radiators";                   //Set it to extend.
                    Events["DoAllRadiator"].guiName = Localizer.Format("#AYA_ANTENNA_UI_RADIATOR_EXTEND_ALL");                   //Set it to extend.
                    Events["DoAllRadiator"].active = true;
                }
                if (thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDED)          //If it's extended...
                {
                    // Events["DoAllRadiator"].guiName = "Retract all radiators";                  //set it to retract.
                    Events["DoAllRadiator"].guiName = Localizer.Format("#AYA_ANTENNA_UI_RADIATOR_RETRACT_ALL");                  //set it to retract.
                    Events["DoAllRadiator"].active = true;
                }
            }
        }
    }
}
