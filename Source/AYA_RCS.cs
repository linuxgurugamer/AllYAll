// ALL Y'ALL
// By 5thHorseman with much help from many others
// License: CC SA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using static VehiclePhysics.VPAudio;

namespace AllYAll
{
    public static class Extensions
    {

        // Following method copied from RealChute, with improvements
        /// <summary>
        /// Gets all the children of a part and their children
        /// </summary>
        /// <param name="part">Part to get the children for</param>
        public static List<Part> GetAllChildren(this Part part, bool print = true)
        {
#if DEBUG
            if (print)
                Log.Info("GetAllChildren, part: " + part.partInfo.title + ", persistentId: " + part.persistentId);
#endif
            if (part.children.Count <= 0)
            {
                return new List<Part>();
            }

            //Thanks to Padishar here
            List<Part> result = new List<Part>(part.children);
            for (int i = 0; i < result.Count; i++)
            {
                Part p = result[i];
                if (p.children.Count > 0)
                {
                    result.AddRange(p.children);
                    for (int j = 0; j < p.children.Count; j++)
                    {
                        Part child = p.children[j];
                        result.AddRange(child.GetAllChildren(print));
                    }
                }
            }
#if DEBUG

            if (print)
            {
                foreach (var r in result)
                {
                    Log.Info("GetAllChildren, child: " + r.partInfo.title + ", persistentId: " + r.persistentId);
                }
            }
#endif
            return result;
        }
    }



    // ############# RCS ############### //



    public class AYA_RCS : PartModule
    {
        [UI_FloatRange(requireFullControl = true, stepIncrement = 0.5f, maxValue = 100f, minValue = 0f)]
        [KSPAxisField(minValue = 0f, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 100f, guiActive = true, guiActiveEditor = true, guiName = "RCS Stage Thrust Limiter", groupStartCollapsed = true, groupName = "RCSControl", groupDisplayName = "RCS Control")]
        public float rcsStageThrustPercentage = 100f;

        [UI_FloatRange(requireFullControl = true, stepIncrement = 0.5f, maxValue = 100f, minValue = 0f)]
        [KSPAxisField(minValue = 0f, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 100f, guiActive = true, guiActiveEditor = true, guiName = "RCS Global Thrust Limiter", groupStartCollapsed = true, groupName = "RCSControl", groupDisplayName = "RCS Control")]
        public float rcsGlobalThrustPercentage = 100f;

        Dictionary<uint, Part> usedParts = new Dictionary<uint, Part>();
        Dictionary<int, Dictionary<uint, Part>> stageParts = new Dictionary<int, Dictionary<uint, Part>>();

        public void UI_Event_SetGlobalRCSThrust(BaseField field, object what)
        {
            rcsStageThrustPercentage = rcsGlobalThrustPercentage;
            part.GetAllChildren();

            for (int i = 0; i < vessel.Parts.Count; i++)
            {
                for (int j = 0; j < vessel.Parts[i].Modules.Count; j++)
                {
                    PartModule pm = vessel.Parts[i].Modules[j]; //change from part to partmodules

                    if (pm.moduleName == "ModuleRCS") //find partmodule RCS on th epart
                    {
                        var moduleRCS = (ModuleRCS)pm;
                        moduleRCS.thrustPercentage = rcsGlobalThrustPercentage;
                    }
                    else if (pm.moduleName == "ModuleRCSFX") //find partmodule RCS on th epart
                    {
                        var moduleRCSFX = (ModuleRCSFX)pm;
                        moduleRCSFX.thrustPercentage = rcsGlobalThrustPercentage;
                    }
                }
            }
        }

