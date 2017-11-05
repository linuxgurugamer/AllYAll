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

    public class AYA_Drill : PartModule
    {
        bool resourceConvertersActive = false;
        bool drillsDeployed = false;

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Start All")]
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
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Extend All")]
        public void ExtendRetractAllDrills()
        {
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


        public void FixedUpdate()
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR)
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
                Events["StartStopDrills"].guiName = "Stop all drills";
                Events["ExtendRetractAllDrills"].guiActive = false;
            }
            else
            {
                Events["StartStopDrills"].guiName = "Start all drills";
                Events["ExtendRetractAllDrills"].guiActive = true;
            }

            if (drillsDeployed)
            {
                Events["ExtendRetractAllDrills"].guiName = "Retract All drills";
                Events["StartStopDrills"].guiActive = true;
            }
            else
            {
                Events["ExtendRetractAllDrills"].guiName = "Extend All drills";
                Events["StartStopDrills"].guiActive = false;
            }


        }
    }
}
