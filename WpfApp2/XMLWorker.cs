using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace WpfApp2
{
    internal class XML_Worker
    {

        public static void Save_to_xml_file(Type T, object obj, string filename)
        {
            var xw = new XmlTextWriter(filename, Encoding.UTF8);
            xw.Formatting = Formatting.Indented;
            var writer = XmlDictionaryWriter.CreateDictionaryWriter(xw);
            var ser = new DataContractSerializer(T);
            ser.WriteObject(writer, obj);
            writer.Close();
            xw.Close();
        }

        public static string Save_to_xml_string(Type T, object obj)
        {
            var ser = new DataContractSerializer(T);
            var output = new StringWriter();
            var writer = new XmlTextWriter(output);
            ser.WriteObject(writer, obj);
            return output.GetStringBuilder().ToString();
        }

        public static object Read_from_xml(Type T, string filename)
        {
            var obj = Activator.CreateInstance(T);
            var path = filename;
            var xr = new XmlTextReader(path);
            var reader = XmlDictionaryReader.CreateDictionaryReader(xr);
            var ser = new DataContractSerializer(T);
            obj = ser.ReadObject(reader);
            reader.Close();
            xr.Close();
            return obj;
        }

        public static object Read_from_string(Type T, string str)
        {
            var obj = Activator.CreateInstance(T);
            var ser = new DataContractSerializer(T);
            var input = new StringReader(str);
            var reader = new XmlTextReader(input);
            obj = ser.ReadObject(reader);
            reader.Close();
            input.Close();
            return obj;
        }
    }
}