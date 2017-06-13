using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfApp2
{
    [Serializable]
    public class ProjectDoc
    { 
        public List<warnings> list_of_warnings;
        public List<protect_mech> list_of_protect_mech;
        public List<barrier> list_of_barrier;
        public List<weakness> list_of_weakness;
        public List<@object> list_of_objects;

        public string name { get; set; }
        public string description { get; set; }
        public int object_type { get; set; }
        public int calc_type { get; set; }
        
    }
}
