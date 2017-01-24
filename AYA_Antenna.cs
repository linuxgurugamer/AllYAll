// ALL Y'ALL
// By 5thHorseman with much help from many others
// License: CC SA

namespace AllYAll
{

    // ############# ANTENNAE ############### //

    public class AYA_Antenna : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Extend All")]
        public void DoAllAntenna()                                                                //This runs every time you click "extend all" or "retract all"
        {
            bool extended = true;                                                               //This is the check if we are extending or retracting all, default to retracting.
            var callingPart = this.part.FindModuleImplementing<ModuleDeployableAntenna>();   //Variable for the part doing the work.
            if (callingPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)          //If the calling part is retracted...
            {
                extended = false;                                                               //...then it's not extended. Duh!
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

        public void FixedUpdate()                                                               //This runs every second and makes sure the menus are correct.
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR)
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
                    Events["DoAllAntenna"].guiName = "Extend all antennae";                          //Set it to extend.
                    Events["DoAllAntenna"].active = true;
                }
                if (thisPart.retractable && thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDED)  //If it's extended AND retractable...
                {
                    Events["DoAllAntenna"].guiName = "Retract all antennae";                         //set it to retract.
                    Events["DoAllAntenna"].active = true;
                }
            }
        }
    }
}
