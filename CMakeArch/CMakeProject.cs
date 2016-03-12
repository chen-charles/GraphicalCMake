using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CMakeArch
{
    [Serializable]
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

        protected CMakeProject(SerializationInfo info, StreamingContext context)
        {
            Name = (String)info.GetValue("Name", typeof(String));
            rootDirectory = (CMakeDirectory)info.GetValue("rootDirectory", typeof(CMakeDirectory));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(String));
            info.AddValue("rootDirectory", rootDirectory, typeof(CMakeDirectory));
        }

    }
}
