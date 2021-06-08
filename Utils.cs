using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace PrivateAbstraction
{
    class Utils
    {
        public static FieldDef[] getEnumValues(TypeDef type)
        {
            List<FieldDef> fields = new List<FieldDef>();
            for (int x = 0; x < type.Fields.Count; x++)
            {
                FieldDef field = type.Fields[x];
                if (field.FieldType.AssemblyQualifiedName == type.AssemblyQualifiedName)
                {
                    fields.Add(field);
                }
            }
            return fields.ToArray();
        }

        public static string getEventName(object fullOperand)
        {
            return ((MemberRef)fullOperand).Name.Replace("add_", null);
            //return fullOperand.Split(':')[2].Split('(')[0].Replace("add_", null);
        }

        public static bool isObfuscatedName(string typeName)
        {
            foreach (char ch in typeName.ToCharArray())
            {
                if (!char.IsDigit(ch)) { return false; }
            }
            return true;
        }
    }
}
