using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization;

namespace CMakeArch
{
    [Serializable]
    public class CMakeDirectoryPropertyCollection : CMakePropertyCollection
    {
        public static new List<string> property_lst
           = new List<string>()
           {
                "ADDITIONAL_MAKE_CLEAN_FILES",
                "CACHE_VARIABLES",
                "CLEAN_NO_CUSTOM",
                "CMAKE_CONFIGURE_DEPENDS",
                "COMPILE_DEFINITIONS",
                "COMPILE_OPTIONS",
                "DEFINITIONS",
                "EXCLUDE_FROM_ALL",
                "IMPLICIT_DEPENDS_INCLUDE_TRANSFORM",
                "INCLUDE_DIRECTORIES",
                "INCLUDE_REGULAR_EXPRESSION",
                "INTERPROCEDURAL_OPTIMIZATION",
                "LINK_DIRECTORIES",
                "LISTFILE_STACK",
                "MACROS",
                "PARENT_DIRECTORY",
                "RULE_LAUNCH_COMPILE",
                "RULE_LAUNCH_CUSTOM",
                "RULE_LAUNCH_LINK",
                "TEST_INCLUDE_FILE",
                "VARIABLES",
           };

        public static new List<string> property_fixed
            = new List<string>()
            {
                "INTERPROCEDURAL_OPTIMIZATION_<CONFIG>",
                "VS_GLOBAL_SECTION_POST_<section>",
                "VS_GLOBAL_SECTION_PRE_<section>",
            };
    }

    [Serializable]
    public class CMakeDirectory : CMakeElement
    {
        public String Name { get; set; }
        public CMakeDirectoryPropertyCollection properties { get; private set; }
        public HashSet<FileInfo> sources { get; private set; }

        public HashSet<CMakeTarget> targets { get; private set; }
        public DirectoryInfo directory { get; private set; }

        public HashSet<CMakeDirectory> subdirectories { get; private set; }

        public CMakeDirectory(DirectoryInfo thisDirectory)
        {
            Name = thisDirectory.Name;
            directory = thisDirectory;
            targets = new HashSet<CMakeTarget>();
            properties = new CMakeDirectoryPropertyCollection();


            sources = new HashSet<FileInfo>();
            subdirectories = new HashSet<CMakeDirectory>();
            bSubdirectoriesEnumerated = false;
            bFilesEnumerated = false;
        }

        #region Enmueration
        public bool bFilesEnumerated { get; private set; }
        public HashSet<FileInfo> EnumerateFiles()
        {
            if (bFilesEnumerated) return sources;
            foreach (FileInfo finfo in from f in directory.EnumerateFiles()
                                       where (f.Attributes & FileAttributes.Hidden) == 0
                                       select f)
                sources.Add(finfo);
            return sources;
        }

        public bool bSubdirectoriesEnumerated { get; private set; }
        public HashSet<CMakeDirectory> EnumerateSubdirectories()
        {
            if (bSubdirectoriesEnumerated) return subdirectories;

            foreach (DirectoryInfo dinfo in from d in directory.EnumerateDirectories()
                                            where (d.Attributes & FileAttributes.Hidden) == 0
                                            select d)
            {
                CMakeDirectory cd = new CMakeDirectory(dinfo);
                foreach (FileInfo finfo in from f in directory.EnumerateFiles()
                                           where (f.Attributes & FileAttributes.Hidden) == 0
                                           select f)
                {
                    cd.sources.Add(finfo);
                }
                subdirectories.Add(cd);
            }
            bSubdirectoriesEnumerated = true;
            return subdirectories;
        }
        #endregion

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            sb.Append("\tPath=");
            sb.Append(directory.ToString());
            sb.Append("\nCMakeTarget Infos:\n");
            foreach (CMakeTarget ct in targets)
            {
                sb.Append(ct.ToString());
            }
            return sb.ToString();
        }

        protected CMakeDirectory(SerializationInfo info, StreamingContext context)
        {
            Name = (String)info.GetValue("Name", typeof(String));
            properties = (CMakeDirectoryPropertyCollection)info.GetValue("properties", typeof(CMakeDirectoryPropertyCollection));
            sources = (HashSet<FileInfo>)info.GetValue("sources", typeof(HashSet<FileInfo>));
            targets = (HashSet<CMakeTarget>)info.GetValue("targets", typeof(HashSet<CMakeTarget>));
            directory = (DirectoryInfo)info.GetValue("directory", typeof(DirectoryInfo));
            subdirectories = (HashSet<CMakeDirectory>)info.GetValue("subdirectories", typeof(HashSet<CMakeDirectory>));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(String));
            info.AddValue("properties", properties, typeof(CMakeDirectoryPropertyCollection));
            info.AddValue("sources", sources, typeof(HashSet<FileInfo>));
            info.AddValue("targets", targets, typeof(HashSet<CMakeTarget>));
            info.AddValue("directory", directory, typeof(DirectoryInfo));
            info.AddValue("subdirectories", subdirectories, typeof(HashSet<CMakeDirectory>));
        }

    }
}
