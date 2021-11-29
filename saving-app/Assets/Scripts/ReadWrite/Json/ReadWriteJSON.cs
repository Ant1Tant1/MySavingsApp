using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Scripts.ReadWrite.Objects;

namespace Scripts.ReadWrite.JSON
{
    [System.Serializable]
    public class ReadWriteJSON : IReadWriteData
    {
        private string folderPath = null;
        private string filePath = null;

        public string GetPath(SerializationTypes serializationType)
        {
            SetFilePath(serializationType);
            return filePath; 
        }

        private void SetFilePath(SerializationTypes serializationType)
        {
            string[] pathNames = SetFileName(serializationType);
            folderPath = Path.Combine(Application.streamingAssetsPath, pathNames[1]);

            // check if directory exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            filePath = Path.Combine(folderPath, pathNames[0] + ".json");
        }

        /// <returns name="fileName" name="fileFolder"></returns>
        private string[] SetFileName(SerializationTypes serializationType)
        {
            switch (serializationType)
            {
                case SerializationTypes.Users:
                    return new string[2] { "users", "data" };
                    
                case SerializationTypes.CashFlows:
                    return new string[2] { Engine.UserId.Replace(" ", "") + "_cashflows", "CashFlows" };

                case SerializationTypes.Savings:
                    return new string[2] { Engine.UserId.Replace(" ", "") + "_savings", "Savings"};

                case SerializationTypes.CashFlowSubCategories:
                    return new string[2] { Engine.UserId.Replace(" ", "") + "_cashflows_subcategories", "SubCategories" };

                default:
                    throw new System.ArgumentNullException("fileName has not been set");
            }
        }

        public List<T> ReadList<T>(SerializationTypes serializationType) where T : new() => ReadListFromJson<T>(serializationType);
        private List<T> ReadListFromJson<T>(SerializationTypes serializationType) where T : new()
        {
            // set filePath
            SetFilePath(serializationType);

            // handles exceptions
            if (filePath == null)
            {
                throw new System.ArgumentNullException("filePath has not been set");
            }
            else if (!File.Exists(filePath))
            {
                File.Create(filePath);
                return new List<T>();
            }

            // Read file
            string jsonString = File.ReadAllText(filePath);
            List<T> jsonList = JsonConvert.DeserializeObject<List<T>>(jsonString);

            // return object
            Debug.Log("File has been read from " + filePath);
            return jsonList;
        }

        public T Read<T>(SerializationTypes serializationType) where T : new() => ReadFromJson<T>(serializationType);
        private T ReadFromJson<T>(SerializationTypes serializationType) where T: new()
        {
            // set filePath
            SetFilePath(serializationType);

            // handles exceptions
            if (filePath == null)
            {
                throw new System.ArgumentNullException("filePath has not been set");
            }
            else if (!File.Exists(filePath))
            {
                File.Create(filePath);
                return new T();
            }

            // Read file
            string jsonString = File.ReadAllText(filePath);
            T json = JsonConvert.DeserializeObject<T>(jsonString);

            // return object
            Debug.Log("File has been read from " + filePath);
            return json;
        }

        public bool Save<T>(List<T> obj, SerializationTypes serializationType) => SaveToJson(obj, serializationType);
        private bool SaveToJson<T>(List<T> obj, SerializationTypes serializationType)
        {
            // set filePath
            SetFilePath(serializationType);

            // handles exceptions
            if (filePath == null)
            {
                throw new System.ArgumentNullException("filePath has not been set");
            }

            try
            {
                // write file
                File.WriteAllText(filePath, JsonConvert.SerializeObject(obj, Formatting.Indented));

                Debug.Log("File has been written to " + filePath);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.Log("Failed to write json Files:  " + e);
                return false;
            }
        }

        public bool Save<T>(T obj, SerializationTypes serializationType) => SaveToJson(obj, serializationType);
        private bool SaveToJson<T>(T obj, SerializationTypes serializationType)
        {
            // set filePath
            SetFilePath(serializationType);

            // handles exceptions
            if (filePath == null)
            {
                throw new System.ArgumentNullException("filePath has not been set");
            }

            try
            {
                // write file
                File.WriteAllText(filePath, JsonConvert.SerializeObject(obj, Formatting.Indented));

                Debug.Log("File has been written to " + filePath);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.Log("Failed to write json Files:  " + e);
                return false;
            }
        }
    }
}


