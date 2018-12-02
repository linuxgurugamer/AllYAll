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

    public class AYA_Drill : PartModule
    {
        bool resourceConvertersActive = false;
        bool drillsDeployed = false;

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#AYA_ANTENNA_UI_DRILL_START_ALL")]
        public void StartStopDrills()
        {
            foreach (Part eachPart in vessel.Parts)
            {
                var baseConverterPart = eachPart.FindModuleImplementing<BaseConverter>();
                if (baseConverterPart != null)
                {
                    if (!resourceConvertersActive)
                        baseConverterPart.StartResourceConverter();
                    else
                        baseConverterPart.StopResourceConverter();
                }
            }
            UpdatePAWMenu();
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#AYA_ANTENNA_UI_DRILL_EXTEND_ALL")]
        public void ExtendRetractAllDrills()
        {
            //Events["ExtendRetractAllDrills"].active = false;
            //AYA_PAW_Refresh.Instance.RefreshPAWMenu(this.part, AYA_PAW_Refresh.AYA_Module.drill, "ExtendRetractAllDrills");

            foreach (Part eachPart in vessel.Parts)
            {
                var moduleAnimationGroupPart = eachPart.FindModuleImplementing<ModuleAnimationGroup>();
                if (moduleAnimationGroupPart != null)
                {
                    if (!drillsDeployed)
                        moduleAnimationGroupPart.DeployModule();
                    else
                        moduleAnimationGroupPart.RetractModule();
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

            //
            // Check to see if any resource converters are active
            //
            resourceConvertersActive = false;

            //
            // Check to see if any drills are deployed
            //
            drillsDeployed = false;

            foreach (Part eachPart in vessel.Parts)
            {
                var baseConverterPart = eachPart.FindModuleImplementing<BaseConverter>();
                if (baseConverterPart != null && baseConverterPart.IsActivated)
                {
                    // Make the assumption here that if the resource converter is active, then the drill must be depoloyed
                    resourceConvertersActive = true;
                    drillsDeployed = true;
                    break;
                }

                var moduleAnimationGroupPart = eachPart.FindModuleImplementing<ModuleAnimationGroup>();
                if (moduleAnimationGroupPart != null && moduleAnimationGroupPart.isDeployed)
                    drillsDeployed = true;
                
            }

            if (resourceConvertersActive)
            {
                // Events["StartStopDrills"].guiName = "Stop all drills";
                Events["StartStopDrills"].guiName = Localizer.Format("#AYA_ANTENNA_UI_DRILL_STOP_ALL");
                Events["ExtendRetractAllDrills"].guiActive = false;
            }
            else
            {
                // Events["StartStopDrills"].guiName = "Start all drills";
                Events["StartStopDrills"].guiName = Localizer.Format("#AYA_ANTENNA_UI_DRILL_START_ALL");
                Events["ExtendRetractAllDrills"].guiActive = true;
            }

            if (drillsDeployed)
            {
                // Events["ExtendRetractAllDrills"].guiName = "Retract All drills";
                Events["ExtendRetractAllDrills"].guiName = Localizer.Format("#AYA_ANTENNA_UI_DRILL_RETRACT_ALL");
                Events["StartStopDrills"].guiActive = true;
            }
            else
            {
                // Events["ExtendRetractAllDrills"].guiName = "Extend All drills";
                Events["ExtendRetractAllDrills"].guiName = Localizer.Format("#AYA_ANTENNA_UI_DRILL_EXTEND_ALL");
                Events["StartStopDrills"].guiActive = false;
            }


        }
    }
}
