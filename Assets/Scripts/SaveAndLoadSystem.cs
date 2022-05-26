using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts
{
    class SaveAndLoadSystem
    {
        static string saving_path = Application.persistentDataPath + "/SavedShapeData.Brackeys";

        public static void SaveShapeData(ShapeData shapeData)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saving_path, FileMode.Create);
            formatter.Serialize(stream, shapeData);
            stream.Close();
        }

        public static ShapeData LoadShapeData()
        {
            if (File.Exists(saving_path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(saving_path, FileMode.Open);
                ShapeData LoadedData = formatter.Deserialize(stream)as ShapeData;
                stream.Close();
                return LoadedData;
            }
            else
            {
                Debug.Log("file not found make sure you saved a shape first");
                return null;
            }
        }
    }
}
