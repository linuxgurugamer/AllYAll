// ALL Y'ALL
// By 5thHorseman with much help from many others
// License: CC SA

using KSP.Localization;

namespace AllYAll
{

    // ############# REACTION WHEELS ############### //

    public class AYA_SAS : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#AYA_ANTENNA_UI_SAS_ACTIVATE_ALL")]
        public void DoAllReactionWheel()
        {
            bool active = false;                                                                //This is the check if we are activating or deactivating reactio wheels.
            var callingPart = this.part.FindModuleImplementing<ModuleReactionWheel>();          //Variable for the part doing the work.
            if (callingPart.State == ModuleReactionWheel.WheelState.Active)                     //If the calling part is active...
            {
                active = true;                                                                 //...then it's active. Duh!
            }


           // Events["DoAllReactionWheel"].active = false;
           // AYA_PAW_Refresh.Instance.RefreshPAWMenu(this.part, AYA_PAW_Refresh.AYA_Module.sas, "DoAllReactionWheel");


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

            UpdatePAWMenu();
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

            var thisPart = this.part.FindModuleImplementing<ModuleReactionWheel>();      //This is so the below code knows the part it's dealing with is a Reaction Wheel.
            if (thisPart != null)
            {
                if (thisPart.State == ModuleReactionWheel.WheelState.Active)
                {
                    // Events["DoAllReactionWheel"].guiName = "Deactivate All SAS";
                    Events["DoAllReactionWheel"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SAS_DEACTIVATE_ALL");
                }
                else
                {
                    // Events["DoAllReactionWheel"].guiName = "Activate All SAS";
                    Events["DoAllReactionWheel"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SAS_ACTIVATE_ALL");
                }
            }
        }
    }
}