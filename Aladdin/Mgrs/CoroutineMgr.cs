using System.Collections;
using UnityEngine;

namespace Aladdin.Mrgs
{
    static public class CoroutineMgr
    {
        private class CoroutineMgrComp : MonoBehaviour {}

        static CoroutineMgr()
        {
            comp = MgrBase.AddComponent<CoroutineMgrComp>();
        }

        private static readonly MonoBehaviour comp;

        static public Coroutine StartCoroutine(IEnumerator routine)
        {
            return comp.StartCoroutine(routine);
        }
    }
}
