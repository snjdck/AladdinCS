using System.Collections.Generic;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using System;

namespace Aladdin.CSV
{
	static public class CsvGen
	{
		static public void Gen(string[] fileList)
		{
			var fileDict = Do(fileList);
			
			var compileUnit = new CodeCompileUnit();

			var name = new CodeNamespace("Aladdin");
			name.Imports.Add(new CodeNamespaceImport("System"));
			name.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));

			foreach(var fileDesc in fileDict.Values)
			{
				var dataClass = new DataClass(fileDesc.name);
				var tableClass = new TableClass(fileDesc.tableName, fileDesc.name);

				foreach(var field in fileDesc.propList) {
					if(field.isUnique) {
						tableClass.RegUniqueField(field);
					}
					dataClass.RegField(field, fileDict);
				}

				name.Types.Add(dataClass);
				name.Types.Add(tableClass);
			}

			compileUnit.Namespaces.Add(name);
			GenerateCode(compileUnit, "test.cs");
		}

		static void GenerateCode(CodeCompileUnit compileUnit, string fileName)
		{
			var provider = new CSharpCodeProvider();
			StreamWriter sw = new StreamWriter(fileName, false);
			provider.GenerateCodeFromCompileUnit(compileUnit, sw, new CodeGeneratorOptions());
			sw.Close();
		}

		static CompilerResults CompileCode(string fileName)
		{
			var provider = new CSharpCodeProvider();
			var compiler = provider.CreateCompiler();
			//编译参数
			CompilerParameters cp = new CompilerParameters(
				new string[] { "System.dll" }, fileName, false
			);
			cp.GenerateExecutable = true;//生成EXE,不是DLL

			CompilerResults cr = compiler.CompileAssemblyFromFile(cp, fileName);
			return cr;
		}

		static Dictionary<string, Test2> Do(string[] fileNameList)
		{
			var fileDict = new Dictionary<string, Test2>();
			foreach(var fileName in fileNameList) {
				string className = Path.GetFileNameWithoutExtension(fileName);
				fileDict[className] = new Test2(className, File.ReadAllText(fileName));
			}
			return fileDict;
		}
	}

	class Test1
	{
		public string name;
		public string type;
		public int index;
		public bool isUnique;
		public bool isRef;
		public string refName;

		public Test1(string name, string typeDesc, int index)
		{
			this.name = name;
			this.index = index;
			var list = typeDesc.Split(' ');
			isUnique = list.Length > 1 && list[1] == "unique";
			int dotIndex = list[0].IndexOf('.');
			isRef = dotIndex >= 0;
			if(isRef){
				type = list[0].Substring(0, dotIndex);
				refName = list[0].Substring(dotIndex+1);
			} else {
				type = list[0];
			}
		}

		public string fieldName{
			get{
				return "_" + name;
			}
		}

		public string dictName{
			get{
				return name + "Dict";
			}
		}

		public string findBy{
			get{
				return "FindBy" + name;
			}
		}
	}

	class Test2
	{
		public string name;
		public Dictionary<string, Test1> propDict;
		public List<Test1> propList;

		public Test2(string name, string content)
		{
			this.name = name;

			string[] lineList = content.Split('\n');
			string[] keyList = lineList[0].Split(',');
			string[] typeList = lineList[1].Split(',');

			this.propDict = new Dictionary<string, Test1>(keyList.Length);
			this.propList = new List<Test1>(keyList.Length);

			for(int i = 0, n = keyList.Length; i < n; ++i ) {
				string key = keyList[i].Trim();
				var prop = new Test1(key, typeList[i].Trim(), i);
				propDict[key] = prop;
				propList.Add(prop);
			}
		}

		public string tableName{
			get{
				return name + "Table";
			}
		}
	}

	class DataClass : CodeTypeDeclaration
	{
		CodeConstructor constructor;
		public DataClass(string className) : base(className)
		{
			constructor = new CodeConstructor();
			constructor.Attributes = MemberAttributes.Public;
			constructor.Parameters.Add(new CodeParameterDeclarationExpression("List<string>", "itemList"));
			Members.Add(constructor);
		}

		CodeMemberProperty CreateProperty(string propName)
		{
			var property = new CodeMemberProperty();
			property.Attributes = MemberAttributes.Public;
			property.HasGet = true;
			property.Name = propName;
			Members.Add(property);
			return property;
		}

		public void RegField(Test1 field, Dictionary<string, Test2> fileDict)
		{
			var fieldCodeRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.fieldName);

			var property = CreateProperty(field.name);
			string fieldType;
			if(field.isRef) {
				/*
				if(!fileDict.ContainsKey(field.type)){
					return;
				}*/
				var typeInfo = fileDict[field.type];
				var refInfo = typeInfo.propDict[field.refName];
				fieldType = refInfo.type;
				property.Type = new CodeTypeReference(field.type);
				property.GetStatements.Add(new CodeMethodReturnStatement(
					new CodeMethodInvokeExpression(
						new CodeTypeReferenceExpression(typeInfo.tableName),
						refInfo.findBy,
						fieldCodeRef
					)
				));
			} else {
				fieldType = field.type;
				property.Type = new CodeTypeReference(field.type);
				property.GetStatements.Add(new CodeMethodReturnStatement(fieldCodeRef));
			}

			Members.Add(new CodeMemberField(fieldType, field.fieldName));
			constructor.Statements.Add(AssignInit(fieldCodeRef, field.index, fieldType));
		}

		static CodeStatement AssignInit(CodeExpression fieldCodeRef, int fieldIndex, string fieldType)
		{
			var valueToAssign = new CodeIndexerExpression(
				new CodeVariableReferenceExpression("itemList"),
				new CodePrimitiveExpression(fieldIndex)
			);
			if(fieldType == "int") {
				return GetConditionStatement(fieldCodeRef, valueToAssign, fieldType, 0);
			}
			if(fieldType == "float") {
				return GetConditionStatement(fieldCodeRef, valueToAssign, fieldType, 0f);
			}
			if(fieldType == "bool") {
				return GetConditionStatement(fieldCodeRef, valueToAssign, fieldType, false);
			}
			return new CodeAssignStatement(fieldCodeRef, valueToAssign);
		}

		static CodeStatement GetConditionStatement(CodeExpression fieldCodeRef, CodeExpression valueToAssign, string fieldType, object defaultValue)
		{
			var result = new CodeConditionStatement(new CodeMethodInvokeExpression(
				new CodeTypeReferenceExpression("string"), "IsNullOrEmpty", valueToAssign
			));
			result.TrueStatements.Add(new CodeAssignStatement(fieldCodeRef, new CodePrimitiveExpression(defaultValue)));
			result.FalseStatements.Add(new CodeAssignStatement(fieldCodeRef, new CodeMethodInvokeExpression(
				new CodeTypeReferenceExpression(fieldType), "Parse", valueToAssign
			)));
			return result;
		}
	}

	class TableClass : CodeTypeDeclaration
	{
		CodeMemberMethod methodAdd;
		string itemClassName;
		public TableClass(string className, string itemClassName) : base(className)
		{
			this.itemClassName = itemClassName;

			methodAdd = new CodeMemberMethod2("Add", true, true);
			methodAdd.Parameters.Add(new CodeParameterDeclarationExpression(itemClassName, "item"));
			Members.Add(methodAdd);

			var methodRegister = new CodeMemberMethod2("Register", true, true);
			methodRegister.Parameters.Add(new CodeParameterDeclarationExpression(
				string.Format("Dictionary<object, {0}>", itemClassName), "itemDict")
			);
			methodRegister.Statements.Add(new CodeVariableDeclarationStatement("var", "itemList", new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("itemDict"), "Values")));
			methodRegister.Statements.Add(new CodeVariableDeclarationStatement("int", "count", new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("itemList"), "Count")));
			methodRegister.Statements.Add(new CodeIterationStatement(
				new CodeVariableDeclarationStatement("int", "i", new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.LessThan, new CodeVariableReferenceExpression("count")),
				new CodeSnippetStatement("++i"),
				new CodeExpressionStatement(new CodeMethodInvokeExpression(
					new CodeTypeReferenceExpression(className), "Add",
					new CodeIndexerExpression(
						new CodeVariableReferenceExpression("itemList"),
						new CodeVariableReferenceExpression("i"))
			))));
			Members.Add(methodRegister);
		}

		public void RegUniqueField(Test1 field)
		{
			var dictType = string.Format("Dictionary<object, {0}>", itemClassName);
			var dictField = new CodeMemberField(dictType, field.dictName);
			dictField.Attributes = MemberAttributes.Static;
			dictField.InitExpression = new CodeObjectCreateExpression(dictType);
			Members.Add(dictField);

			var dictRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(Name), field.dictName);
			var itemRef = new CodeVariableReferenceExpression("item");

			var method = new CodeMemberMethod2(field.findBy, true, true);
			method.ReturnType = new CodeTypeReference(itemClassName);
			method.Parameters.Add(new CodeParameterDeclarationExpression(field.type, field.name));
			method.Statements.Add(new CodeMethodReturnStatement(
				new CodeIndexerExpression(dictRef, new CodeVariableReferenceExpression(field.name))
			));
			Members.Add(method);

			methodAdd.Statements.Add(new CodeAssignStatement(
				new CodeIndexerExpression(dictRef, new CodePropertyReferenceExpression(itemRef, field.name)),
				itemRef
			));
		}
	}

	class CodeMemberMethod2 : CodeMemberMethod
	{
		public CodeMemberMethod2(string name, bool isPublic, bool isStatic)
		{
			Name = name;
			if(isPublic && isStatic){
				Attributes = MemberAttributes.Static | MemberAttributes.Public;
			}else if(isPublic){
				Attributes = MemberAttributes.Public;
			}else if(isStatic){
				Attributes = MemberAttributes.Static;
			}
		}
	}
}
