using System;
using System.Collections;
using System.Diagnostics;

namespace AllYAll
{
    public static class Log
    {
        public enum LEVEL
        {
            OFF = 0,
            ERROR = 1,
            WARNING = 2,
            INFO = 3,
            DETAIL = 4,
            TRACE = 5,
            PERFORMANCE = 6
        };

        public static LEVEL level = LEVEL.INFO;

        private static readonly String PREFIX = "AllYAll" + ": ";

        public static LEVEL GetLevel()
        {
            return level;
        }

        public static void SetLevel(LEVEL level)
        {
            UnityEngine.Debug.Log("log level " + level);
            Log.level = level;
        }

        public static LEVEL GetLogLevel()
        {
            return level;
        }

        private static bool IsLevel(LEVEL level)
        {
            return level == Log.level;
        }

        public static bool IsLogable(LEVEL level)
        {
            return level <= Log.level;
        }

        public static void Trace(String msg)
        {
            if (IsLogable(LEVEL.TRACE))
            {
                UnityEngine.Debug.Log(PREFIX + msg);
            }
        }

        public static void Detail(String msg)
        {
            if (IsLogable(LEVEL.DETAIL))
            {
                UnityEngine.Debug.Log(PREFIX + msg);
            }
        }

        [ConditionalAttribute("DEBUG")]
        public static void Info(String msg)
        {
            if (IsLogable(LEVEL.INFO))
            {
                UnityEngine.Debug.Log(PREFIX + msg);
            }
        }

        [ConditionalAttribute("DEBUG")]
        public static void Test(String msg)
        {
            //if (IsLogable(LEVEL.INFO))
            {
                UnityEngine.Debug.LogWarning(PREFIX + "TEST:" + msg);
            }
        }

        public static void Warning(String msg)
        {
            if (IsLogable(LEVEL.WARNING))
            {
                UnityEngine.Debug.LogWarning(PREFIX + msg);
            }
        }

        public static void Error(String msg)
        {
            if (IsLogable(LEVEL.ERROR))
            {
                UnityEngine.Debug.LogError(PREFIX + msg);
            }
        }

        public static void Exception(Exception e)
        {
            Log.Error("exception caught: " + e.GetType() + ": " + e.Message);
        }


        internal static void Write(string message, LEVEL level)
        {

            switch (level)
            {
                case LEVEL.ERROR:
                    Error(message);
                    return;
                case LEVEL.DETAIL:
                    Detail(message);
                    return;
                case LEVEL.WARNING:
                    Warning(message);
                    return;
                case LEVEL.INFO:
                    Info(message);
                    return;
                case LEVEL.PERFORMANCE:
                    UnityEngine.Debug.Log("[PERF] " + message);
                    return;
            }
            UnityEngine.Debug.Log(message);
        }


        internal static void Write(string message, LEVEL level, params object[] strParams)
        {

            Write(string.Format(message, strParams), level);

        }
        [ConditionalAttribute("DEBUG")]
        internal static void Debug(string message, params object[] strParams)
        {
            Write(message, LEVEL.INFO, strParams);
        }

        internal static void Normal(string message, params object[] strParams)
        {
            Write(message, LEVEL.INFO, strParams);
        }

        internal static void Warning(string message, params object[] strParams)
        {
            Write(message, LEVEL.WARNING, strParams);
        }

        internal static void Error(string message, params object[] strParams)
        {
            Write(message, LEVEL.ERROR, strParams);
        }

#if OLDLOG
        internal static void SaveInto(ConfigNode parentNode)
        {
            ConfigNode configNode = parentNode.AddNode(new ConfigNode("LogSettings"));
            configNode.AddValue("LogMask", (int)Level);
            string[] names = System.Enum.GetNames(typeof(LogMask));
            System.Array values = System.Enum.GetValues(typeof(LogMask));
            configNode.AddValue("// Bit index", "message type");
            for (int i = 0; i < names.Length - 1; i++)
            {
                configNode.AddValue($"// Bit {i}", values.GetValue(i));
            }
            Debug("[AllYAll].SaveInto = {0}", configNode.ToString());
        }

        internal static void LoadFrom(ConfigNode parentNode)
        {
            if (parentNode == null || !parentNode.HasNode("LogSettings"))
            {
                Warning("[AllYAll] failed, did not find LogSettings in: {0}", parentNode != null ? parentNode.ToString() : "<null ConfigNode>");
                return;
            }
            ConfigNode node = parentNode.GetNode("LogSettings");
            try
            {
                if (!node.HasValue("LogMask"))
                {
                    throw new System.Exception("[AllYAll]:No LogMask value in ConfigNode");
                }
                string value = node.GetValue("LogMask");
                int num = 0;
                if (int.TryParse(value, out num))
                {
                    if (num == 0)
                    {
                        Warning("[AllYAll]: Log disabled");
                    }
                    Level = (LogMask)num;
                    Debug("[AllYAll]:Loaded LogMask = {0} from ConfigNode", Level.ToString());
                }
                else
                {
                    Debug("[AllYAll]:  LogMask value '{0}' cannot be converted to LogMask", value);
                }
            }
            catch (System.Exception ex)
            {
                Warning("[AllYAll] failed with exception: {0}", ex);
            }
        }
        }
#endif



    }
}
