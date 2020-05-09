#if false
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

    // ############# ModuleAnimateGeneric ############### //

    public class AYA_ModuleAnimateGeneric : ModuleAnimateGeneric
    {
        [KSPField(isPersistant = true)]
        public ModuleDeployablePart.DeployState deployState = ModuleDeployablePart.DeployState.RETRACTED;

        [KSPField]
        public bool retractable = true;

        [KSPField]
        public string extendAll = "Extend all parts";

        [KSPField]
        public string retractAll = "Retract all parts";

        [KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#AYA_ANTENNA_UI_ANTENNA_EXTEND_ALL")]
        public void DoAllParts()                                                                //This runs every time you click "extend all" or "retract all"
        {
            if (this.animationName == "")
                return;
            bool extended = true;                                                               //This is the check if we are extending or retracting all, default to retracting.
            AYA_ModuleAnimateGeneric callingPart = this.part.FindModuleImplementing<AYA_ModuleAnimateGeneric>();   //Variable for the part doing the work.

            if (callingPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)          //If the calling part is retracted...
            {
                extended = false;                                                               //...then it's not extended. Duh!
            }
            foreach (Part eachPart in vessel.Parts)                                             //Cycle through each part on the vessel
            {
                AYA_ModuleAnimateGeneric thisPartAnimate = eachPart.FindModuleImplementing<AYA_ModuleAnimateGeneric>();   //If it's an antenna...
                if (thisPartAnimate != null && thisPartAnimate.animationName == this.animationName)                           //..and it has an animation (rules out ox-stats and the like)
                {
                    KSPActionParam param = new KSPActionParam(KSPActionGroup.Custom01, KSPActionType.Activate);
                    if (extended)                                                               //then if the calling part was extended...
                    {
                        //Retract it
                        //thisPartAnimate.ToggleAction(param);
                        thisPartAnimate.Toggle();
                        callingPart.deployState = ModuleDeployablePart.DeployState.RETRACTING;
                    }
                    else                                                                        //otherwise...
                    {
                        if (callingPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)
                        {
                            //thisPartAnimate.ToggleAction(param);
                            thisPartAnimate.Toggle();
                            callingPart.deployState = ModuleDeployablePart.DeployState.EXTENDING;                                                     //Extend it
                        }
                    }
                }
                

            }
        }

        void Start()
        {
            Events["DoAllParts"].guiName = Localizer.Format(extendAll);
            OnStop.Add(doOnStop);
        }
        void OnDestroy()
        {
            OnStop.Remove(doOnStop);
        }
        void doOnStop(float f1)
        {
            Log.Info("doOnStop, deployState: " + deployState);
            switch (deployState)
            {
                case ModuleDeployablePart.DeployState.EXTENDING:
                    deployState = ModuleDeployablePart.DeployState.EXTENDED;
                    break;
                case ModuleDeployablePart.DeployState.RETRACTING:
                    deployState = ModuleDeployablePart.DeployState.RETRACTED;
                    break;
            }
        }
        public void FixedUpdate()                                                               //This runs every second and makes sure the menus are correct.
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR || this.animationName == "")
                return;

            var thisPart = this.part.FindModuleImplementing<AYA_ModuleAnimateGeneric>();      //This is so the below code knows the part it's dealing with is an antenna.
            if (thisPart != null && thisPart.animationName == this.animationName)                               //Verify it's actually an antenna and has an animation (rules out ox-stats and the like)
            {
                if (thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDING ||
                    thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTING)        //If it's extending or retracting...
                {
                    Events["DoAllParts"].active = false;                                        //...you don't get no menu option!
                }

                if (thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)         //If it's retracted...
                {
                    Events["DoAllParts"].guiName = Localizer.Format(extendAll);    //Set it to extend.
                    Events["DoAllParts"].active = true;
                }
                if (thisPart.retractable && thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDED)  //If it's extended AND retractable...
                {
                    Events["DoAllParts"].guiName = Localizer.Format(retractAll);      //set it to retract.
                    Events["DoAllParts"].active = true;
                }
            }
        }
    }
}
#endif