using UnityEngine;

namespace Aladdin.Mrgs
{
    static public class MgrBase
    {
        static private readonly GameObject go;

        static MgrBase()
        {
            go = new GameObject("__MgrBase__");
            Object.DontDestroyOnLoad(go);
        }

        static public T AddComponent<T>() where T : Component
        {
            return go.AddComponent<T>();
        }
    }
}
