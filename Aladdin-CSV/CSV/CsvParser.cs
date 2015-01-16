using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aladdin.CSV
{
	static public class CsvParser
	{
		static public Dictionary<object, T> Parse<T>(string content) where T : new()
		{
			var result = new Dictionary<object, T>();
			string[] lineList = content.Split('\n');
			string[] keyList = lineList[0].Split(',');
			for(int i=3, n=lineList.Length; i<n; ++i)
			{
				string line = lineList[i].Trim();
				if(line.Length <= 0){
					continue;
				}
				var itemList = ParseLine(line, keyList.Length);
				string itemId = itemList[0];
				if (string.IsNullOrEmpty(itemId)){
					continue;
				}
				T record = new T();
				ParseRecord(keyList, itemList, record);
				result[int.Parse(itemId)] = record;
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

		static void ParseRecord(string[] keyList, List<string> itemList, object record)
        {
            Type type = record.GetType();
            for (int i = 0, n = keyList.Length; i < n; ++i)
            {
                string key = keyList[i];
                FieldInfo field = type.GetField(key);
                if (field != null){
					field.SetValue(record, CastType(itemList[i], field.FieldType));
                    continue;
                }
                PropertyInfo prop = type.GetProperty(key);
                if (prop != null && prop.CanWrite){
					prop.SetValue(record, CastType(itemList[i], prop.PropertyType), null);
                }
            }
        }

        static object CastType(string value, Type type)
        {
            if (type.Equals(typeof(System.Int32)))
            {
	            if(string.IsNullOrEmpty(value)){
		            return 0;
	            }
                return int.Parse(value);
            }
            if (type.Equals(typeof(System.Boolean)))
            {
				if(string.IsNullOrEmpty(value)){
		            return false;
	            }
                return Boolean.Parse(value);
            }
            if (type.Equals(typeof(System.Single)))
            {
				if(string.IsNullOrEmpty(value)){
		            return 0f;
	            }
                return float.Parse(value);
            }
            return value;
        }
	}
}
