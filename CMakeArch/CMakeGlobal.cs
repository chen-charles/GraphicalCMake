using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMakeArch
{
    public class CMakeGlobalPropertyCollection : CMakePropertyCollection
    {
        public static new List<string> property_lst
           = new List<string>()
           {
                "ALLOW_DUPLICATE_CUSTOM_TARGETS",
                "AUTOGEN_TARGETS_FOLDER",
                "AUTOMOC_TARGETS_FOLDER",
                "CMAKE_C_KNOWN_FEATURES",
                "CMAKE_CXX_KNOWN_FEATURES",
                "DEBUG_CONFIGURATIONS",
                "DISABLED_FEATURES",
                "ENABLED_FEATURES",
                "ENABLED_LANGUAGES",
                "FIND_LIBRARY_USE_LIB64_PATHS",
                "FIND_LIBRARY_USE_OPENBSD_VERSIONING",
                "GLOBAL_DEPENDS_DEBUG_MODE",
                "GLOBAL_DEPENDS_NO_CYCLES",
                "IN_TRY_COMPILE",
                "PACKAGES_FOUND",
                "PACKAGES_NOT_FOUND",
                "JOB_POOLS",
                "PREDEFINED_TARGETS_FOLDER",
                "ECLIPSE_EXTRA_NATURES",
                "REPORT_UNDEFINED_PROPERTIES",
                "RULE_LAUNCH_COMPILE",
                "RULE_LAUNCH_CUSTOM",
                "RULE_LAUNCH_LINK",
                "RULE_MESSAGES",
                "TARGET_ARCHIVES_MAY_BE_SHARED_LIBS",
                "TARGET_SUPPORTS_SHARED_LIBS",
                "USE_FOLDERS",

           };

        public static new List<string> property_fixed
            = new List<string>()
            {
            };
    }

    public class CMakeGlobal
    {
        CMakeGlobalPropertyCollection properties { get; }
    }
}
