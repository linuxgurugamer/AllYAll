using System;
using System.Reflection;

// Following copied from  the [X] Science! mod
// The license for this is slightly different than the All Y'All mod, license follows:

// [x] Science! (and this file) is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International license.
// 
// You are free to:
//
// Share - copy and redistribute the material in any medium or format
//
// Adapt - remix, transform, and build upon the material
//
// As long as:
//
// Attribution - You must give appropriate credit, provide a link to the license, and indicate if changes were made.
//               You may do so in any reasonable manner, but not in any way that suggests the licensor endorses you or your use.
//
// NonCommercial - You may not use the material for commercial purposes.
//
// ShareAlike - If you remix, transform, or build upon the material, you must distribute your contributions under the same license as the original.

namespace AllYAll
{
    /// <summary>
    /// Class to access the DMagic API via reflection so we don't have to recompile when the DMagic mod updates. If the DMagic API changes, we will need to modify this code.
    /// </summary>


    /// <summary>
    /// Class to access the DMagic API via reflection so we don't have to recompile when the DMagic mod updates. If the DMagic API changes, we will need to modify this code.
    /// </summary>

    public class DMagicFactory
    {
        private static bool _dmagicIsInstalled = false;
        private static bool _dmagicSciAnimateGenIsInstalled = false;

        public static bool DMagic_IsInstalled { get { return _dmagicIsInstalled; } }
        public static bool DMagicScienceAnimateGeneric_IsInstalled { get { return _dmagicSciAnimateGenIsInstalled; } }


        static internal DMagicStuff DMStuff { get; private set; }
        static internal DMagic_SciAnimGenFactory DMM_SciAnimGenericStuff { get; private set; }


        static public void InitDMagicFactory()
        {
            _dmagicIsInstalled = false;

            if (HasMod("DMagic"))
            {
                _dmagicIsInstalled = true;
                doit_DMStuff();
            }
            if (HasMod("DMModuleScienceAnimateGeneric"))
            {
                _dmagicSciAnimateGenIsInstalled = true;
                doit_DMSciAnimGenStuff();
            }

        }

        static void doit_DMStuff()
        {
            if (DMStuff == null)
                DMStuff = new DMagicStuff();
        }
        private static bool HasMod(string modIdent)
        {
            foreach (AssemblyLoader.LoadedAssembly a in AssemblyLoader.loadedAssemblies)
            {
                if (modIdent == a.name)
                    return true;
            }
            return false;
        }

        internal static bool RunExperiment(string sid, ModuleScienceExperiment exp, bool runSingleUse = true)
        {
            return DMagicStuff.fetch.RunExperiment(sid, exp, runSingleUse);
        }
        static void doit_DMSciAnimGenStuff()
        {
            DMM_SciAnimGenericStuff = new DMagic_SciAnimGenFactory();
            if (DMM_SciAnimGenericStuff != null)
            {
                string ver = GetAssemblyInfo.GetVersionStringFromAssembly("DMModuleScienceAnimateGeneric");
                if (String.Compare(ver, "0.23") < 0)
                {
                    Log.Error("Old version of DMModuleScienceAnimateGeneric installed, disabling any references to that");
                    DMM_SciAnimGenericStuff = null;
                    _dmagicSciAnimateGenIsInstalled = false;
                }
                else
                    Log.Info("DMModuleScienceAnimateGeneric version: " + GetAssemblyInfo.GetVersionStringFromAssembly("DMModuleScienceAnimateGeneric"));
            }

        }


        internal static bool RunSciAnimGenExperiment(string sid, ModuleScienceExperiment exp, bool runSingleUse = true)
        {
            return DMagic_SciAnimGenFactory.fetch.RunExperiment(sid, exp, runSingleUse);
        }

        internal static Type getType(string name)
        {
            Type type = null;
            AssemblyLoader.loadedAssemblies.TypeOperation(t =>
            {
                if (t.FullName != null && t.FullName == name)
                {
                    type = t;
                }
            });
            return type;
        }


    }
}