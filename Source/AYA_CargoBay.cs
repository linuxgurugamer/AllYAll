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

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#AYA_ANTENNA_UI_CARGO_BAYS_OPEN_ALL")]
        public void DoAllBays()
        {
            bool cargoBayOpen = false;

            var callingPart = this.part.FindModuleImplementing<ModuleAnimateGeneric>();   //Variable for the part doing the work.
            var thisPartCargoBay = this.part.FindModuleImplementing<ModuleCargoBay>();

            if ((callingPart.animSwitch & thisPartCargoBay.closedPosition == 1) ||
                      (!callingPart.animSwitch & thisPartCargoBay.closedPosition == 0))
            {
                cargoBayOpen = true;                                                      //...then it's open. Duh!
            }

            foreach (Part eachPart in vessel.Parts)
            {
                var thisPartModule = eachPart.FindModuleImplementing<ModuleCargoBay>();
                if (thisPartModule != null)
                {
                    var thisPartAnimate = eachPart.FindModuleImplementing<ModuleAnimateGeneric>();
                    if (thisPartAnimate != null)
                    {
                        KSPActionParam param = new KSPActionParam(KSPActionGroup.Custom01, KSPActionType.Activate);
                        if (cargoBayOpen)
                        {
                            if ((thisPartAnimate.animSwitch & thisPartModule.closedPosition == 1) ||
                                (!thisPartAnimate.animSwitch & thisPartModule.closedPosition == 0))
                            {
                                thisPartAnimate.ToggleAction(param);
                            }
                        }
                        else
                        {
                            if ((!thisPartAnimate.animSwitch & thisPartModule.closedPosition == 1) ||
                                (thisPartAnimate.animSwitch & thisPartModule.closedPosition == 0))
                            {
                                 thisPartAnimate.ToggleAction(param);
                            }
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
                    if (( thisPartAnimate.animSwitch & thisPart.closedPosition == 1) ||
                        (!thisPartAnimate.animSwitch & thisPart.closedPosition == 0))
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
