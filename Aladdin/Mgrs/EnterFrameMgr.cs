using System;
using UnityEngine;

namespace Aladdin.Mrgs
{
    static public class EnterFrameMgr
    {
        private class EnterFrameMgrComp : MonoBehaviour
        {
            void Update()
            {
                EnterFrameMgr.Update();
            }
        }

        static EnterFrameMgr()
        {
            MgrBase.AddComponent<EnterFrameMgrComp>();
        }

        static private event Action OnUpdate;

        static public void Add(Action callback)
        {
            OnUpdate += callback;
        }

        static public void Remove(Action callback)
        {
            OnUpdate -= callback;
        }

        static private void Update()
        {
            if(OnUpdate != null){
                OnUpdate();
            }
        }
    }
}
