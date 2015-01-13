using System;
using System.Collections;
using UnityEngine;

namespace Aladdin.Mrgs
{
    static public class LoadMgr
    {
        static public void Load(string path, Action<WWW> onLoad, Action<string> onError=null)
        {
            CoroutineMgr.StartCoroutine(LoadImpl(path, onLoad, onError));
        }

        static private IEnumerator LoadImpl(string path, Action<WWW> onLoad, Action<string> onError)
        {
            var www = new WWW(GetFullPath(path));
            yield return www;
            var errorText = www.error;
            if(string.IsNullOrEmpty(errorText)){
                onLoad(www);
            }else{
                Debug.LogError(errorText);
                if(onError != null){
                    onError(errorText);
                }
            }
        }

        static private string GetFullPath(string path)
        {
            if(path.Contains("://")){
                return path;
            }
            var fullPath = path;
            if(!IsFullNativePath(fullPath)){
                fullPath = Application.streamingAssetsPath + "/" + fullPath;
            }
            if(Application.platform != RuntimePlatform.Android){
                fullPath = "file:///" + fullPath;
            }
            return fullPath;
        }

        static private bool IsFullNativePath(string path)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return path.Contains(":/");
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.IPhonePlayer:
                    return path.StartsWith("/");
            }
            return true;
        }
    }
}
