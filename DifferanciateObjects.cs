using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace PrivateAbstraction
{
    class DifferanciateObjects
    {
        public static TypeDef recogniseEnum(TypeDef type)
        {
            FieldDef[] fields = Utils.getEnumValues(type);
            //if (!type.Name.StartsWith("_g") && !type.Name.StartsWith("Copyright_Tangible_Software_Solutions")) { return type; }
            foreach (FieldDef field in fields)
            {
                switch (field.Name)
                {
                    case "Round":
                    case "Square":
                    case "Curly":
                    case "Angle":
                        type.Name = "BracketType";
                        return type;
                    case "OriginalCode":
                    case "ConvertedCode":
                        type.Name = "CodeType";
                        return type;
                    case "SelectBlock":
                        type.Name = "VisualBasic_BlockType";
                        return type;
                    case "SwitchBlock":
                        type.Name = "CSharp_BlockType";
                        return type;
                    case "Note":
                    case "Warning":
                    case "ToDo":
                        type.Name = "CommentType";
                        return type;
                    case "EndAddHandler":
                        type.Name = "VisualBasic_StatementType";
                        return type;
                    case "VBCode":
                    case "CPlusCode":
                    case "JavaCode":
                    case "CSharpCode":
                        type.Name = "CodeLanguage";
                        return type;
                    case "Fields":
                    case "Methods":
                    case "Properties":
                        type.Name = "StartingSegementType";
                        return type;
                    case "CrLfToBackslashRBackslashN":
                    case "CrLfToNewLine":
                        type.Name = "NewLineType";
                        return type;
                    case "NoBase":
                    case "WindowsFormBase":
                    case "ConsoleBase":
                    case "SomeOtherBase":
                        type.Name = "ApplicationBaseType";
                        return type;
                    case "AbstractMethodHeader":
                    case "NamespaceStatement":
                        type.Name = "HeaderDeclarationType";
                        return type;
                    case "DelegatesNotAvailable_VBToJava":
                        type.Name = "ActionForLanguage";
                        return type;
                    case "PrivateField":
                    case "NonPrivateField":
                        type.Name = "FieldType";
                        return type;
                    case "ClassType":
                    case "DotNetStructType":
                    case "InterfaceType":
                    case "EnumType":
                        type.Name = "CodeSegmentType";
                        return type;
                    case "Int":
                    case "Long":
                    case "Short":
                    case "Byte":
                    case "SByte":
                    case "Double":
                    case "Float":
                    case "Decimal":
                    case "Char":
                    case "String":
                    case "Boolean":
                    case "UnsignedInt":
                    case "UnsignedLong":
                    case "UnsignedShort":
                    case "IntPtr":
                    case "UIntPtr":
                    case "Date":
                    case "Object":
                        type.Name = "KnownVarTypes";
                        return type;
                    case "ClassLevel":
                    case "Global":
                        type.Name = "VisibilityLevel";
                        return type;
                    case "MemberSameNameAsEnclosingType":
                        type.Name = "UnknownEnum_0";
                        return type;
                    case "TypesIndexersStaticsAndAssemblyBaseClassMembers":
                        type.Name = "UnknownEnum_1";
                        return type;
                    case "UserCode":
                    case "CustomAssemblyCode":
                    case "SystemAssemblyCode":
                        type.Name = "CodeStyle";
                        return type;
                    case "ListType":
	                case "DictionaryType":
	                case "ArrayType":
                        type.Name = "StackType"; 
                        return type;
                    case "SecondIsOutsideOfFirst":
                        type.Name = "UnknownEnum_2";
                        return type;
                    case "NonGenericTypeWithIndexer":
                        type.Name = "UnknownEnum_3";
                        return type;
                    case "MultiplicativeOrHigher":
                        type.Name = "ComparasionType";
                        return type;
                    case "FileLocation":
                        type.Name = "InputType";
                        return type;
                    case "DotNetFramework":
                        type.Name = "DotNetRuntimeType";
                        return type;
                }
            }
            return type;
        }

        public static TypeDef renameFromString(TypeDef type)
        {
            foreach (MethodDef methods in type.Methods)
            {
                bool methodsRenamed = false;
                foreach (Instruction inst in methods.Body.Instructions)
                {
                    if (inst.OpCode.Equals(OpCodes.Ldstr))
                    {
                        switch (inst.Operand.ToString())
                        {
                            case "Your one-year subscription license expired on ":
                                methods.Name = "TimeRemaining_YearLengthSubscription";
                                methodsRenamed = true;
                                break;
                            case "You have already received an extension.":
                                methods.Name = "TimeRemaining_AddExtension";
                                methodsRenamed = true;
                                break;
                            case "A":
                                if (methods.Body.Instructions[methods.Body.Instructions.IndexOf(inst)+5].OpCode.Equals(OpCodes.Ldstr))
                                {
                                    if (methods.Body.Instructions[methods.Body.Instructions.IndexOf(inst)+5].Operand.ToString() == "B")
                                    {
                                        methods.Name = "KeyCheck_NumberToLetter";
                                        methodsRenamed = true;
                                        break;
                                    }
                                }
                                break;
                            case "/instant-csharp-help.html":
                                methods.Name = "OpenHelpPage";
                                methodsRenamed = true;
                                break;
                            case "Fifteenth":
                                methods.Name = "GetNumFullname";
                                methodsRenamed = true;
                                break;
                            case "copy_non_code_files_in_folder_conversion":
                                if (methods.Body.Instructions[methods.Body.Instructions.IndexOf(inst) + 1].IsLdcI4())
                                {
                                    ((MethodDef)methods.Body.Instructions[methods.Body.Instructions.IndexOf(inst) + 2].Operand).Name = "GetSetting";
                                }
                                else if (methods.Body.Instructions[methods.Body.Instructions.IndexOf(inst) + 1].IsLdarg())
                                {
                                    ((MethodDef)methods.Body.Instructions[methods.Body.Instructions.IndexOf(inst) + 2].Operand).Name = "SetSetting";
                                }
                                break;
                        }
                    }
                    if (methodsRenamed)
                    {
                        break;
                    }
                }
            }
            return type;
        }

        public static bool isJunkMethod(MethodDef methods)
        {
            if (!methods.HasBody) { return true; }
            switch (methods.Body.Instructions.Count)
            {
                case 1:
                    return true;
                case 2:
                    switch (methods.Body.Instructions[0].OpCode.Code)
                    {
                        case Code.Ldstr:
                            if (methods.Body.Instructions[0].Operand == null || methods.Body.Instructions[0].Operand.ToString() == "") { return true; }
                            break;
                        case Code.Ldnull:
                            return true;
                    }
                    break;
                case 3:
                    if (methods.Body.Instructions[0].OpCode.Equals(OpCodes.Ldc_I4_0) && methods.Body.Instructions[1].OpCode.Equals(OpCodes.Newarr) && methods.Body.Instructions[2].OpCode.Equals(OpCodes.Ret))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        public static TypeDef fixFields(TypeDef type)
        {
            List<int> typeRepeatCount = new List<int>();
            List<string> fieldTypeNames = new List<string>();
            foreach (FieldDef field in type.Fields)
            {
                if (fieldTypeNames.Contains(field.FieldType.TypeName)) { typeRepeatCount[fieldTypeNames.IndexOf(field.FieldType.TypeName)]++; }
                else { fieldTypeNames.Add(field.FieldType.TypeName); typeRepeatCount.Add(0); }
                field.Name = field.FieldType.TypeName.Split('`')[0] + "_" + typeRepeatCount[fieldTypeNames.IndexOf(field.FieldType.TypeName)];
            }
            return type;
        }

        public static TypeDef sortForm(TypeDef type)
        {
            if (Utils.isObfuscatedName(type.Name))
            {
                type.Name = "Form_" + type.Name;
            }
            foreach (MethodDef methods in type.Methods)
            {
                if (!methods.HasBody) { continue; }
                foreach (Instruction inst in methods.Body.Instructions)
                {
                    int instIndex = methods.Body.Instructions.IndexOf(inst);
                    switch (inst.OpCode.Code)
                    {
                        case Code.Ldarg_0:
                            if (methods.Body.Instructions[instIndex + 1].OpCode.Equals(OpCodes.Call))
                            {
                                if (methods.Body.Instructions[instIndex + 1].Operand.ToString().Contains("System.Windows.Forms.Control::SuspendLayout"))
                                {
                                    methods.Name = "InitializeComponent";
                                }
                            }
                            break;
                        case Code.Ldftn:
                            if (methods.Body.Instructions[instIndex + 1].OpCode.Equals(OpCodes.Newobj))
                            {
                                if (((MemberRef)methods.Body.Instructions[instIndex + 1].Operand).Class.Name.Contains("EventHandler"))
                                {
                                    string eventVarName = "";
                                    if (methods.Body.Instructions[instIndex - 2].Operand == null) { continue; }
                                    switch (methods.Body.Instructions[instIndex - 2].Operand.GetType().Name)
                                    {
                                        case "FieldDefMD":
                                            eventVarName = ((FieldDef)methods.Body.Instructions[instIndex - 2].Operand).Name;
                                            break;
                                        case "Local":
                                            eventVarName = ((Local)methods.Body.Instructions[instIndex - 2].Operand).Type.TypeName;
                                            break;
                                        default:
                                            if (((MemberRef)methods.Body.Instructions[instIndex + 2].Operand).DeclaringType.Name != "Application") { continue; }
                                            eventVarName = "Application";
                                            break;
                                    }
                                    if (eventVarName == "") { throw new Exception("Cannot find target var name"); }
                                    ((MethodDef)inst.Operand).Name = eventVarName + "_" + Utils.getEventName(methods.Body.Instructions[instIndex + 2].Operand);
                                }
                            }
                            break;
                    }
                }
            }
            return type;
        }

        public static bool isForm(TypeDef type)
        {
            bool isFormType = false;
            foreach (MethodDef methods in type.Methods)
            {
                if (!methods.HasBody) { continue; }
                foreach (Instruction inst in methods.Body.Instructions)
                {
                    int instIndex = methods.Body.Instructions.IndexOf(inst);
                    if (inst.OpCode.Equals(OpCodes.Ldarg_0) && methods.Body.Instructions[instIndex + 1].OpCode.Equals(OpCodes.Call))
                    {
                        if (methods.Body.Instructions[instIndex + 1].Operand.ToString().Contains("System.Windows.Forms.Control::SuspendLayout"))
                        {
                            isFormType = true;
                        }
                    }
                }
            }
            return isFormType;
        }
    }
}
