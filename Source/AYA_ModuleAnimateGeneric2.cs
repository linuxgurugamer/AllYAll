// ALL Y'ALL
// By 5thHorseman with much help from many others
// License: CC SA

using System.Collections.Generic;



namespace AllYAll
{

    // ############# Generic ModuleAnimateGeneric ############### //

    public class AYA_ModuleAnimateGeneric : ModuleAnimateGeneric
    {
        [KSPField(isPersistant = true)]
        public ModuleDeployablePart.DeployState deployState = ModuleDeployablePart.DeployState.RETRACTED;

        [KSPField]
        public string extendAll = "Extend all parts";

        [KSPField]
        public string retractAll = "Retract all parts";

        [KSPField]
        public float closedPosition = 0;

        [KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "openAll (this should never be seen")]
        public void DoAllIdenticalParts()
        {
            bool partExtended = false;

            List< AYA_ModuleAnimateGeneric> callingPartModules = this.part.FindModulesImplementing<AYA_ModuleAnimateGeneric>();   //Variable for the part doing the work.
            List< AYA_ModuleAnimateGeneric> theseAnimatedParts = this.part.FindModulesImplementing<AYA_ModuleAnimateGeneric>();

            foreach (var callingPartModule in callingPartModules) // Usually only one, but in case a part has multiple AYA_ModuleanimateGenerics
            {
                if (callingPartModule.animationName == this.animationName)
                {
                    foreach (var thisAnimatedPart in theseAnimatedParts)
                    {
                        if (thisAnimatedPart.animationName == this.animationName)
                        {
                            if ((callingPartModule.animTime == 0 && thisAnimatedPart.closedPosition == 1) ||
                                      (callingPartModule.animTime > 0 && thisAnimatedPart.closedPosition == 0))
                            {
                                partExtended = true;                                                      //...then it's open. Duh!
                            }
                        }
                    }
                }
            }

            Events["DoAllIdenticalParts"].active = false;

            AYA_PAW_Refresh.Instance.RefreshPAWMenu(this.part, AYA_PAW_Refresh.AYA_Module.genericAnimation, "DoAllIdenticalParts", this.animationName);

            if (HighLogic.LoadedSceneIsEditor)
            {
                foreach (Part eachPart in EditorLogic.fetch.ship.Parts)                                             //Cycle through each part on the vessel
                {
                    DoIt(eachPart, partExtended);
                }
            }
            else
            {
                foreach (Part eachPart in vessel.Parts)                                             //Cycle through each part on the vessel
                {
                    DoIt(eachPart, partExtended);
                }
            }
        }

        void DoIt(Part eachPart, bool partExtended)
        {
            // Use the lastDeployModuleIndex to avoid activating 2 different AYA_ModuleAnimateGeneric2 modules which
            // both use the same index
            //int lastDeployModuleIndex = -1;

            var thisPartModules = eachPart.FindModulesImplementing<AYA_ModuleAnimateGeneric>();
            foreach (var thisPartModule in thisPartModules)
            {
                //if (thisPartModule != null && lastDeployModuleIndex != thisPartModule.DeployModuleIndex)
                if (thisPartModule.animationName == this.animationName)
                {
                    //lastDeployModuleIndex = thisPartModule.DeployModuleIndex;
                    var thisPartAnimates = eachPart.FindModulesImplementing<AYA_ModuleAnimateGeneric>();
                    foreach (var thisPartAnimate in thisPartAnimates)
                    {
                        if (thisPartAnimate != null && thisPartAnimate.animationName == this.animationName)
                        {
                            KSPActionParam param = new KSPActionParam(KSPActionGroup.Custom01, KSPActionType.Activate);
                            if (partExtended)
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
            var theseModules = this.part.FindModulesImplementing<AYA_ModuleAnimateGeneric>();       //This is so the below code knows the part it's dealing with is a AYA_ModuleAnimateGeneric.
            foreach (var thisPart in theseModules)
            {
                if (thisPart != null && thisPart.animationName == this.animationName)                                                               
                {
                    var thisPartAnimates = this.part.FindModulesImplementing<AYA_ModuleAnimateGeneric>();
                    foreach (var thisPartAnimate in thisPartAnimates)
                    {
                        if (thisPartAnimate != null && thisPartAnimate.animationName == this.animationName)
                        {
                            if (thisPartAnimate.aniState == animationStates.MOVING)
                            {
                                Events["DoAllIdenticalParts"].active = false;
                                break;
                            }
                            //Log.Info("part: " + this.part.partInfo.title + ", animTime: " + thisPartAnimate.animTime + ", closedPosition: " + thisPart.closedPosition + ", animSwitch: " + thisPartAnimate.animSwitch);

                            if ((thisPartAnimate.animTime == 0 && thisPart.closedPosition == 1) ||
                                (thisPartAnimate.animTime > 0 && thisPart.closedPosition == 0))
                            {
                                Events["DoAllIdenticalParts"].guiName = retractAll; // Localizer.Format(retractAll);
                                Events["DoAllIdenticalParts"].active = true;
                            }
                            else
                            {
                                Events["DoAllIdenticalParts"].guiName = extendAll; // Localizer.Format(extendAll);
                                Events["DoAllIdenticalParts"].active = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
