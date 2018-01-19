using Newtonsoft.Json;


namespace EntityTempleteSaveLoader
{
    public class ClassWrapper
    {
        public string TypeName;
        public string Json;

        public ClassWrapper()
        {
        }

        public ClassWrapper(object obj)
        {
            TypeName = obj.GetType().ToString();
            Json = JsonConvert.SerializeObject(obj);
        }

    }
}