using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace CMakeArch
{
    [Serializable]
    public class CMakeTargetPropertyCollection : CMakePropertyCollection
    {
        public static new List<string> property_lst
           = new List<string>()
           {
                "ALIASED_TARGET",
                "ANDROID_API",
                "ANDROID_API_MIN",
                "ANDROID_GUI",
                "ARCHIVE_OUTPUT_DIRECTORY",
                "ARCHIVE_OUTPUT_NAME",
                "AUTOGEN_TARGET_DEPENDS",
                "AUTOMOC_MOC_OPTIONS",
                "AUTOMOC",
                "AUTOUIC",
                "AUTOUIC_OPTIONS",
                "AUTORCC",
                "AUTORCC_OPTIONS",
                "BUILD_WITH_INSTALL_RPATH",
                "BUNDLE_EXTENSION",
                "BUNDLE",
                "C_EXTENSIONS",
                "C_STANDARD",
                "C_STANDARD_REQUIRED",
                "COMPATIBLE_INTERFACE_BOOL",
                "COMPATIBLE_INTERFACE_NUMBER_MAX",
                "COMPATIBLE_INTERFACE_NUMBER_MIN",
                "COMPATIBLE_INTERFACE_STRING",
                "COMPILE_DEFINITIONS",
                "COMPILE_FEATURES",
                "COMPILE_FLAGS",
                "COMPILE_OPTIONS",
                "COMPILE_PDB_NAME",
                "COMPILE_PDB_OUTPUT_DIRECTORY",
                "CROSSCOMPILING_EMULATOR",
                "CXX_EXTENSIONS",
                "CXX_STANDARD",
                "CXX_STANDARD_REQUIRED",
                "DEBUG_POSTFIX",
                "DEFINE_SYMBOL",
                "EchoString",
                "ENABLE_EXPORTS",
                "EXCLUDE_FROM_ALL",
                "EXCLUDE_FROM_DEFAULT_BUILD",
                "EXPORT_NAME",
                "FOLDER",
                "Fortran_FORMAT",
                "Fortran_MODULE_DIRECTORY",
                "FRAMEWORK",
                "GENERATOR_FILE_NAME",
                "GNUtoMS",
                "HAS_CXX",
                "IMPLICIT_DEPENDS_INCLUDE_TRANSFORM",
                "IMPORTED_CONFIGURATIONS",
                "IMPORTED_IMPLIB",
                "IMPORTED_LINK_DEPENDENT_LIBRARIES",
                "IMPORTED_LINK_INTERFACE_LANGUAGES",
                "IMPORTED_LINK_INTERFACE_LIBRARIES",
                "IMPORTED_LINK_INTERFACE_MULTIPLICITY",
                "IMPORTED_LOCATION",
                "IMPORTED_NO_SONAME",
                "IMPORTED",
                "IMPORTED_SONAME",
                "IMPORT_PREFIX",
                "IMPORT_SUFFIX",
                "INCLUDE_DIRECTORIES",
                "INSTALL_NAME_DIR",
                "INSTALL_RPATH",
                "INSTALL_RPATH_USE_LINK_PATH",
                "INTERFACE_AUTOUIC_OPTIONS",
                "INTERFACE_COMPILE_DEFINITIONS",
                "INTERFACE_COMPILE_FEATURES",
                "INTERFACE_COMPILE_OPTIONS",
                "INTERFACE_INCLUDE_DIRECTORIES",
                "INTERFACE_LINK_LIBRARIES",
                "INTERFACE_POSITION_INDEPENDENT_CODE",
                "INTERFACE_SOURCES",
                "INTERFACE_SYSTEM_INCLUDE_DIRECTORIES",
                "INTERPROCEDURAL_OPTIMIZATION",
                "JOB_POOL_COMPILE",
                "JOB_POOL_LINK",
                "LABELS",
                "LIBRARY_OUTPUT_DIRECTORY",
                "LIBRARY_OUTPUT_NAME",
                "LINK_DEPENDS_NO_SHARED",
                "LINK_DEPENDS",
                "LINKER_LANGUAGE",
                "LINK_FLAGS",
                "LINK_INTERFACE_LIBRARIES",
                "LINK_INTERFACE_MULTIPLICITY",
                "LINK_LIBRARIES",
                "LINK_SEARCH_END_STATIC",
                "LINK_SEARCH_START_STATIC",
                "LOCATION",
                "MACOSX_BUNDLE_INFO_PLIST",
                "MACOSX_BUNDLE",
                "MACOSX_FRAMEWORK_INFO_PLIST",
                "MACOSX_RPATH",
                "NAME",
                "NO_SONAME",
                "NO_SYSTEM_FROM_IMPORTED",
                "OSX_ARCHITECTURES",
                "OUTPUT_NAME",
                "PDB_NAME",
                "PDB_OUTPUT_DIRECTORY",
                "POSITION_INDEPENDENT_CODE",
                "PREFIX",
                "PRIVATE_HEADER",
                "PROJECT_LABEL",
                "PUBLIC_HEADER",
                "RESOURCE",
                "RULE_LAUNCH_COMPILE",
                "RULE_LAUNCH_CUSTOM",
                "RULE_LAUNCH_LINK",
                "RUNTIME_OUTPUT_DIRECTORY",
                "RUNTIME_OUTPUT_NAME",
                "SKIP_BUILD_RPATH",
                "SOURCES",
                "SOVERSION",
                "STATIC_LIBRARY_FLAGS",
                "SUFFIX",
                "TYPE",
                "VERSION",
                "VISIBILITY_INLINES_HIDDEN",
                "VS_DOTNET_REFERENCES",
                "VS_DOTNET_TARGET_FRAMEWORK_VERSION",
                "VS_GLOBAL_KEYWORD",
                "VS_GLOBAL_PROJECT_TYPES",
                "VS_GLOBAL_ROOTNAMESPACE",
                "VS_KEYWORD",
                "VS_SCC_AUXPATH",
                "VS_SCC_LOCALPATH",
                "VS_SCC_PROJECTNAME",
                "VS_SCC_PROVIDER",
                "VS_WINRT_COMPONENT",
                "VS_WINRT_EXTENSIONS",
                "VS_WINRT_REFERENCES",
                "WIN32_EXECUTABLE",
                "XCTEST",

           };

        public static new List<string> property_fixed
            = new List<string>()
            {
                "ARCHIVE_OUTPUT_DIRECTORY_<CONFIG>",
                "ARCHIVE_OUTPUT_NAME_<CONFIG>",
                "COMPILE_PDB_NAME_<CONFIG>",
                "COMPILE_PDB_OUTPUT_DIRECTORY_<CONFIG>",
                "<CONFIG>_OUTPUT_NAME",
                "<CONFIG>_POSTFIX",
                "EXCLUDE_FROM_DEFAULT_BUILD_<CONFIG>",
                "IMPORTED_IMPLIB_<CONFIG>",
                "IMPORTED_LINK_DEPENDENT_LIBRARIES_<CONFIG>",
                "IMPORTED_LINK_INTERFACE_LANGUAGES_<CONFIG>",
                "IMPORTED_LINK_INTERFACE_LIBRARIES_<CONFIG>",
                "IMPORTED_LINK_INTERFACE_MULTIPLICITY_<CONFIG>",
                "IMPORTED_LOCATION_<CONFIG>",
                "IMPORTED_NO_SONAME_<CONFIG>",
                "IMPORTED_SONAME_<CONFIG>",
                "INTERPROCEDURAL_OPTIMIZATION_<CONFIG>",
                "<LANG>_INCLUDE_WHAT_YOU_USE",
                "<LANG>_VISIBILITY_PRESET",
                "LIBRARY_OUTPUT_DIRECTORY_<CONFIG>",
                "LIBRARY_OUTPUT_NAME_<CONFIG>",
                "LINK_FLAGS_<CONFIG>",
                "LINK_INTERFACE_LIBRARIES_<CONFIG>",
                "LINK_INTERFACE_MULTIPLICITY_<CONFIG>",
                "LOCATION_<CONFIG>",
                "MAP_IMPORTED_CONFIG_<CONFIG>",
                "OSX_ARCHITECTURES_<CONFIG>",
                "OUTPUT_NAME_<CONFIG>",
                "PDB_NAME_<CONFIG>",
                "PDB_OUTPUT_DIRECTORY_<CONFIG>",
                "RUNTIME_OUTPUT_DIRECTORY_<CONFIG>",
                "RUNTIME_OUTPUT_NAME_<CONFIG>",
                "STATIC_LIBRARY_FLAGS_<CONFIG>",
                "VS_GLOBAL_<variable>",
                "XCODE_ATTRIBUTE_<an-attribute>",
            };
    }

    [Serializable]
    public class CMakeTarget : CMakeElement
    {
        public String Name { get; set; }
        public CMakeTargetPropertyCollection properties { get; }
        public HashSet<FileInfo> sources { get; }

        public enum TargetType
        {
            EXECUTABLE,
            EXECUTABLE_ALIAS,

            LIBRARY_SHARED,
            LIBRARY_STATIC,
            LIBRARY_MODULE,

            LIBRARY_OBJECT,
            LIBRARY_ALIAS,
            LIBRARY_INTERFACE
        }

        public TargetType Type { get; }
        public bool IMPORTED { get; set; }

        public CMakeTarget(TargetType type, bool isImported)
        {
            Type = type;
            IMPORTED = isImported;
            sources = new HashSet<FileInfo>();
            properties = new CMakeTargetPropertyCollection();
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            sb.Append("\tTargetType=");
            sb.Append(Type);
            sb.Append("\tIMPORTED=");
            sb.Append(IMPORTED);
            if (properties.Count != 0)
            {
                sb.Append("\nDefined Properties: \n");
                foreach (CMakeProperty property in properties)
                {
                    sb.Append(property);
                }
            }
            return sb.ToString();
        }


        protected CMakeTarget(SerializationInfo info, StreamingContext context)
        {
            Type = (TargetType)info.GetValue("Type", typeof(TargetType));
            IMPORTED = (bool)info.GetValue("IMPORTED", typeof(bool));
            Name = (string)info.GetValue("Name", typeof(string));
            sources = (HashSet<FileInfo>)info.GetValue("sources", typeof(HashSet<FileInfo>));
            properties = (CMakeTargetPropertyCollection)info.GetValue("properties", typeof(CMakeTargetPropertyCollection));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Type", Type, typeof(TargetType));
            info.AddValue("IMPORTED", IMPORTED, typeof(bool));
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("sources", sources, typeof(HashSet<FileInfo>));
            info.AddValue("properties", properties, typeof(CMakeTargetPropertyCollection));
        }

    }
}
