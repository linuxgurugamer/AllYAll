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

    // ############# CARGO BAYS ############### //
    
    public class AYA_CargoBay : PartModule
    {

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Open Bays")]
        public void DoAllBays()
        {
            bool cargoBayOpen = false;

            var callingPart = this.part.FindModuleImplementing<ModuleAnimateGeneric>();   //Variable for the part doing the work.
            if (callingPart.animSwitch == true)                                           //If the calling part is open...
            {
                cargoBayOpen = true;                                                      //...then it's open. Duh!
            }

            foreach (Part eachPart in vessel.Parts)
            {
                var thisPart = eachPart.FindModuleImplementing<ModuleCargoBay>();
                if (thisPart != null)
                {
                    var thisPartAnimate = eachPart.FindModuleImplementing<ModuleAnimateGeneric>();
                    if (thisPartAnimate != null)
                    {
                        KSPActionParam param = new KSPActionParam(KSPActionGroup.Custom01, KSPActionType.Activate);
                        if (cargoBayOpen)
                        {
                            if (thisPartAnimate.animSwitch)
                                thisPartAnimate.ToggleAction(param);
                        }
                        else
                        {
                            if (!thisPartAnimate.animSwitch)
                                thisPartAnimate.ToggleAction(param);
                        }
                    }
                }
            }
        }


        public void FixedUpdate()
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR)
                return;

            var thisPart = this.part.FindModuleImplementing<ModuleCargoBay>();                  //This is so the below code knows the part it's dealing with is a cargo bay.
            if (thisPart != null)                                                               //Verify it's actually a cargo bay)
            {
                var thisPartAnimate = this.part.FindModuleImplementing<ModuleAnimateGeneric>();
                if (thisPartAnimate != null)
                {
                    if (thisPartAnimate.aniState == ModuleAnimateGeneric.animationStates.MOVING)
                    {
                        Events["DoAllBays"].active = false;
                    }
                    if (thisPartAnimate.animSwitch)
                    {
                        // Events["DoAllBays"].guiName = "Close all bays";
                        Events["DoAllBays"].guiName = Localizer.Format("#AYA_ANTENNA_UI_CARGO_BAYS_CLOSE_ALL");
                        Events["DoAllBays"].active = true;
                    }
                    else
                    {
                        // Events["DoAllBays"].guiName = "Open all bays";
                        Events["DoAllBays"].guiName = Localizer.Format("#AYA_ANTENNA_UI_CARGO_BAYS_OPEN_ALL");
                        Events["DoAllBays"].active = true;
                    }
                }
            }
        }
    }  
}
