using System.Collections;
using UnityEngine;

namespace AllYAll
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class AYA_PAW_Refresh : MonoBehaviour
    {
        public static AYA_PAW_Refresh Instance;
        public enum AYA_Module { cargo, radiator, antenna, rt_antenna, drill, fuelcell, activeradiator, sas, solar, genericAnimation };
        public void Start()
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }

        internal void RefreshPAWMenu(Part p, AYA_Module m, string e, string animName = "")
        {
            switch (m)
            {
                case AYA_Module.cargo:
                    StartCoroutine(CargoDelayedUpdatePAWMenu(p, e));
                    break;
                case AYA_Module.radiator:
                    StartCoroutine(RadiatorDelayedUpdatePAWMenu(p, e));
                    break;
                case AYA_Module.antenna:
                    StartCoroutine(AntennaDelayedUpdatePAWMenu(p, e));
                    break;
                case AYA_Module.rt_antenna:
                    StartCoroutine(AntennaRTDelayedUpdatePAWMenu(p, e));
                    break;
                case AYA_Module.fuelcell:
                    StartCoroutine(FuelCellDelayedUpdatePAWMenu(p, e));
                    break;
                case AYA_Module.activeradiator:
                    StartCoroutine(ActiveRadiatorDelayedUpdatePAWMenu(p, e));
                    break;

                case AYA_Module.solar:
                    StartCoroutine(SolarDelayedUpdatePAWMenu(p, e));
                    break;
                case AYA_Module.sas:
                    StartCoroutine(SaSDelayedUpdatePAWMenu(p, e));
                    break;

                case AYA_Module.drill:
                    StartCoroutine(DrillDelayedUpdatePAWMenu(p, e));
                    break;

                case AYA_Module.genericAnimation:
                    StartCoroutine(GenericAnimationUpdatePAWMenu(p, e, animName));
                    break;
            }
        }

        public IEnumerator CargoDelayedUpdatePAWMenu(Part p, string e)
        {
            var thisPartCargoBay = p.FindModuleImplementing<AYA_CargoBay>();

            while (thisPartCargoBay.EventStatus(e) == false)
            {
                yield return new WaitForSeconds(0.25f);
                if (HighLogic.LoadedSceneIsEditor || p.vessel == FlightGlobals.ActiveVessel)
                {
                    //var thisPartCargoBay = p.FindModuleImplementing<AYA_CargoBay>();
                    thisPartCargoBay.UpdatePAWMenu();
                }
                else
                    break;
            }
        }

        public IEnumerator RadiatorDelayedUpdatePAWMenu(Part p, string e)
        {
            var thisPartCargoBay = p.FindModuleImplementing<AYA_Radiator>();

            while (thisPartCargoBay.EventStatus(e) == false)
            {
                yield return new WaitForSeconds(0.25f);
                if (p.vessel == FlightGlobals.ActiveVessel)
                {
                    //var thisPartCargoBay = p.FindModuleImplementing<AYA_CargoBay>();
                    thisPartCargoBay.UpdatePAWMenu();
                }
                else
                    break;
            }
        }

        public IEnumerator AntennaDelayedUpdatePAWMenu(Part p, string e)
        {
            var thisPartAntenna = p.FindModuleImplementing<AYA_Antenna>();

            while (thisPartAntenna.EventStatus(e) == false)
            {
                yield return new WaitForSeconds(0.25f);
                if (p.vessel == FlightGlobals.ActiveVessel)
                {
                    //var thisPartCargoBay = p.FindModuleImplementing<AYA_CargoBay>();
                    thisPartAntenna.UpdatePAWMenu();
                }
                else
                    break;
            }
        }
        public IEnumerator AntennaRTDelayedUpdatePAWMenu(Part p, string e)
        {
            var thisPartAntenna = p.FindModuleImplementing<AYA_AntennaRT>();

            while (thisPartAntenna.EventStatus(e) == false)
            {
                yield return new WaitForSeconds(0.25f);
                if (p.vessel == FlightGlobals.ActiveVessel)
                {
                    //var thisPartCargoBay = p.FindModuleImplementing<AYA_CargoBay>();
                    thisPartAntenna.UpdatePAWMenu();
                }
                else
                    break;
            }
        }

        public IEnumerator FuelCellDelayedUpdatePAWMenu(Part p, string e)
        {
            var thisPartFuelCell = p.FindModuleImplementing<AYA_FuelCell>();

            while (thisPartFuelCell.EventStatus(e) == false)
            {
                yield return new WaitForSeconds(0.25f);
                if (p.vessel == FlightGlobals.ActiveVessel)
                {
                    //var thisPartCargoBay = p.FindModuleImplementing<AYA_CargoBay>();
                    thisPartFuelCell.UpdatePAWMenu();
                }
                else
                    break;
            }
        }

        public IEnumerator ActiveRadiatorDelayedUpdatePAWMenu(Part p, string e)
        {
            var thisPartActiveRadiator = p.FindModuleImplementing<AYA_ActiveRadiator>();

            while (thisPartActiveRadiator.EventStatus(e) == false)
            {
                yield return new WaitForSeconds(0.25f);
                if (p.vessel == FlightGlobals.ActiveVessel)
                {
                    //var thisPartCargoBay = p.FindModuleImplementing<AYA_CargoBay>();
                    thisPartActiveRadiator.UpdatePAWMenu();
                }
                else
                    break;
            }
        }

        public IEnumerator SolarDelayedUpdatePAWMenu(Part p, string e)
        {
            var thisPartSolar = p.FindModuleImplementing<AYA_Solar>();

            while (thisPartSolar.EventStatus(e) == false)
            {
                yield return new WaitForSeconds(0.25f);
                if (p.vessel == FlightGlobals.ActiveVessel)
                {
                    thisPartSolar.UpdatePAWMenu();
                }
                else
                    break;
            }
        }

        
        public IEnumerator SaSDelayedUpdatePAWMenu(Part p, string e)
        {
            var thisPartSaS = p.FindModuleImplementing<AYA_SAS>();

            while (thisPartSaS.EventStatus(e) == false)
            {
                yield return new WaitForSeconds(0.25f);
                if (p.vessel == FlightGlobals.ActiveVessel)
                {
                    thisPartSaS.UpdatePAWMenu();
                }
                else
                    break;
            }
        }

        public IEnumerator DrillDelayedUpdatePAWMenu(Part p, string e)
        {
            var thisPartDrill = p.FindModuleImplementing<AYA_Drill>();

            while (thisPartDrill.EventStatus(e) == false)
            {
                yield return new WaitForSeconds(0.25f);
                if (p.vessel == FlightGlobals.ActiveVessel)
                {
                    //var thisPartCargoBay = p.FindModuleImplementing<AYA_CargoBay>();
                    thisPartDrill.UpdatePAWMenu();
                }
                else
                    break;
            }
        }

        public IEnumerator GenericAnimationUpdatePAWMenu(Part p, string e, string animName)
        {
            AYA_ModuleAnimateGeneric thisAYA_MAG = null;
            var allsAYA_MAG = p.FindModulesImplementing<AYA_ModuleAnimateGeneric>();
            foreach (var m in allsAYA_MAG)
            {
                if (m.animationName == animName)
                {
                    thisAYA_MAG = m;
                    break;
                }
            }
            if (thisAYA_MAG == null)
                yield return null;
            if (thisAYA_MAG.isOneShot)
            {
                thisAYA_MAG.UpdatePAWMenu();
                yield return null;
            }
            while (thisAYA_MAG.EventStatus(e) == false)
            {
                yield return new WaitForSeconds(0.25f);
                if (HighLogic.LoadedSceneIsEditor || p.vessel == FlightGlobals.ActiveVessel)
                {
                    //var thisPartCargoBay = p.FindModuleImplementing<AYA_CargoBay>();
                    thisAYA_MAG.UpdatePAWMenu();
                }
                else
                {
                    break;
                }
            }

        }
    }

}
