using System.Runtime.Serialization.Formatters.Binary;

namespace Exam.Services
{
    public static class BinarySerializer
    {
        public static void Serialize(string filename, object data)
        {
            var serializer = new BinaryFormatter();
            using var stream = new FileStream(filename, FileMode.Create);
            serializer.Serialize(stream, data);
        }

        public static object Deserialize(string filename)
        {
            var deserializer = new BinaryFormatter();
            using var stream = new FileStream(filename, FileMode.Open);
            return deserializer.Deserialize(stream);
        }
    }
}
