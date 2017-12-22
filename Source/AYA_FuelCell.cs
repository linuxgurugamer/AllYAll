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
    // ############# FUEL CELLS ############### //

    public class AYA_FuelCell : PartModule
    {
        double turnOnTime = 0;

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Start All")]
        public void DoAllFuelCells()                                                                //This runs every time you click "extend all" or "retract all"
        {
            bool b = false;
            double ACTIVATION_TIME = 0;            

            var ayaFuelCell = this.part.FindModuleImplementing<AYA_FuelCell>();      // Use the state of the part being clicked on to determine what to do
            if (ayaFuelCell != null)
            {
                var mrc = this.part.FindModuleImplementing<ModuleResourceConverter>();
                if (mrc != null)
                    b = mrc.IsActivated;
            }
            else
                return;             // should never happen
            
            foreach (Part part in vessel.Parts)                                             //Cycle through each part on the vessel
            {
                if (part != null)                       
                {
                    ayaFuelCell = part.FindModuleImplementing<AYA_FuelCell>();  
                    if (ayaFuelCell != null)
                    {
                        var mrc = part.FindModuleImplementing<ModuleResourceConverter>();
                        if (mrc != null)
                        {
                            ACTIVATION_TIME++;                           
                            if (mrc.IsActivated)
                                mrc.StopResourceConverter();
                            else
                                mrc.StartResourceConverter();
                            mrc = null;
                        }
                    }

                }
            }
            turnOnTime = Planetarium.GetUniversalTime() + ACTIVATION_TIME;
        }



        public void FixedUpdate()                                                               //This runs every second and makes sure the menus are correct.
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR)
                return;

          
                var ayaFuelCell = this.part.FindModuleImplementing<AYA_FuelCell>();      //This is so the below code knows the part it's dealing with is a fuel cell.
                if (ayaFuelCell != null)                          
                {
                    var thisPart = this.part.FindModuleImplementing<ModuleResourceConverter>();
                    if (thisPart != null)
                    {
                        if (Planetarium.GetUniversalTime() > turnOnTime)
                        {
                            if (thisPart.IsActivated)
                            {
                                Events["DoAllFuelCells"].guiName = Localizer.Format("#AYA_ANTENNA_UI_FUEL_CELL_STOP_ALL");
                                Events["DoAllFuelCells"].active = true;
                            }
                            else
                            {
                                Events["DoAllFuelCells"].guiName = Localizer.Format("#AYA_ANTENNA_UI_FUEL_CELL_START_ALL");
                                Events["DoAllFuelCells"].active = true;
                            }
                        }
                        else
                        {
                            Events["DoAllFuelCells"].active = false;
                        }
                    }
                } 
            
        }
    }
}
