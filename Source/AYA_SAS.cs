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

    // ############# REACTION WHEELS ############### //

    public class AYA_SAS : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Activate All")]
        public void DoAllReactionWheel()
        {
            bool active = false;                                                                //This is the check if we are activating or deactivating reactio wheels.
            var callingPart = this.part.FindModuleImplementing<ModuleReactionWheel>();          //Variable for the part doing the work.
            if (callingPart.State == ModuleReactionWheel.WheelState.Active)                     //If the calling part is active...
            {
                active = true;                                                                 //...then it's active. Duh!
            }
            foreach (Part eachPart in vessel.Parts)
            {
                var thisPart = eachPart.FindModuleImplementing<ModuleReactionWheel>();
                KSPActionParam kap = new KSPActionParam(KSPActionGroup.Custom01, KSPActionType.Activate);
                if (thisPart != null)
                {
                    if (active)
                        thisPart.Deactivate(kap);
                    else
                        thisPart.Activate(kap);
                }
            }
        }


        public void FixedUpdate()
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR)
                return;

            var thisPart = this.part.FindModuleImplementing<ModuleReactionWheel>();      //This is so the below code knows the part it's dealing with is a Reaction Wheel.
            if (thisPart != null)
            {
                if (thisPart.State == ModuleReactionWheel.WheelState.Active)
                {
                    Events["DoAllReactionWheel"].guiName = "Deactivate All SAS";
                }
                else
                {
                    Events["DoAllReactionWheel"].guiName = "Activate All SAS";
                }
            }
        }
    }
}