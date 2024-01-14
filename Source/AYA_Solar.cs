// ALL Y'ALL
// By 5thHorseman with much help from many others
// License: CC SA

using System;
using UnityEngine;
using KSP.Localization;

using System.Reflection;



namespace AllYAll
{
    // ############# SOLAR PANELS ############### //

    public class AYA_Solar : PartModule
    {
        //NFSWrapper.NFSCurvedPanel nfsWrapper = null;
        static bool NFSPresent;
        NFSWrapper.NFSCurvedPanel nfsCurvedPanelModule;

        [KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#AYA_ANTENNA_UI_SOLAR_EXTEND_ALL")]
        public void DoAllSolar()                                                                //This runs every time you click "extend all" or "retract all"
        {

            bool extended = true;                                                               //This is the check if we are extending or retracting all, default to retracting.
            var callingPart = this.part.FindModuleImplementing<ModuleDeployableSolarPanel>();   //Variable for the part doing the work.
            if (callingPart != null && callingPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)          //If the calling part is retracted...
            {
                extended = false;                                                               //...then it's not extended. Duh!
            }
            //
            // Same thing for the NearFuture Solar, if it's loaded
            //
            if (NFSPresent)
            {
                nfsCurvedPanelModule = NFSWrapper.NFSCurvedPanel.GetNFSModule(this.part);

                if (nfsCurvedPanelModule != null)
                {
                    if (nfsCurvedPanelModule.DeployState == ModuleDeployablePart.DeployState.RETRACTED)
                        extended = false;
                }               
            }

            Events["DoAllSolar"].active = false;
            AYA_PAW_Refresh.Instance.RefreshPAWMenu(this.part, AYA_PAW_Refresh.AYA_Module.solar, "DoAllSolar");
            if (HighLogic.LoadedSceneIsEditor)
            {
                foreach (Part eachPart in EditorLogic.fetch.ship.Parts)                                             //Cycle through each part on the vessel
                {
                    DoIt(eachPart, extended);
                }
            }
            else
            {
                foreach (Part eachPart in vessel.Parts)                                             //Cycle through each part on the vessel
                {
                    DoIt(eachPart, extended);
                }
            }
        }
        
        void DoIt(Part eachPart, bool extended)
        {
            var thisPart = eachPart.FindModuleImplementing<ModuleDeployableSolarPanel>();   //If it's a solar panel...
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


            //
            // Now do the nearFutureSolar
            //
            if (NFSPresent)
            {
                nfsCurvedPanelModule = NFSWrapper.NFSCurvedPanel.GetNFSModule(eachPart);
                if (nfsCurvedPanelModule != null && nfsCurvedPanelModule.isDeployable)                           //..and it has an animation (rules out ox-stats and the like)
                {
                    if (extended)                                                               //then if the calling part was extended...
                    {
                        nfsCurvedPanelModule.Retract();                                                     //Retract it
                    }
                    else                                                                        //otherwise...
                    {
                        nfsCurvedPanelModule.Deploy();                                                      //Extend it
                    }
                }

            }
        }
        public void Start()
        {
            NFSPresent = NFSWrapper.AssemblyExists;
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

        public void UpdatePAWMenu()                                                               //This runs every second and makes sure the menus are correct.
        {
            if (HighLogic.LoadedScene != GameScenes.FLIGHT)
                return;

            var thisPart = this.part.FindModuleImplementing<ModuleDeployableSolarPanel>();      //This is so the below code knows the part it's dealing with is a solar panel.
            if (thisPart != null && thisPart.animationName != "")                               //Verify it's actually a solar panel and has an animation (rules out ox-stats and the like)
            {
                if (thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDING ||
                    thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTING)        //If it's extending or retracting...
                {
                    Events["DoAllSolar"].active = false;                                        //...you don't get no menu option!
                }

                if (thisPart.deployState == ModuleDeployablePart.DeployState.RETRACTED)         //If it's retracted...
                {
                    // Events["DoAllSolar"].guiName = "Extend all solar";                          //Set it to extend.
                    Events["DoAllSolar"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SOLAR_EXTEND_ALL");                         //Set it to extend.
                    Events["DoAllSolar"].active = true;
                }
                if (thisPart.retractable && thisPart.deployState == ModuleDeployablePart.DeployState.EXTENDED)  //If it's extended AND retractable...
                {
                    // Events["DoAllSolar"].guiName = "Retract all solar";                         //set it to retract.
                    Events["DoAllSolar"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SOLAR_RETRACT_ALL");                         //set it to retract.
                    Events["DoAllSolar"].active = true;
                }
            }

            if (NFSPresent)
            {
                nfsCurvedPanelModule = NFSWrapper.NFSCurvedPanel.GetNFSModule(this.part);
                if (nfsCurvedPanelModule != null)
                {

 
                    if (nfsCurvedPanelModule.DeployState == ModuleDeployablePart.DeployState.EXTENDING ||
                        nfsCurvedPanelModule.DeployState == ModuleDeployablePart.DeployState.RETRACTING)        //If it's extending or retracting...
                    {
                        Events["DoAllSolar"].active = false;                                        //...you don't get no menu option!
                    }

                    if (nfsCurvedPanelModule.DeployState == ModuleDeployablePart.DeployState.RETRACTED)         //If it's retracted...
                    {
                        // Events["DoAllSolar"].guiName = "Extend all solar";                          //Set it to extend.
                        Events["DoAllSolar"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SOLAR_EXTEND_ALL");                         //Set it to extend.
                        Events["DoAllSolar"].active = true;
                    }
                    if (nfsCurvedPanelModule.DeployState == ModuleDeployablePart.DeployState.EXTENDED)  //If it's extended AND retractable...
                    {
                        // Events["DoAllSolar"].guiName = "Retract all solar";                         //set it to retract.
                        Events["DoAllSolar"].guiName = Localizer.Format("#AYA_ANTENNA_UI_SOLAR_RETRACT_ALL");                         //set it to retract.
                        Events["DoAllSolar"].active = true;
                    }
                }
            }
        }
    }



    /// <summary>
    /// The Wrapper class to access Near Future Solar
    /// </summary>
    public class NFSWrapper
    {
        internal static System.Type NFSCurvedsolarPanelType;

        /// <summary>
        /// Whether we found the Near Future Solar assembly in the loadedassemblies.
        ///
        /// SET AFTER INIT
        /// </summary>
        public static Boolean AssemblyExists {
            get {
                if (_NFSWrapped == null) InitNFSWrapper();
                return ((bool)_NFSWrapped == true && NFSCurvedsolarPanelType != null);
            }
        }

        /// <summary>
        /// Whether we managed to wrap all the methods/functions from the instance.
        ///
        /// SET AFTER INIT
        /// </summary>
        private static Boolean? _NFSWrapped;
        

        /// <summary>
        /// This method will set up the Near Future Solar object and wrap all the methods/functions
        /// </summary>
        /// <returns></returns>
        public static Boolean InitNFSWrapper()
        {
            //reset the internal objects
            _NFSWrapped = false;
            Log.Info("Attempting to Grab Near Future Solar Types...");

            //find the NFSCurvedsolarPanelType type
            NFSCurvedsolarPanelType = getType("NearFutureSolar.ModuleCurvedSolarPanel");

            if (NFSCurvedsolarPanelType == null)
            {
                return false;
            }

            Log.Info("Near Future Solar Version: "+ NFSCurvedsolarPanelType.Assembly.GetName().Version.ToString());

            _NFSWrapped = true;
            return true;
        }

        internal static Type getType(string name)
        {
            Type type = null;
            AssemblyLoader.loadedAssemblies.TypeOperation(t =>

            {
                if (t.FullName == name)
                    type = t;
            }
            );

            if (type != null)
            {
                return type;
            }
            return null;
        }

        public class NFSCurvedPanel
        {
            System.Object actualNFSCurvedPanel;
            FieldInfo animationName; 
            FieldInfo Deployable;
            PropertyInfo deployState;
            MethodInfo DeployMethod;
            MethodInfo RetractMethod;

            internal NFSCurvedPanel(System.Object a)
            {
                actualNFSCurvedPanel = a;
                

                DeployMethod = NFSCurvedsolarPanelType.GetMethod("Deploy", BindingFlags.Public | BindingFlags.Instance);
                RetractMethod = NFSCurvedsolarPanelType.GetMethod("Retract", BindingFlags.Public | BindingFlags.Instance);

                animationName = NFSCurvedsolarPanelType.GetField("DeployAnimation");

                deployState = NFSCurvedsolarPanelType.GetProperty("State");
                Deployable = NFSCurvedsolarPanelType.GetField("Deployable");
            }

            static public  NFSWrapper.NFSCurvedPanel GetNFSModule(Part part)
            {
                foreach (var m in part.Modules)
                {
                    if (m.moduleName == "ModuleCurvedSolarPanel")
                    {
                        return  new NFSWrapper.NFSCurvedPanel(m);
                    }
                }
                return null;
            }

            public string AnimationName
            {
                get { return (string)animationName.GetValue(actualNFSCurvedPanel); }
            }
            public ModuleDeployablePart.DeployState DeployState
            {
                get
                {
                    System.Object tmpObj = (System.Object)deployState.GetValue(actualNFSCurvedPanel, null);
                    ModuleDeployablePart.DeployState tmpState = (ModuleDeployablePart.DeployState)tmpObj;
                    return tmpState;
                }
            }

            public bool isDeployable
            {
                get { return (bool)Deployable.GetValue(actualNFSCurvedPanel); }
            }




            /// <summary>
            /// Deploy Panels
            /// </summary>
            /// <returns>Bool indicating success of call</returns>
            public bool Deploy()
            {
                try
                {
                    DeployMethod.Invoke(actualNFSCurvedPanel, null);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error("Deploy Failed: "+ ex.Message);
                    return false;
                }
            }


            /// <summary>
            /// Retract Panels
            /// </summary>
            /// <returns>Bool indicating success of call</returns>
            public bool Retract()
            {
                try
                {
                    RetractMethod.Invoke(actualNFSCurvedPanel, null);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error("Retract Failed: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
