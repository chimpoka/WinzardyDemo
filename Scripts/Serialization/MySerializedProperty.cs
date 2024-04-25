using UnityEngine;

namespace WinzardyDemo.Save
{
    public class MySerializedProperty<T>
    {
        private readonly string FileName;
        private readonly string KeyName;

        public MySerializedProperty(string FileName, string KeyName)
        {
            this.FileName = FileName;
            this.KeyName = KeyName;
        }

        public void Save(T Object)
        {
            if (Object == null)
            {
                Debug.LogError("Trying to save null object");
                Delete();
                return;
            }

            var writer = QuickSaveWriter.Create(FileName);
            writer.Write(KeyName, Object).Commit();
        }

        public T Load(T DefaultValue = default(T))
        {
            var reader = QuickSaveReader.TryCreate(FileName);
            T result = DefaultValue;
            reader.TryRead(KeyName, ref result);
            
            return result;
        }

        public void Delete()
        {
            var writer = QuickSaveWriter.Create(FileName);

            writer.Delete(KeyName);
            writer.Commit();
        }
    }
}