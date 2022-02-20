using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Exam.Services
{
    public static class SerializeService
    {
        private static BinaryFormatter Serializer { get; } = new BinaryFormatter();

        public static void Serialize(string filename, object data)
        {
            using var stream = new FileStream(filename, FileMode.Create);
            Serializer.Serialize(stream, data);
        }

        public static object Deserialize(string filename)
        {
            using var stream = new FileStream(filename, FileMode.Open);
            return Serializer.Deserialize(stream);
        }
    }
}
