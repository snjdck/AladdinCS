using System;
using System.Collections.Generic;

namespace Aladdin.CSV
{
	static public class CsvParser
	{
		static public List<T> Parse<T>(string content)
		{
			var result = new List<T>();
			string[] lineList = content.Split('\n');
			int keyCount = lineList[0].Split(',').Length;
			for(int i=3, n=lineList.Length; i<n; ++i)
			{
				string line = lineList[i].Trim();
				if(line.Length <= 0){
					continue;
				}
				var itemList = ParseLine(line, keyCount);
				if (string.IsNullOrEmpty(itemList[0])){
					continue;
				}
				T record = (T)Activator.CreateInstance(typeof(T), itemList);
				result.Add(record);
			}
			return result;
		}

		static List<string> ParseLine(string line, int itemCount)
		{
			var result = new List<string>();
			int charIndex = 0;
			for (int i = 0; i < itemCount; ++i){
				result.Add(ReadItem(line, ref charIndex));
			}
			return result;
		}

		static string ReadItem(string content, ref int charIndex)
		{
			if(charIndex >= content.Length){
				return null;
			}
			char firstChar = content[charIndex];
			if (firstChar == ','){
				++charIndex;
				return null;
			}
			string result;
			if (firstChar == '"')
			{
				int nextQuoteIndex = ++charIndex;
				bool hasQuoteInStr = false;
				for (;;)
				{
					nextQuoteIndex = content.IndexOf('"', nextQuoteIndex);
					if (nextQuoteIndex < content.Length - 1 && content[nextQuoteIndex + 1] == '"'){
						nextQuoteIndex += 2;
						hasQuoteInStr = true;
					}else{
						break;
					}
				}
				result = content.Substring(charIndex, nextQuoteIndex - charIndex);
				if (hasQuoteInStr){
					result = result.Replace("\"\"", "\"");
				}
				charIndex = nextQuoteIndex + 2;
			}
			else
			{
				int nextCommaIndex = content.IndexOf(',', charIndex);
				if (nextCommaIndex < 0){
					nextCommaIndex = content.Length;
				}
				result = content.Substring(charIndex, nextCommaIndex - charIndex);
				charIndex = nextCommaIndex + 1;
			}
			return result;
		}
	}
}
