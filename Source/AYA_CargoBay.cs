// ALL Y'ALL
// By 5thHorseman with much help from many others
// License: CC SA


using UnityEngine;
using KSP.Localization;

namespace AllYAll
{

    // ############# CARGO BAYS ############### //

    public class AYA_CargoBay : PartModule
    {

        [KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#AYA_ANTENNA_UI_CARGO_BAYS_OPEN_ALL")]
        public void DoAllBays()
        {
            bool cargoBayOpen = false;

            var callingParts = this.part.FindModulesImplementing<ModuleAnimateGeneric>();   //Variable for the part doing the work.
            var thisPartCargoBays = this.part.FindModulesImplementing<ModuleCargoBay>();

            foreach (var callingPart in callingParts)
            {
                foreach (var thisPartCargoBay in thisPartCargoBays)
                {
                    if ((callingPart.animTime == 0 && thisPartCargoBay.closedPosition == 1) ||
                              (callingPart.animTime >0 && thisPartCargoBay.closedPosition == 0))
                    {
                        cargoBayOpen = true;                                                      //...then it's open. Duh!
                    }
                }
            }

            Events["DoAllBays"].active = false;
            AYA_PAW_Refresh.Instance.RefreshPAWMenu(this.part, AYA_PAW_Refresh.AYA_Module.cargo, "DoAllBays");

            if (HighLogic.LoadedSceneIsEditor)
            {
                foreach (Part eachPart in EditorLogic.fetch.ship.Parts)                                             //Cycle through each part on the vessel
                {
                    DoIt(eachPart, cargoBayOpen);
                }
            }
            else
            {
                foreach (Part eachPart in vessel.Parts)                                             //Cycle through each part on the vessel
                {
                    DoIt(eachPart, cargoBayOpen);
                }
            }
        }
        void DoIt(Part eachPart, bool cargoBayOpen)
        {
            // Use the lastDeployModuleIndex to avoid activating 2 different ModuleCargoBay modules which
            // both use the same index
            int lastDeployModuleIndex = -1;

            var thisPartModules = eachPart.FindModulesImplementing<ModuleCargoBay>();
            foreach (var thisPartModule in thisPartModules)
            {
                if (thisPartModule != null && lastDeployModuleIndex != thisPartModule.DeployModuleIndex)
                {
                    lastDeployModuleIndex = thisPartModule.DeployModuleIndex;
                    var thisPartAnimates = eachPart.FindModulesImplementing<ModuleAnimateGeneric>();
                    foreach (var thisPartAnimate in thisPartAnimates)
                    {
                        if (thisPartAnimate != null)
                        {
                            KSPActionParam param = new KSPActionParam(KSPActionGroup.Custom01, KSPActionType.Activate);
                            if (cargoBayOpen)
                            {
                                if ((thisPartAnimate.animTime == 0 && thisPartModule.closedPosition == 1) ||
                                    (thisPartAnimate.animTime > 0 && thisPartModule.closedPosition == 0))
                                {
                                    thisPartAnimate.ToggleAction(param);
                                }
                            }
                            else
                            {
                                if ((thisPartAnimate.animTime > 0 && thisPartModule.closedPosition == 1) ||
                                    (thisPartAnimate.animTime == 0 && thisPartModule.closedPosition == 0))
                                {
                                    thisPartAnimate.ToggleAction(param);
                                }
                            }
                        }
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

        public void UpdatePAWMenu()
        {
            //if (HighLogic.LoadedScene != GameScenes.FLIGHT)
            //    return;

            var thisParts = this.part.FindModulesImplementing<ModuleCargoBay>();                  //This is so the below code knows the part it's dealing with is a cargo bay.
            foreach (var thisPart in thisParts)
            {
                if (thisPart != null)                                                               //Verify it's actually a cargo bay)
                {
                    var thisPartAnimates = this.part.FindModulesImplementing<ModuleAnimateGeneric>();
                    foreach (var thisPartAnimate in thisPartAnimates)
                    {
                        if (thisPartAnimate != null)
                        {
                            if (thisPartAnimate.aniState == ModuleAnimateGeneric.animationStates.MOVING)
                            {
                                Events["DoAllBays"].active = false;
                                break;
                            }
                            Debug.Log("part: " + this.part.partInfo.title + ", animTime: " + thisPartAnimate.animTime + ", closedPosition: " + thisPart.closedPosition + ", animSwitch: " + thisPartAnimate.animSwitch);

                            if ((thisPartAnimate.animTime == 0 && thisPart.closedPosition == 1) ||
                                (thisPartAnimate.animTime>0 && thisPart.closedPosition == 0))
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
    }  
}