        public void UI_Event_SetStageRCSThrust(BaseField field, object what)
        {
            var curStage = FlightGlobals.ActiveVessel.currentStage - 1;

            GetAllPartsInEachStage();

            // First, find out if any engines in this stage
            int engineOffset = 0;
            int curStageOffset = 0;

            foreach (Part part in stageParts[curStage].Values)
            {
#if false
                Log.Info("Part: " + part.partInfo.title + ", inverseStage: " + part.inverseStage + ", stageOffset: " + part.stageOffset + ", curStage: " + curStage + ", engineOffset: " + engineOffset);
#endif
                if (part.inverseStage + engineOffset >= curStage + curStageOffset)
                {
                    for (int j = 0; j < part.Modules.Count; j++)
                    {
                        PartModule pm = part.Modules[j]; //change from part to partmodules
                        {
                            if (pm.moduleName == "ModuleRCS") //find partmodule RCS on th epart
                            {
                                var moduleRCS = (ModuleRCS)pm;
                                moduleRCS.thrustPercentage = rcsStageThrustPercentage;
                            }
                            else if (pm.moduleName == "ModuleRCSFX") //find partmodule RCS on th epart
                            {
                                var moduleRCSFX = (ModuleRCSFX)pm;
                                moduleRCSFX.thrustPercentage = rcsStageThrustPercentage;
                            }
                        }
                    }
                }
            }
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Shutdown all RCSs",  groupStartCollapsed = true, groupName = "RCSControl", groupDisplayName = "RCS Control")]  //  "#AYA_RCS_UI_DEACTIVATE_ALL")]
        public void DeactivateAllRCSs()
        {
            Log.Info("DeactivateAllRCSs, part: " + part.partInfo.title + ", id: " + part.persistentId);
            part.GetAllChildren();

            for (int i = 0; i < vessel.Parts.Count; i++)
            {
                for (int j = 0; j < vessel.Parts[i].Modules.Count; j++)
                {
                    PartModule pm = vessel.Parts[i].Modules[j]; //change from part to partmodules

                    if (pm.moduleName == "ModuleRCS") //find partmodule RCS on the part
                    {
                        var moduleRCS = (ModuleRCS)pm;
                        moduleRCS.rcsEnabled = false;
                    }
                    else if (pm.moduleName == "ModuleRCSFX") //find partmodule RCS on th epart
                    {
                        var moduleRCSFX = (ModuleRCSFX)pm;
                        moduleRCSFX.rcsEnabled = false;
                    }
                }
            }
        }

#if false
        void DumpVesselStagingInfo(Vessel v)
        {
            foreach (Part part in v.Parts)
            {
                if (part.parent != null)
                    Log.Info("Part: " + part.partInfo.title + ", inverseStage: " + part.inverseStage + ", stageOffset: " + part.stageOffset + ", parent: " + part.parent.partInfo.title + ", parent.inverseStage: " + part.parent.inverseStage + ", parent.stageOffset: " + part.parent.stageOffset);
                else
                    Log.Info("Part: " + part.partInfo.title + ", inverseStage: " + part.inverseStage + ", stageOffset: " + part.stageOffset);
            }
        }
#endif


        //int stage;

        void ProcessDecoupler(ModuleDecouplerBase d, int stage)
        {
            if (d.part.inverseStage == stage)
            {
                if (!usedParts.ContainsKey(d.part.persistentId))
                {
                    // Add decoupler(s) to list of used parts
                    // Add decoupler(s) to list of parts used in stage
                    usedParts.Add(d.part.persistentId, d.part);
                    stageParts[stage].Add(d.part.persistentId, d.part);
                }

                // For each decoupler, find all children.
                var result = d.part.GetAllChildren(false);
                foreach (var c in result)
                {
                    if (!usedParts.ContainsKey(c.persistentId))
                    {
                        usedParts.Add(c.persistentId, c);
                        stageParts[stage].Add(c.persistentId, c);
                    }
                }
            }
        }

        void GetAllPartsInEachStage()
        {
            int stage = vessel.currentStage; // (need to verify this is the same as the inverseStage in the part)

            List<ModuleDecouple> decouplers = vessel.FindPartModulesImplementing<ModuleDecouple>();
            List<ModuleAnchoredDecoupler> anchoredDecouplers = vessel.FindPartModulesImplementing<ModuleAnchoredDecoupler>();
            //List<ModuleDecouplerBase> allDecouplers = vessel.FindPartModulesImplementing<ModuleDecouplerBase>();

#if false
            Log.Info("decouplers.Count: " + decouplers.Count);
            Log.Info("anchoredDecouplers.Count: " + anchoredDecouplers.Count);
            //Log.Info("allDecouplers.Count: " + allDecouplers.Count);
#endif
            usedParts.Clear();
            stageParts.Clear();
#if false
            Log.Info("--------------------------------");
            Log.Info("All part");
            foreach (var p in vessel.parts)
            {
                Log.Info("Part: " + p.partInfo.title + ", inverseStage: " + p.inverseStage);
            }
            Log.Info("--------------------------------");
#endif
            while (stage >= -1)
            {
                stageParts[stage] = new Dictionary<uint, Part>();
                for (int i = 0; i < decouplers.Count; i++)
                        ProcessDecoupler(decouplers[i], stage);
                for (int i = 0; i < anchoredDecouplers.Count;i++)
                        ProcessDecoupler(anchoredDecouplers[i], stage);

#if false
                if (stage > -1)
                {
                    Log.Info("--------------------------------");
                    Log.Info("Part in Stage: " + stage + ", Part: " + part.partInfo.title);
                    foreach (var s in stageParts[stage].Values)
                    {
                        Log.Info("Part: " + s.partInfo.title);
                    }
                }
#endif
                stage--;
            }
            for (int i = 0;i < vessel.Parts.Count;i++)
            {
                Part p = vessel.parts[i];
            
                if (!(usedParts.ContainsKey(p.persistentId)))
                    stageParts[-1].Add(p.persistentId, p);
            }
#if false
            Log.Info("--------------------------------");
            Log.Info("Part in Stage: " + -1 + ", Part: " + part.partInfo.title);
            foreach (var s in stageParts[-1].Values)
            {
                Log.Info("Part: " + s.partInfo.title);
            }
#endif

        }

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Activate all RCSs in active stage", groupStartCollapsed = true, groupName = "RCSControl", groupDisplayName = "RCS Control")]  //  "#AYA_RCS_UI_DEACTIVATE_ALL")]
        public void ActivateAllRCSsInActiveStage()
        {
            var curStage = FlightGlobals.ActiveVessel.currentStage - 1;

            //DumpVesselStagingInfo(vessel);

            GetAllPartsInEachStage();

            foreach (Part part in stageParts[curStage].Values)
            {
#if false
                Log.Info("Part: " + part.partInfo.title + ", inverseStage: " + part.inverseStage + ", stageOffset: " + part.stageOffset + ", curStage: " + curStage + ", engineOffset: " + engineOffset);
#endif
                if (part.inverseStage >= curStage)
                {
                    for (int i = 0; i < part.Modules.Count; i++)
                    {
                        PartModule pm = part.Modules[i]; //change from part to partmodules
                    
                        if (pm.moduleName == "ModuleRCS") //find partmodule RCS on th epart
                        {
                            var moduleRCS = (ModuleRCS)pm;
                            moduleRCS.rcsEnabled = true;
                        }
                        else if (pm.moduleName == "ModuleRCSFX") //find partmodule RCS on th epart
                        {
                            var moduleRCSFX = (ModuleRCSFX)pm;
                            moduleRCSFX.rcsEnabled = true;
                        }
                    }
                }
            }
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Activate all RCSs in current stage", groupStartCollapsed = true, groupName = "RCSControl", groupDisplayName = "RCS Control")]  //  "#AYA_RCS_UI_DEACTIVATE_ALL")]
        public void ActivateAllRCSsInCurrentStage()
        {
            var curStage = FlightGlobals.ActiveVessel.currentStage - 1;
            curStage = part.inverseStage;

            //DumpVesselStagingInfo(vessel);

            GetAllPartsInEachStage();


            foreach (Part part in stageParts[curStage].Values)
            {
#if false
                Log.Info("Part: " + part.partInfo.title + ", inverseStage: " + part.inverseStage + ", stageOffset: " + part.stageOffset + ", curStage: " + curStage + ", engineOffset: " + engineOffset);
#endif
                if (part.inverseStage == curStage)
                {
                    for (int i = 0; i < part.Modules.Count; i++)
                    {
                        PartModule pm = part.Modules[i]; //change from part to partmodules

                        if (pm.moduleName == "ModuleRCS") //find partmodule RCS on th epart
                        {
                            var moduleRCS = (ModuleRCS)pm;
                            moduleRCS.rcsEnabled = true;
                        }
                        else if (pm.moduleName == "ModuleRCSFX") //find partmodule RCS on th epart
                        {
                            var moduleRCSFX = (ModuleRCSFX)pm;
                            moduleRCSFX.rcsEnabled = true;
                        }
                    }
                }
            }
        }


        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Activate all RCSs", groupStartCollapsed = true, groupName = "RCSControl", groupDisplayName = "RCS Control")]  //  "#AYA_RCS_UI_DEACTIVATE_ALL")]
        public void ActivateAllRCSs()
        {
            var curStage = FlightGlobals.ActiveVessel.currentStage - 1;
            for (int j = 0; j < vessel.Parts.Count; j++)
            {
                Part part = vessel.Parts[j];
            
                for (int i = 0; i < part.Modules.Count; i++)
                {
                    PartModule pm = part.Modules[i]; //change from part to partmodules

                    if (pm is ModuleRCS) //find partmodule RCS on the part
                    {
                        var moduleRCS = (ModuleRCS)pm;
                        moduleRCS.rcsEnabled = true;
                    }
                    else if (pm is ModuleRCSFX) //find partmodule RCS on the part
                    {
                        var moduleRCSFX = (ModuleRCSFX)pm;
                        moduleRCSFX.rcsEnabled = true;
                    }
                }
            }
        }
            
        public void Start()
        {
            if (HighLogic.LoadedScene != GameScenes.FLIGHT)
                return;

            Fields["rcsStageThrustPercentage"].uiControlFlight.onFieldChanged += UI_Event_SetStageRCSThrust;
            Fields["rcsGlobalThrustPercentage"].uiControlFlight.onFieldChanged += UI_Event_SetGlobalRCSThrust;

            StartCoroutine("SlowUpdate");
        }

        IEnumerator SlowUpdate()
        {
            while (true)
            {
                bool anyRCSActive = false;
                bool thisRCSActive = false;
                bool activeStageRCSActive = false;


                // First check to see if this part has RCS enabled
                for (int i = 0; i < part.Modules.Count; i++)
                {
                    PartModule pm = part.Modules[i]; //change from part to partmodules

                    if (pm is ModuleRCS) //find partmodule RCS on th epart
                    {
                        var moduleRCS = (ModuleRCS)pm;
                        //Events["DeactivateAllRCSs"].active = moduleRCS.rcsEnabled;
                        if (moduleRCS.rcsEnabled)
                        {
                            thisRCSActive = true;
                            break;
                        }
                    }
                    else if (pm is ModuleRCSFX) //find partmodule RCS on the part
                    {
                        var moduleRCSFX = (ModuleRCSFX)pm;
                        //Events["DeactivateAllRCSs"].active = moduleRCSFX.rcsEnabled;
                        if (moduleRCSFX.rcsEnabled)
                        {
                            thisRCSActive = true;
                            break;
                        }
                    }
                }

                var curStage = FlightGlobals.ActiveVessel.currentStage - 1;

                // Check the active stage
                GetAllPartsInEachStage();
                foreach (Part part in stageParts[curStage].Values)
                {
                    if (part.inverseStage >= curStage)
                    {
                        for (int i = 0; i < part.Modules.Count; i++)
                        {
                            PartModule pm = part.Modules[i]; //change from part to partmodules

                            if (pm.moduleName == "ModuleRCS") //find partmodule RCS on th epart
                            {
                                var moduleRCS = (ModuleRCS)pm;
                                if (moduleRCS.rcsEnabled)
                                {
                                    activeStageRCSActive = true;
                                    break;
                                }
                            }
                            else if (pm.moduleName == "ModuleRCSFX") //find partmodule RCS on th epart
                            {
                                var moduleRCSFX = (ModuleRCSFX)pm;
                                if (moduleRCSFX.rcsEnabled == true)
                                {
                                    activeStageRCSActive = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                // Now check the rest of the vessel

                //if (part.inverseStage != curStage)
                {
                    for (int j = 0; j < vessel.Parts.Count; j++)
                    {
                        Part part = vessel.Parts[j];
                    
                        for (int i = 0; i < part.Modules.Count; i++)
                        {
                            PartModule pm = part.Modules[i]; //change from part to partmodules

                            if (pm is ModuleRCS) //find partmodule RCS on the part
                            {
                                var moduleRCS = (ModuleRCS)pm;
                                if (moduleRCS.rcsEnabled)
                                {
                                    anyRCSActive = true;
                                    break;
                                }
                            }
                            else if (pm is ModuleRCSFX) //find partmodule RCS on th epart
                            {
                                var moduleRCSFX = (ModuleRCSFX)pm;
                                if (moduleRCSFX.rcsEnabled)
                                {
                                    anyRCSActive = true;
                                    break;
                                }
                            }
                        }
                        if (anyRCSActive)
                            break;
                    }
                }
                Events["ActivateAllRCSsInActiveStage"].active = !activeStageRCSActive;
                Events["ActivateAllRCSsInCurrentStage"].active = !thisRCSActive;
                Events["ActivateAllRCSs"].active = !anyRCSActive;

                Events["DeactivateAllRCSs"].active = anyRCSActive;

                yield return (object)new WaitForSeconds(1f);

            }
        }
    }
}