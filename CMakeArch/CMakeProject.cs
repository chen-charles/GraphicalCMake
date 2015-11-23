using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMakeArch
{
    public class CMakeProject : CMakeElement
    {
        public String Name { get; set; }
        public CMakeDirectory rootDirectory { get; set; }

        public HashSet<FileInfo> sources
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public CMakeProject(String Name, CMakeDirectory rootDirectory)
        {
            this.Name = Name;
            this.rootDirectory = rootDirectory;
        }

    }
}
