using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

namespace PrivateAbstraction
{
    class Program
    {
        public static ModuleDefMD asm;

        static void renameObjects()
        {
            foreach (TypeDef type in asm.Types)
            {
                if (type.BaseType != null)
                {
                    if (type.BaseType.ToString().StartsWith("System") && type.BaseType.Name != "Object" && type.BaseType.Name != "Enum")
                    {
                        type.Name = type.Name + "_" + type.BaseType.TypeName + "-Inherits";
                    }
                }
                foreach (FieldDef fields in type.Fields)
                {
                    fields.Name = fields.Name.Replace("Copyright_Tangible_Software_Solutions", null).Replace("_g", null);
                }
                foreach (MethodDef methods in type.Methods)
                {
                    foreach (Parameter parameter in methods.Parameters)
                    {
                        parameter.Name = parameter.Name.Replace("Copyright_Tangible_Software_Solutions", null).Replace("_p", parameter.Type.TypeName + "_");
                    }
                    methods.Name = methods.Name.Replace("Copyright_Tangible_Software_Solutions", null).Replace("_g", null);
                }
                foreach (PropertyDef properties in type.Properties)
                {
                    properties.Name = properties.Name.Replace("Copyright_Tangible_Software_Solutions", null).Replace("_g", null);
                }
                type.Name = type.Name.Replace("Copyright_Tangible_Software_Solutions", null).Replace("_g", null);
            }
            for (int x = 0; x < asm.Types.Count; x++)
            {
                TypeDef type = asm.Types[x];
                if (!type.HasMethods && !type.HasFields && !type.HasProperties) { asm.Types.RemoveAt(x); x--; }
                for (int x_ = 0; x_ < type.Methods.Count; x_++)
                {
                    if (DifferanciateObjects.isJunkMethod(type.Methods[x_]))
                    {
                        type.Methods.RemoveAt(x_);
                        x_--;
                    }
                }
                for (int x_ = 0; x_ < type.Properties.Count; x_++)
                {
                    if (type.Properties[x_].GetMethod == null && type.Properties[x_].SetMethod == null)
                    {
                        type.Properties.RemoveAt(x_);
                        x_--;
                    }
                }
                if (type.IsEnum) { type = DifferanciateObjects.recogniseEnum(type); continue; }
                type = DifferanciateObjects.fixFields(type);
                if (DifferanciateObjects.isForm(type)) { type = DifferanciateObjects.sortForm(type); }
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "PrivateAbstraction";
            Console.WriteLine();
            Console.WriteLine(" PrivateAbstraction by misonothx | Tangible Software Obfuscator Cleaner");
            Console.WriteLine("  |- https://github.com/miso-xyz/PrivateAbstraction/");
            Console.WriteLine();
            try { asm = ModuleDefMD.Load(args[0]); }
            catch { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Invalid assmebly, please make sure it is a valid .NET application"); goto end; }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Cleaning '" + args[0] + "'...");
            renameObjects();
            ModuleWriterOptions moduleWriterOptions = new ModuleWriterOptions(asm);
            moduleWriterOptions.MetadataOptions.Flags |= MetadataFlags.PreserveAll;
            moduleWriterOptions.Logger = DummyLogger.NoThrowInstance;
            NativeModuleWriterOptions nativeModuleWriterOptions = new NativeModuleWriterOptions(asm, true);
            nativeModuleWriterOptions.MetadataOptions.Flags |= MetadataFlags.PreserveAll;
            nativeModuleWriterOptions.Logger = DummyLogger.NoThrowInstance;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(" Now Saving '" + Path.GetFileNameWithoutExtension(args[0]) + "-PrivateAbstraction" + Path.GetExtension(args[0]) + "'...");
            if (asm.IsILOnly) { asm.Write(Path.GetFileNameWithoutExtension(args[0]) + "-PrivateAbstraction" + Path.GetExtension(args[0]), moduleWriterOptions); } else { asm.NativeWrite(Path.GetFileNameWithoutExtension(args[0]) + "-PrivateAbstraction" + Path.GetExtension(args[0])); }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" '" + Path.GetFileNameWithoutExtension(args[0]) + "-PrivateAbstraction" + Path.GetExtension(args[0]) + "' Successfully cleaned & saved!");
        end:
            Console.ResetColor();
            Console.WriteLine();
            Console.Write(" Press any key to exit...");
            Console.ReadKey();
        }
    }
}