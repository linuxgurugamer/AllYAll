using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.Localization;

namespace AllYAll
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class AYA_PAW_Refresh : MonoBehaviour
    {
        public static AYA_PAW_Refresh Instance;
        public enum AYA_Module { cargo, radiator, antenna, drill, fuelcell, activeradiator, sas, solar };
        public void Start()
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }

        internal void RefreshPAWMenu(Part p, AYA_Module m, string e)
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
                    DrillDelayedUpdatePAWMenu(p, e);
                    break;

            }
        }
        public IEnumerator CargoDelayedUpdatePAWMenu(Part p, string e)
        {
            var thisPartCargoBay = p.FindModuleImplementing<AYA_CargoBay>();

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
    }

}
