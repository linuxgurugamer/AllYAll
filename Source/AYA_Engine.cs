// ALL Y'ALL
// By 5thHorseman with much help from many others
// License: CC SA

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.Localization;

namespace AllYAll
{

    // ############# REACTION WHEELS ############### //

    public class AYA_Engine : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Shutdown all engines")]  //  "#AYA_ENGINE_UI_DEACTIVATE_ALL")]
        public void DeactivateAllEngines()
        {
            foreach (Part part in vessel.Parts)
            {
                foreach (PartModule pm in part.Modules) //change from part to partmodules
                {
                    if (pm.moduleName == "ModuleEngines") //find partmodule engine on th epart
                    {
                        var moduleEngine = (ModuleEngines)pm;
                        moduleEngine.Shutdown();
                    }
                    else if (pm.moduleName == "ModuleEnginesFX") //find partmodule engine on th epart
                    {
                        var moduleEngineFX = (ModuleEnginesFX)pm;
                        moduleEngineFX.Shutdown();
                    }
                }
            }
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Activate all engines in current stage")]  //  "#AYA_ENGINE_UI_DEACTIVATE_ALL")]
        public void ActivateAllEnginesInStage()
        {
            var curStage = FlightGlobals.ActiveVessel.currentStage - 1;
            foreach (Part part in vessel.Parts)
            {
                Debug.Log("Part: " + part.partInfo.title + ", inverseStage: " + part.inverseStage + ", curStage: " + curStage);
                if (part.inverseStage >= curStage)
                {
                    foreach (PartModule pm in part.Modules) //change from part to partmodules
                    {
                        if (pm.moduleName == "ModuleEngines") //find partmodule engine on th epart
                        {
                            var moduleEngine = (ModuleEngines)pm;
                            moduleEngine.Activate();
                        }
                        else if (pm.moduleName == "ModuleEnginesFX") //find partmodule engine on th epart
                        {
                            var moduleEngineFX = (ModuleEnginesFX)pm;
                            moduleEngineFX.Activate();
                        }
                    }
                }
            }
        }

        public void Start()
        {
            if (HighLogic.LoadedScene != GameScenes.FLIGHT)
                return;
            StartCoroutine("SlowUpdate");
        }

        IEnumerator SlowUpdate()
        {
            while (true)
            {
                bool anyEngineActive = false;
                
                foreach (PartModule pm in part.Modules)
                {
                    if (pm.moduleName == "ModuleEngines") //find partmodule engine on th epart
                    {
                        var moduleEngine = (ModuleEngines)pm;
                        Events["DeactivateAllEngines"].active = moduleEngine.EngineIgnited;
                        if (moduleEngine.EngineIgnited)
                        {
                            anyEngineActive = true;
                            break;
                        }
                    }
                    else if (pm.moduleName == "ModuleEnginesFX") //find partmodule engine on th epart
                    {
                        var moduleEngineFX = (ModuleEnginesFX)pm;
                        Events["DeactivateAllEngines"].active = moduleEngineFX.EngineIgnited;
                        if (moduleEngineFX.EngineIgnited)
                        {
                            anyEngineActive = true;
                            break;
                        }
                    }
                }
                var curStage = FlightGlobals.ActiveVessel.currentStage - 1;

                if (!anyEngineActive && part.inverseStage != curStage)
                {
                    foreach (Part part in vessel.Parts)
                    {

                        foreach (PartModule pm in part.Modules) //change from part to partmodules
                        {
                            if (pm.moduleName == "ModuleEngines") //find partmodule engine on th epart
                            {
                                var moduleEngine = (ModuleEngines)pm;
                                if (moduleEngine.EngineIgnited)
                                {
                                    anyEngineActive = true;
                                    break;
                                }
                            }
                            else if (pm.moduleName == "ModuleEnginesFX") //find partmodule engine on th epart
                            {
                                var moduleEngineFX = (ModuleEnginesFX)pm;
                                if (moduleEngineFX.EngineIgnited)
                                {
                                    anyEngineActive = true;
                                    break;
                                }
                            }
                        }
                        if (anyEngineActive)
                            break;
                    }
                }
                Events["ActivateAllEnginesInStage"].active = !anyEngineActive;
                yield return (object)new WaitForSeconds(1f);
            }
        }
    }
}