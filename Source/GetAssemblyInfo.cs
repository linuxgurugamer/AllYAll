
using System;

namespace AllYAll
{
    public static class GetAssemblyInfo
    {

        public static System.Reflection.Assembly GetAssembly(string pAssemblyName)
        {
            System.Reflection.Assembly tMyAssembly = null;

            if (string.IsNullOrEmpty(pAssemblyName)) { return tMyAssembly; }
            tMyAssembly = GetAssemblyEmbedded(pAssemblyName);
            if (tMyAssembly == null) { GetAssemblyDLL(pAssemblyName); }

            return tMyAssembly;
        }//System.Reflection.Assembly GetAssemblyEmbedded(string pAssemblyDisplayName)


        public static System.Reflection.Assembly GetAssemblyEmbedded(string pAssemblyDisplayName)
        {
            System.Reflection.Assembly tMyAssembly = null;

            if (string.IsNullOrEmpty(pAssemblyDisplayName)) { return tMyAssembly; }
            try //try #a
            {
                tMyAssembly = System.Reflection.Assembly.Load(pAssemblyDisplayName);
            }// try #a
            catch (Exception ex)
            {
                string m = ex.Message;
            }// try #a
            return tMyAssembly;
        }//System.Reflection.Assembly GetAssemblyEmbedded(string pAssemblyDisplayName)


        public static System.Reflection.Assembly GetAssemblyDLL(string pAssemblyNameDLL)
        {
            System.Reflection.Assembly tMyAssembly = null;

            if (string.IsNullOrEmpty(pAssemblyNameDLL)) { return tMyAssembly; }
            try //try #a
            {
                if (!pAssemblyNameDLL.ToLower().EndsWith(".dll")) { pAssemblyNameDLL += ".dll"; }
                tMyAssembly = System.Reflection.Assembly.LoadFrom(pAssemblyNameDLL);
            }// try #a
            catch (Exception ex)
            {
                string m = ex.Message;
            }// try #a
            return tMyAssembly;
        }//System.Reflection.Assembly GetAssemblyFile(string pAssemblyNameDLL)


        public static string GetVersionStringFromAssembly(string pAssemblyDisplayName)
        {
            string tVersion = "Unknown";
            System.Reflection.Assembly tMyAssembly = null;

            tMyAssembly = GetAssembly(pAssemblyDisplayName);
            if (tMyAssembly == null) { return tVersion; }
            tVersion = GetVersionString(tMyAssembly.GetName().Version.ToString());
            return tVersion;
        }//string GetVersionStringFromAssemblyEmbedded(string pAssemblyDisplayName)


        public static string GetVersionString(Version pVersion)
        {
            string tVersion = "Unknown";
            if (pVersion == null) { return tVersion; }
            tVersion = GetVersionString(pVersion.ToString());
            return tVersion;
        }//string GetVersionString(Version pVersion)


        public static string GetVersionString(string pVersionString)
        {
            string tVersion = "Unknown";
            string[] aVersion;

            if (string.IsNullOrEmpty(pVersionString)) { return tVersion; }
            aVersion = pVersionString.Split('.');
            if (aVersion.Length > 0) { tVersion = aVersion[0]; }
            if (aVersion.Length > 1) { tVersion += "." + aVersion[1]; }
            if (aVersion.Length > 2) { tVersion += "." + aVersion[2].PadLeft(4, '0'); }
            if (aVersion.Length > 3) { tVersion += "." + aVersion[3].PadLeft(4, '0'); }

            return tVersion;
        }//string GetVersionString(Version pVersion)

        public class VersionNumbers
        {
            public uint major = 0;
            public uint minor = 0;
            public uint patch = 0;
            public uint build = 0;
        }
        public static VersionNumbers GetVersionNumbers(string pVersionString)
        {
            string tVersion = "Unknown";
            string[] aVersion;

            VersionNumbers vn = new VersionNumbers();

            if (string.IsNullOrEmpty(pVersionString)) { return vn; }
            aVersion = pVersionString.Split('.');
            if (aVersion.Length > 0)
            {
                tVersion = aVersion[0];
                vn.major = uint.Parse(aVersion[0]);
            }
            if (aVersion.Length > 1)
            {
                tVersion += "." + aVersion[1];
                vn.minor = uint.Parse(aVersion[1]);
            }
            if (aVersion.Length > 2)
            {
                tVersion += "." + aVersion[2].PadLeft(4, '0');
                vn.patch = uint.Parse(aVersion[2]);
            }
            if (aVersion.Length > 3)
            {
                tVersion += "." + aVersion[3].PadLeft(4, '0');
                vn.build = uint.Parse(aVersion[3]);
            }

            return vn; ;
        }//string GetVersionNumbers(Version pVersion)


        public static string GetVersionStringFromAssemblyEmbedded(string pAssemblyDisplayName)
        {
            string tVersion = "Unknown";
            System.Reflection.Assembly tMyAssembly = null;

            tMyAssembly = GetAssemblyEmbedded(pAssemblyDisplayName);
            if (tMyAssembly == null) { return tVersion; }
            tVersion = GetVersionString(tMyAssembly.GetName().Version.ToString());
            return tVersion;
        }//string GetVersionStringFromAssemblyEmbedded(string pAssemblyDisplayName)


        public static string GetVersionStringFromAssemblyDLL(string pAssemblyDisplayName)
        {
            string tVersion = "Unknown";
            System.Reflection.Assembly tMyAssembly = null;

            tMyAssembly = GetAssemblyDLL(pAssemblyDisplayName);
            if (tMyAssembly == null) { return tVersion; }
            tVersion = GetVersionString(tMyAssembly.GetName().Version.ToString());
            return tVersion;
        }//string GetVersionStringFromAssemblyEmbedded(string pAssemblyDisplayName)

    }
}