using System;
using System.Collections;
using UnityEngine;

namespace Aladdin.Utils
{
	static public class TimeUtil
	{
		static public IEnumerator Call(float lastTime, float beginTime, float intervalTime, Action callback)
		{
			int totalCount = (int)((lastTime - beginTime) / intervalTime) + 1;
			int currentCount = 0;
			float timeElapsed = 0f;

			while (currentCount < totalCount){
				if (currentCount > 0){
					if (timeElapsed >= intervalTime){
						callback();
						timeElapsed -= intervalTime;
						++currentCount;
					}
				}else if (timeElapsed >= beginTime){
					callback();
					timeElapsed -= beginTime;
					++currentCount;
				}
				float prev = Time.time;
				yield return null;
				timeElapsed += Time.time - prev;
			}
		}
	}
}
