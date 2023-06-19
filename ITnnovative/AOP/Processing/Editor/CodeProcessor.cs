#define DEBUG_MODE

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using ITnnovative.AOP.Attributes;
using ITnnovative.AOP.Attributes.Event;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Attributes.Property;
using ITnnovative.AOP.Processing.Execution;
using ITnnovative.AOP.Processing.Execution.Arguments;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace ITnnovative.AOP.Processing.Editor
{
    [InitializeOnLoad]
    public static class CodeProcessor
    {
        static CodeProcessor()
        {
            AssemblyReloadEvents.beforeAssemblyReload += WeaveEditorAssemblies; 
        }
        
        /// <summary>
        /// Cache for Attribute Types
        /// </summary>
        private static Dictionary<Type, List<Type>> _typeCache = new Dictionary<Type, List<Type>>();
    
        public static void WeaveAssembly(AssemblyDefinition assembly)
        {
            // For all types in every module
            foreach (var module in assembly.Modules)
            {
                foreach (var type in module.Types)
                {
                    // For all constructors
                    for (var index = 0; index < type.Methods.Count; index++)
                    { 
                        var method = type.Methods[index];

                        if (!HasAttributeOfType<AOPGeneratedAttribute>(method))
                        {
                            // Register AOP Processor by encapsulating method
                            if (HasAttributeOfType<IMethodAspect>(method))
                            { 
                                MarkAsProcessed(module, method);
                                WrapEncapsulateMethod(assembly, module, type, method);
                            }
                        }
                    }

                    // Process events
                    for (var index = 0; index < type.Events.Count; index++)
                    {
                        var evt = type.Events[index];
                        if (!HasAttributeOfType<AOPGeneratedAttribute>(evt))
                        {
                            // Register AOP Processor by encapsulating method
                            if (HasAttributeOfType<IEventAspect>(evt))
                            {
                                MarkAsProcessed(module, evt);
                                if (HasAttributeOfType<IEventAddedListenerAspect>(evt))
                                {
                                    WrapEncapsulateMethod(assembly, module, type, evt.AddMethod, evt.Name);
                                }
                                if (HasAttributeOfType<IEventRemovedListenerAspect>(evt))
                                {
                                    WrapEncapsulateMethod(assembly, module, type, evt.RemoveMethod, evt.Name);
                                }
                            }
                        }

                        
                    }

                    for (var index = 0; index < type.Events.Count; index++)
                    {
                        var evt = type.Events[index];
                        if (!HasAttributeOfType<AOPGeneratedAttribute>(evt))
                        {
                            // Register AOP Processor by encapsulating method
                            if (HasAttributeOfType<IEventAspect>(evt))
                            {
                                MarkAsProcessed(module, evt);
                                if (HasAttributeOfType<IEventAddedListenerAspect>(evt))
                                {
                                    WrapEncapsulateMethod(assembly, module, type, evt.AddMethod,evt.Name, 
                                        nameof(AOPProcessor.OnEventAddListenerEnter), nameof(AOPProcessor.OnEventAddListenerExit));    
                                }
                                
                                if (HasAttributeOfType<IEventRemovedListenerAspect>(evt))
                                {
                                    WrapEncapsulateMethod(assembly, module, type, evt.RemoveMethod, evt.Name, 
                                        nameof(AOPProcessor.OnEventRemoveListenerEnter), nameof(AOPProcessor.OnEventRemoveListenerExit));
                                }
                            }
                        }

                        
                    }
                    
                    // Process properties
                    for (var index = 0; index < type.Properties.Count; index++)
                    {
                        var property = type.Properties[index];
                        if (!HasAttributeOfType<AOPGeneratedAttribute>(property))
                        {  
                            // Register AOP Processor by encapsulating method
                            if (HasAttributeOfType<IPropertyAspect>(property))
                            { 
                                MarkAsProcessed(module, property);
                                if (HasAttributeOfType<IPropertyGetAspect>(property))
                                {
                                    WrapEncapsulateMethod(assembly, module, type, property.GetMethod, property.Name,
                                        nameof(AOPProcessor.OnPropertyGetEnter), nameof(AOPProcessor.OnPropertyGetExit));
                                }
                                if (HasAttributeOfType<IPropertySetAspect>(property))
                                {
                                    WrapEncapsulateMethod(assembly, module, type, property.SetMethod, property.Name,
                                        nameof(AOPProcessor.OnPropertySetEnter), nameof(AOPProcessor.OnPropertySetExit));
                                }
                                
                            } 
                        }
                    }
                }
            }
        }

        public static List<Instruction> CreateAOPInjectableInstructions(AssemblyDefinition assembly, ModuleDefinition module,
            TypeDefinition type, MethodDefinition method, string overrideName = null, string overrideMethod =
            nameof(AOPProcessor.OnMethodStart))
        {
            var adr = GetOrImportType(module, typeof(AspectData));
            
            // New body for current method (capsule)
            var newMethodBody = new List<Instruction>();
            newMethodBody.Add(Instruction.Create(method.IsStatic ? OpCodes.Ldnull : OpCodes.Ldarg_0)); // ldnull / ldarg_0
            newMethodBody.Add(Instruction.Create(OpCodes.Ldloc_0));
            newMethodBody.Add(Instruction.Create(OpCodes.Ldtoken, type));
            newMethodBody.Add(Instruction.Create(OpCodes.Call, typeof(Type).GetMonoMethod(module, nameof(Type.GetTypeFromHandle))));

            var mName = method.Name;
            if (!string.IsNullOrEmpty(overrideName))
                mName = overrideName;
            
            newMethodBody.Add(Instruction.Create(OpCodes.Ldstr, mName));
            
            // Copy method parameters
            newMethodBody.Add(Instruction.Create(OpCodes.Ldc_I4, method.Parameters.Count));
            newMethodBody.Add(Instruction.Create(OpCodes.Newarr, module.ImportReference(typeof(object))));

            for (var num = 0; num < method.Parameters.Count; num++)
            {
                var param = method.Parameters[num];
                var pType = param.ParameterType;

                newMethodBody.Add(Instruction.Create(OpCodes.Dup));
                newMethodBody.Add(Instruction.Create(OpCodes.Ldc_I4, num));
                newMethodBody.Add(Instruction.Create(OpCodes.Ldarg, param));
                
                if (pType.IsByReference)
                    // Dereference
                {
                    var pElementTypeName = pType.GetElementType().FullName;
                    switch (pElementTypeName)
                    {
                        case "System.Int8":
                            newMethodBody.Add(Instruction.Create(OpCodes.Ldind_I1));
                            break;
                        case "System.Int16":
                            newMethodBody.Add(Instruction.Create(OpCodes.Ldind_I2));
                            break;
                        case "System.Int32":
                            newMethodBody.Add(Instruction.Create(OpCodes.Ldind_I4));
                            break;
                        case "System.Int64": 
                            newMethodBody.Add(Instruction.Create(OpCodes.Ldind_I8));
                            break;
                        case "System.Single":
                            newMethodBody.Add(Instruction.Create(OpCodes.Ldind_R4));
                            break;
                        case "System.Double":
                            newMethodBody.Add(Instruction.Create(OpCodes.Ldind_R8));
                            break;
                        default:
                            newMethodBody.Add(Instruction.Create(OpCodes.Ldind_Ref));
                            break;
                    }
                    
                }
                
                if(param.ParameterType.IsValueType || param.ParameterType.IsGenericParameter)
                    newMethodBody.Add(Instruction.Create(OpCodes.Box, pType));
                newMethodBody.Add(Instruction.Create(OpCodes.Stelem_Ref));
            }
            
            // Call method
            if(module.HasType(typeof(AOPProcessor))){
                newMethodBody.Add(Instruction.Create(OpCodes.Call,
                    module.GetType(typeof(AOPProcessor))
                    .GetMethod(overrideMethod)));
            }
            else
            {
                newMethodBody.Add(Instruction.Create(OpCodes.Call, typeof(AOPProcessor).GetMonoMethod(module, 
                    overrideMethod)));
            }
            
            // Recover parameters from aspect return data 
            var argField = GetMethod(adr, nameof(AspectData.GetArgument));
            for (var num = 0; num < method.Parameters.Count; num++)
            { 
                var param = method.Parameters[num];
                var pType = param.ParameterType;

                if (pType.IsByReference)
                    newMethodBody.Add(Instruction.Create(OpCodes.Ldarg, param));

                newMethodBody.Add(Instruction.Create(OpCodes.Ldloc_0));
                newMethodBody.Add(Instruction.Create(OpCodes.Ldc_I4, num));
                newMethodBody.Add(Instruction.Create(OpCodes.Callvirt, argField));

                if(pType.IsValueType)
                    newMethodBody.Add(Instruction.Create(OpCodes.Unbox_Any, pType));
                else if(!pType.IsByReference)
                    newMethodBody.Add(Instruction.Create(OpCodes.Castclass, pType));
                
                if (pType.IsByReference)
                    // Dereference
                {
                    var pElementTypeName = pType.GetElementType().FullName;
                    switch (pElementTypeName)
                    {
                        case "System.Int8":
                            newMethodBody.Add(Instruction.Create(OpCodes.Stind_I1));
                            break;
                        case "System.Int16":
                            newMethodBody.Add(Instruction.Create(OpCodes.Stind_I2));
                            break;
                        case "System.Int32":
                            newMethodBody.Add(Instruction.Create(OpCodes.Stind_I4));
                            break;
                        case "System.Int64": 
                            newMethodBody.Add(Instruction.Create(OpCodes.Stind_I8));
                            break;
                        case "System.Single":
                            newMethodBody.Add(Instruction.Create(OpCodes.Stind_R4));
                            break;
                        case "System.Double":
                            newMethodBody.Add(Instruction.Create(OpCodes.Stind_R8));
                            break;
                        default:
                            newMethodBody.Add(Instruction.Create(OpCodes.Stind_Ref));
                            break;
                    }
                    
                }
                else
                {
                    newMethodBody.Add(Instruction.Create(OpCodes.Starg_S, param));
                }
            }
            
            // Process return value and error value
            var hasReturned = GetProperty(adr, nameof(AspectData.HasReturned)).GetMethod;
            var hasErrored = GetProperty(adr, nameof(AspectData.HasErrored)).GetMethod;

            
            var returnedValue = GetMethod(adr, nameof(AspectData.GetReturnValue));
            var thrownException = GetMethod(adr, nameof(AspectData.GetException));

            var gamma = Instruction.Create(OpCodes.Ldloc_0);
            var delta = Instruction.Create(OpCodes.Nop);
            
            newMethodBody.Add(Instruction.Create(OpCodes.Ldloc_0));        
            newMethodBody.Add(Instruction.Create(OpCodes.Callvirt, hasReturned));
            newMethodBody.Add(Instruction.Create(OpCodes.Brfalse, gamma)); 

            // Sanity check if method returns a value, otherwise puts regular ret command
            if (method.ReturnType != module.TypeSystem.Void)
            {
                newMethodBody.Add(Instruction.Create(OpCodes.Ldloc_0));
                newMethodBody.Add(Instruction.Create(OpCodes.Callvirt, returnedValue));
                newMethodBody.Add(method.ReturnType.IsValueType
                    ? Instruction.Create(OpCodes.Unbox_Any, method.ReturnType)
                    : Instruction.Create(OpCodes.Castclass, method.ReturnType));
            }
            
            newMethodBody.Add(Instruction.Create(OpCodes.Ret));
            
            newMethodBody.Add(gamma);
            newMethodBody.Add(Instruction.Create(OpCodes.Callvirt, hasErrored));
            newMethodBody.Add(Instruction.Create(OpCodes.Brfalse, delta)); 
            
            newMethodBody.Add(Instruction.Create(OpCodes.Ldloc_0));  
            newMethodBody.Add(Instruction.Create(OpCodes.Callvirt, thrownException));
            newMethodBody.Add(Instruction.Create(OpCodes.Throw));

            newMethodBody.Add(delta);

            return newMethodBody;
        }
        
        private static MethodDefinition GetMethod(TypeDefinition qType, string fName)
        {
            return qType.Methods.FirstOrDefault(f => f.Name == fName);
        }
        
        private static FieldDefinition GetField(TypeDefinition qType, string fName)
        {
            return qType.Fields.FirstOrDefault(f => f.Name == fName);
        }
        
        private static PropertyDefinition GetProperty(TypeDefinition qType, string fName)
        {
            return qType.Properties.FirstOrDefault(f => f.Name == fName);
        }

        private static TypeDefinition GetOrImportType(ModuleDefinition module, Type t)
        {
            var types = module.GetTypes().ToList();
            foreach (var type in types)
            {
                if (type.FullName.Equals(t.FullName))
                { 
                    return type;
                }
            }

            return module.ImportReference(t).Resolve();
        }
        
        private static void WrapEncapsulateMethod(AssemblyDefinition assembly, ModuleDefinition module,
            TypeDefinition type, MethodDefinition method, string overrideName = null, string startMethod = nameof(AOPProcessor.OnMethodStart),
            string completeMethod = nameof(AOPProcessor.OnMethodComplete))
        {
            // Compute for default()
            var isVoidMethod = method.ReturnType == module.TypeSystem.Void;
            var opShift = isVoidMethod ? 2 : 3;
            
            // Load AspectData definitions
            var aData = GetOrImportType(module, typeof(AspectData));
            var aDataCtor = aData.GetConstructors().FirstOrDefault();

            // Start constructing method override
            var newMethodBody = new List<Instruction>();
            var startInstructions = CreateAOPInjectableInstructions(assembly, module, type, method, overrideName, startMethod);
            var endInstructions = CreateAOPInjectableInstructions(assembly, module, type, method, overrideName, completeMethod);

            // If return type is not void then prepare default value for returning
            if (!isVoidMethod)
            {
                if (method.ReturnType == module.TypeSystem.Object ||
                    method.ReturnType == module.TypeSystem.Object)
                {
                    newMethodBody.Add(Instruction.Create(OpCodes.Ldnull));
                    newMethodBody.Add(Instruction.Create(OpCodes.Stloc_2));
                }
                else
                {
                    if(method.ReturnType == module.TypeSystem.UInt64 ||
                       method.ReturnType == module.TypeSystem.Int64)
                        newMethodBody.Add(Instruction.Create(OpCodes.Ldc_I8, 0));
                    else if(method.ReturnType == module.TypeSystem.Double)
                        newMethodBody.Add(Instruction.Create(OpCodes.Ldc_R8, 0d));
                    else if(method.ReturnType == module.TypeSystem.Single)
                        newMethodBody.Add(Instruction.Create(OpCodes.Ldc_R4, 0f));
                    else if (method.ReturnType == module.TypeSystem.IntPtr ||
                             method.ReturnType == module.TypeSystem.UIntPtr)
                    {
                        newMethodBody.Add(Instruction.Create(OpCodes.Ldc_I4, 0));
                        newMethodBody.Add(Instruction.Create(OpCodes.Conv_I));
                    }
                    else
                    {
                        newMethodBody.Add(Instruction.Create(OpCodes.Ldc_I4, 0));
                    }
                    newMethodBody.Add(Instruction.Create(OpCodes.Stloc_2));
                }
            }

            var instructionZero = Instruction.Create(OpCodes.Nop);
            
            // Create new AspectData object 
            newMethodBody.Add(instructionZero);
            newMethodBody.Add(Instruction.Create(OpCodes.Newobj, aDataCtor));
            newMethodBody.Add(Instruction.Create(OpCodes.Stloc_0));
            
            // Create new body
            newMethodBody.AddRange(startInstructions);
            
            // Create try-catch closure
            var handlerNop = Instruction.Create(OpCodes.Nop);
            var finishNop = Instruction.Create(OpCodes.Nop);

            var tryLeave = Instruction.Create(OpCodes.Leave, handlerNop);
            var catchLeave = Instruction.Create(OpCodes.Leave, finishNop);
           
            var handler = new ExceptionHandler (ExceptionHandlerType.Catch) {
                TryStart = instructionZero,
                TryEnd = handlerNop,
                HandlerStart = handlerNop,
                HandlerEnd = finishNop,
                CatchType = module.ImportReference (typeof (Exception))
                
            };

            
            // Method body
            var iCount = method.Body.Instructions.Count;
            for (var index = 0; index < iCount; index++)
            {
                // Get instruction
                var instr0 = method.Body.Instructions[index];

                // If returns
                if (instr0.OpCode == OpCodes.Ret)
                {
                    // Add end method callback and append instruction
                    if (newMethodBody[^1].OpCode == OpCodes.Unbox_Any)
                    {
                        // Copy and delete old instructions
                        var iCopy = newMethodBody[^1];
                        newMethodBody.RemoveAt(newMethodBody.Count - 1);

                        newMethodBody.AddRange(endInstructions);
                        newMethodBody.Add(iCopy);
                    }
                    else
                    {
                        newMethodBody.AddRange(endInstructions);
                    }

                    newMethodBody.Add(instr0);
                    continue;
                }

                // Shift locals
                if (instr0.OpCode == OpCodes.Stloc)
                    instr0 = CreateStLocInstruction(((int) instr0.Operand) + opShift);
                else if (instr0.OpCode == OpCodes.Ldloc)
                    instr0 = CreateLdLocInstruction(((int) instr0.Operand) + opShift);
                else if (instr0.OpCode == OpCodes.Stloc_0)
                    instr0 = CreateStLocInstruction(opShift);
                else if (instr0.OpCode == OpCodes.Stloc_1)
                    instr0 = CreateStLocInstruction(opShift + 1);
                else if (instr0.OpCode == OpCodes.Stloc_2)
                    instr0 = CreateStLocInstruction(opShift + 2);
                else if (instr0.OpCode == OpCodes.Stloc_3)
                    instr0 = CreateStLocInstruction(opShift + 3);
                else if (instr0.OpCode == OpCodes.Ldloc_0)
                    instr0 = CreateLdLocInstruction(opShift);
                else if (instr0.OpCode == OpCodes.Ldloc_1)
                    instr0 = CreateLdLocInstruction(opShift + 1);
                else if (instr0.OpCode == OpCodes.Ldloc_2)
                    instr0 = CreateLdLocInstruction(opShift + 2);
                else if (instr0.OpCode == OpCodes.Ldloc_3)
                    instr0 = CreateLdLocInstruction(opShift + 3);

                if (instr0.OpCode == OpCodes.Br_S)
                    continue;

                // If does not return append instruction
                newMethodBody.Add(instr0);
            }

            var adr = GetOrImportType(module, typeof(AspectData));
            var setExceptionSource = GetMethod(adr, nameof(AspectData.SetException));
            
            // End try closure
            newMethodBody.Add(tryLeave);
            newMethodBody.Add(handlerNop);
            newMethodBody.Add(Instruction.Create(OpCodes.Stloc_1));
            newMethodBody.Add(Instruction.Create(OpCodes.Ldloc_0));
            newMethodBody.Add(Instruction.Create(OpCodes.Ldc_I4, (int) ExceptionSource.Method));
            newMethodBody.Add(Instruction.Create(OpCodes.Ldloc_1)); // Stash exception
            newMethodBody.Add(Instruction.Create(OpCodes.Callvirt, setExceptionSource));
            
            newMethodBody.AddRange(CreateAOPInjectableInstructions(assembly, module, type, method, overrideName, nameof(AOPProcessor.OnCatchExceptionEnterAspect)));
            newMethodBody.AddRange(CreateAOPInjectableInstructions(assembly, module, type, method, overrideName, nameof(AOPProcessor.OnCatchExceptionExitAspect)));
            newMethodBody.Add(catchLeave);
            newMethodBody.Add(finishNop);
            
            if (!isVoidMethod)
            {
                newMethodBody.Add(Instruction.Create(OpCodes.Ldloc_2));
            }
            
            //newMethodBody.Add(Instruction.Create(OpCodes.Throw));
            newMethodBody.Add(Instruction.Create(OpCodes.Ret));

            // Create variable at first spot
            var tempVar = new VariableDefinition(aData);
            method.Body.Variables.Insert(0, tempVar);
            method.Body.Variables.Insert(1, new VariableDefinition(module.ImportReference(typeof(Exception))));
            
            if(!isVoidMethod)
                method.Body.Variables.Insert(2, new VariableDefinition(method.ReturnType));
            
            // Register exception handlers
            method.Body.ExceptionHandlers.Add(handler);
            
            // Update body
            method.Body.Instructions.Clear();
            
            // Register instructions
            foreach (var i in newMethodBody)
                method.Body.Instructions.Add(i);
            newMethodBody.Clear();
        }
        
        /// <summary>
        /// Marks member as processed by AOP
        /// </summary>
        public static void MarkAsProcessed(ModuleDefinition module, IMemberDefinition obj)
        {
            // For Assembly-CSharp load AOPGeneratedAttribute..ctor from local, otherwise reference ..ctor
            if(module.HasType(typeof(AOPGeneratedAttribute)))
            {
                var attribute = module.GetType(typeof(AOPGeneratedAttribute))
                    .GetConstructors().First();
                obj.CustomAttributes.Add(new CustomAttribute(attribute));
            }
            else
            {
                var attribute = module.ImportReference(
                    typeof(AOPGeneratedAttribute).GetConstructors((BindingFlags) int.MaxValue)
                        .First());
                obj.CustomAttributes.Add(new CustomAttribute(attribute));
            }
        }

        public static Instruction CreateStLocInstruction(int fi)
        {
            switch (fi)
            {
                case 0:
                    return Instruction.Create(OpCodes.Stloc_0);
                case 1:
                    return Instruction.Create(OpCodes.Stloc_1);
                case 2:
                    return Instruction.Create(OpCodes.Stloc_2);
                case 3:
                    return Instruction.Create(OpCodes.Stloc_3);
            }
            
            return Instruction.Create(OpCodes.Stloc, fi);
        }
        
        public static Instruction CreateLdLocInstruction(int fi)
        {
            switch (fi)
            {
                case 0:
                    return Instruction.Create(OpCodes.Ldloc_0);
                case 1:
                    return Instruction.Create(OpCodes.Ldloc_1);
                case 2:
                    return Instruction.Create(OpCodes.Ldloc_2);
                case 3:
                    return Instruction.Create(OpCodes.Ldloc_3);
            }
            
            return Instruction.Create(OpCodes.Stloc, fi);
        }
        
        /// <summary>
        /// Weave all assemblies available at specified path
        /// </summary>
        /// <param name="dllFiles">Files to weave. Must be *.dll</param>
        public static void WeaveAssembliesAtPaths(string[] dllFiles)
        {
            // Construct resolvers
            var resolver = new DefaultAssemblyResolver();
            var mdResolver = new MetadataResolver(resolver);

            var paths = new List<string>();
            
            // add .NET runtime dir for the sake of security
            foreach (var dir in Directory.GetDirectories(RuntimeEnvironment.GetRuntimeDirectory(), 
                         "*", SearchOption.AllDirectories))
            {
                resolver.AddSearchDirectory(dir);
            }
            
            foreach (var dir in dllFiles)
            {
                var path = Path.GetDirectoryName(dir);
                if (!paths.Contains(path)) paths.Add(path);
            }
            
            foreach(var dir in paths)
                resolver.AddSearchDirectory(dir);

            
            Debug.Log($"[Unity AOP] Weaving assemblies...");
            foreach (var filePath in dllFiles)
            {
                
                // In debug mode weave only CSharp DLLs (skip all PlasticSCM etc.)
                #if DEBUG_MODE
                    if (!filePath.Contains("CSharp")) continue;    
                #endif
                
                Debug.Log($"[Unity AOP] Weaving {filePath}");
                try 
                {
                    // Ignore MONO Cecil
                    if (filePath.Contains("Mono.Cecil.Rocks.dll") ||
                        filePath.Contains("Mono.Cecil.dll") ||
                        filePath.Contains("Newtonsoft.Json.dll")) return;

                    var assembly = AssemblyDefinition.ReadAssembly(filePath, new ReaderParameters {ReadWrite = true, 
                        AssemblyResolver = resolver, MetadataResolver = mdResolver});
                    
                    WeaveAssembly(assembly);
   
                    assembly.Write();			
                    assembly.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(
                        $"[Unity AOP] Failed to weave assembly '{filePath}': {ex.Message} \r\n{ex.StackTrace}");

                }
            }
        }

        /// <summary>
        /// Get editor assemblies paths
        /// </summary>
        /// <returns></returns>
        public static string[] GetEditorAssembliesPaths()
        { 
            var directoryPath = Application.dataPath + $"/../Library/ScriptAssemblies/";
            if (Directory.Exists(directoryPath))
            {
                var dllFiles = Directory.GetFiles(directoryPath, "*.dll");
                return dllFiles;
            }

            return Array.Empty<string>();
        }
        
        /// <summary>
        /// Weave assemblies in editor
        /// </summary>
        public static void WeaveEditorAssemblies()
        {
            var dllFiles = GetEditorAssembliesPaths();
            WeaveAssembliesAtPaths(dllFiles);
        }

        /// <summary>
        /// Check if member has attribute
        /// </summary>
        public static bool HasAttributeOfType<T>(IMemberDefinition member)
        {
            var subtypes= FindSubtypesOf<T>();
            foreach (var attribute in member.CustomAttributes)
            {
                foreach (var st in subtypes)
                {
                    if (attribute.AttributeType.FullName.Equals(st.FullName)) return true;
                }

            }  
            return false;
        }

        /// <summary>
        /// Find subtypes of T, used for finding children attributes for aspects
        /// </summary>
        public static List<Type> FindSubtypesOf<T>()
        {
            var outObj = new List<Type>();

            // Check cache for result (improves efficiency)
            var mainType = typeof(T);
            if (_typeCache?.ContainsKey(mainType) ?? false)
                return _typeCache[mainType];
            
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in a.GetTypes())
                { 
                    if (mainType.IsAssignableFrom(t))
                    {
                        outObj.Add(t);
                    }
                }
            }

            if(_typeCache != null)
                _typeCache[mainType] = outObj;

            return outObj;
        }
    }
}

