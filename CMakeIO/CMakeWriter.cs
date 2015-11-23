using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
namespace CMakeIO
{

    public class CMakeWriter : IOBase
    {
        public CMakeWriter(CMakeArch.CMakeProject proj)
        {
            StreamWriter file = new StreamWriter(proj.rootDirectory.directory.FullName + "/CMakeLists.txt");
            file.WriteLine("cmake_minimum_required(VERSION " + CMakeVer.Item1 + "." + CMakeVer.Item2 + "." + CMakeVer.Item3 + ")");

            file.WriteLine("project(" + proj.Name + ")");

            new CMakeWriter(proj.rootDirectory, file);
        }

        public CMakeWriter(CMakeArch.CMakeDirectory dir, StreamWriter file = null)
        {
            if (file == null)
                file = new StreamWriter(dir.directory.FullName + "/CMakeLists.txt");
            foreach (CMakeArch.CMakeTarget target in dir.targets)
            {
                StringBuilder sb = new StringBuilder();
                switch (target.Type)
                {
                    case CMakeArch.CMakeTarget.TargetType.EXECUTABLE:
                        sb.Append("add_executable(");
                        sb.Append(target.Name);
                        sb.Append(" ");
                        foreach (FileInfo f in target.sources)
                            sb.Append(f.FullName);
                        sb.Append(")");
                        break;
                    case CMakeArch.CMakeTarget.TargetType.LIBRARY_STATIC:
                        sb.Append("add_library(");
                        sb.Append(target.Name);
                        sb.Append(" STATIC ");
                        foreach (FileInfo f in target.sources)
                            sb.Append(f.FullName);
                        sb.Append(")");
                        break;
                    case CMakeArch.CMakeTarget.TargetType.LIBRARY_SHARED:
                        sb.Append("add_library(");
                        sb.Append(target.Name);
                        sb.Append(" SHARED ");
                        foreach (FileInfo f in target.sources)
                            sb.Append(f.FullName);
                        sb.Append(")");
                        break;

                }
                file.WriteLine(sb.ToString());
                sb.Clear();
                sb.Append("set_target_properties(" + target.Name + " PROPERTIES");
                foreach (CMakeArch.CMakeProperty prop in target.properties)
                {
                    sb.Append(prop.name + " " + prop.value + " ");
                }
                sb.Append(")");
                file.WriteLine(sb.ToString());

            }

            foreach (CMakeArch.CMakeDirectory subd in dir.subdirectories)
            {
                file.WriteLine("add_subdirectory(" + subd.Name + ")");
                new CMakeWriter(subd);
            }

            file.Close();
        }
    }
}
