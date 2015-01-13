using System;
using System.Collections;
using UnityEngine;

namespace Aladdin.Mrgs
{
    static public class SceneSwitcher
    {
        static public bool IsLoadingLevel()
        {
            return Application.isLoadingLevel;
        }

        static public bool IsCurrentLevel(string levelName)
        {
            return Application.loadedLevelName == levelName;
        }

        public static string GetCurrentLevel()
        {
            return Application.loadedLevelName;
        }

        static public void LoadLevel(string levelName, Action onLoad, Action<float> onProgress)
        {
            CoroutineMgr.StartCoroutine(LoadLevelImpl(levelName, onLoad, onProgress));
        }

        static private IEnumerator LoadLevelImpl(string levelName, Action onLoad, Action<float> onProgress)
        {
            var async = Application.LoadLevelAsync(levelName);

            if(onProgress != null)
            {
                async.allowSceneActivation = false;
	            float progress = -1f;
                while(!async.isDone){
					if(progress < async.progress) {
						progress = async.progress;
						onProgress(progress);
	                }
                    if (async.progress < 0.9f){
                        yield return null;
                    }else{
                        break;
                    }
                }
                async.allowSceneActivation = true;
            }
            
            yield return async;

            if(onProgress != null){
                onProgress(1f);
            }
            if(onLoad != null){
                onLoad();
            }
        }
    }
}
