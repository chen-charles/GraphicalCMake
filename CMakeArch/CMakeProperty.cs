using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace CMakeArch
{
    public interface CMakeElement : ISerializable
    {
        String Name { get; set; }
        HashSet<FileInfo> sources { get; }
    }

    [Serializable]
    public class CMakeProperty : ISerializable
    {
        public String name { get; }
        public String value { get; set; }
        public CMakeProperty(String name) { this.name = name; }
        public CMakeProperty(String name, String value) : this(name) { this.value = value; }
        public CMakeProperty(CMakeProperty other) { this.name = other.name; this.value = other.value; }
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            sb.Append("=");
            sb.Append(value);
            sb.Append("\n");
            return sb.ToString();
        }


        public CMakeProperty(SerializationInfo info, StreamingContext context) : this(info.GetValue("name", typeof(string)) as string, info.GetValue("value", typeof(string)) as string) { }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name, typeof(string));
            info.AddValue("value", value, typeof(string));
        }
    }

    [Serializable]
    public class CMakePropertyCollection : ICollection<CMakeProperty>, ISerializable
    {
        public static HashSet<String> property_lst;
        public static HashSet<String> property_fixed;
        private Dictionary<string, CMakeProperty> properties;

        public int Count { get { return properties.Count; } }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(CMakeProperty property)
        {
            if (property_lst.Contains(property.name) || isFixedPropertyName(property.name))
            {
                properties.Add(property.name, property);
            }
            else
            {
                throw new Exception("The given property is unrecognized.  ");
            }
        }

        public bool Remove(CMakeProperty property)
        {
            try
            {
                removeProperty(property.name);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Clear()
        {
            properties.Clear();
        }

        public bool Contains(CMakeProperty item)
        {
            return properties.Values.Contains(item);
        }

        public void CopyTo(CMakeProperty[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<CMakeProperty> GetEnumerator()
        {
            return properties.Values.GetEnumerator();
        }

        public CMakeProperty popProperty(String name)
        {
            CMakeProperty prop;
            bool isSucc = properties.TryGetValue(name, out prop);
            if (isSucc)
            {
                prop = new CMakeProperty(prop);
                properties.Remove(name);
                return prop;
            }
            else
            {
                throw new Exception("The given property name does not exist.  ");
            }
        }

        public void removeProperty(String name) { popProperty(name); }

        public bool isFixedPropertyName(String name)
        {
            foreach (String s in property_fixed)
            {
                s.Remove(s.IndexOf('<'), s.IndexOf('>') - s.IndexOf('<'));
                if (name.Contains(s)) return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)properties).GetObjectData(info, context);
        }
        public CMakePropertyCollection() { properties = new Dictionary<string, CMakeProperty>(); }
        protected CMakePropertyCollection(SerializationInfo info, StreamingContext context)
        {
            properties = (Dictionary<string, CMakeProperty>)info.GetValue("properties", typeof(Dictionary<string, CMakeProperty>));
            properties.OnDeserialization(null);
        }
    }
}

