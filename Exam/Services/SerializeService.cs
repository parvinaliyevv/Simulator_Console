using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace Exam.Services
{
    public static class SerializeService
    {
        private static BinaryFormatter _binarySerializer = new BinaryFormatter();

        public static void BinarySerialize(string filename, object data)
        {
            using var stream = new FileStream(filename, FileMode.Create);
            _binarySerializer.Serialize(stream, data);
        }

        public static object BinaryDeserialize(string filename)
        {
            using var stream = new FileStream(filename, FileMode.Open);
            return _binarySerializer.Deserialize(stream);
        }

        public static void JsonSerialize(string filename, object data)
        {
            var jsonStr = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.AppendAllText(filename, jsonStr);
        }

        public static T JsonDeserialize<T>(string filename)
        {
            var str = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
