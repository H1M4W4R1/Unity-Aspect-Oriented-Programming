using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ITnnovative.AOP.Testing.Framework.Attributes;
using ITnnovative.AOP.Testing.Framework.Data;
using ITnnovative.AOP.Testing.Framework.Enums;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ITnnovative.AOP.Testing.Framework.Editor
{
    public class TestWindow : EditorWindow
    {
        [HideInInspector]
        public List<AOPAssemblyTestGroup> tests = new List<AOPAssemblyTestGroup>();

        private void Update()
        {
            if (tests == null)
                tests = new List<AOPAssemblyTestGroup>();
            
            if(tests.Any(t => t.src == null))
                tests = GetAllAvailableTests(); 
        }

        private void Awake()
        {
            tests = GetAllAvailableTests(); 
        }

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Window/Unity AOP Tests")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            var hwnd = GetWindow(typeof(TestWindow), false, "AOP Tests");
            hwnd.Show();
        }

        bool DrawTestTextFoldout(bool fo, TestState pass, string text)
        {
            if (pass == TestState.Passed)
                GUI.color = Color.green;
            else if (pass == TestState.Failed)
                GUI.color = Color.red;
            else
                GUI.color = Color.white;
            
            return EditorGUILayout.Foldout(fo, text);
        }
        
        void DrawTestText(TestState pass, string text)
        {
            if (pass == TestState.Passed)
                GUI.color = Color.green;
            else if (pass == TestState.Failed)
                GUI.color = Color.red;
            else
                GUI.color = Color.white;
            
            EditorGUILayout.LabelField(text);
        }

        void OnGUI()
        {

            GUILayout.Label("Tests", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh"))
            {
                tests = GetAllAvailableTests();
            }

            if (GUILayout.Button("Run"))
            {
                foreach (var a in tests)
                {
                    if (a == null) continue;

                    a.Run();
                }
            }

            EditorGUILayout.EndHorizontal();

            if (tests == null) return;

            foreach (var assemblyTestGroup in tests)
            {
                if (assemblyTestGroup == null) continue;
                EditorGUI.indentLevel = 0;

                var srcName = assemblyTestGroup.src.FullName;

                assemblyTestGroup.show =
                    DrawTestTextFoldout(assemblyTestGroup.show, assemblyTestGroup.passed, srcName);

                if (assemblyTestGroup.show)
                {
                    foreach (var typeTestGroup in assemblyTestGroup.typeTests)
                    {
                        if (typeTestGroup == null) continue;
                        EditorGUI.indentLevel = 1;

                        var srcNameType = typeTestGroup.src.Name;

                        typeTestGroup.show =
                            DrawTestTextFoldout(typeTestGroup.show, typeTestGroup.passed, srcNameType);

                        if (typeTestGroup.show)
                        {
                            EditorGUI.indentLevel = 2;
                            EditorGUILayout.BeginVertical();
                            foreach (var test in typeTestGroup.tests)
                            {
                                if (test == null) continue;
                                EditorGUI.indentLevel = 2;

                                var testName = test.method.Name;

                                DrawTestText(test.passed, testName);
                            }

                            EditorGUILayout.EndVertical();
                        }
                    }
                }
            }

        }

        public List<AOPAssemblyTestGroup> GetAllAvailableTests()
        {
            var result = new List<AOPAssemblyTestGroup>();
            
            // Scan all methods
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                var assemblyGroup = null as AOPAssemblyTestGroup;
                foreach (var t in assembly.GetTypes())
                {
                    var testGroup = null as AOPTypeTestGroup;
                    
                    foreach (var m in t.GetMethods())
                    {
                        // If test
                        if (m.GetCustomAttribute<TestAttribute>() != null)
                        {
                            // And has parameters
                            if (m.GetParameters().Length > 0)
                            {
                                Debug.LogError($"Tests cannot have parameters! [{t.FullName}::{m.Name}]");
                                continue;
                            }

                            // Register test type
                            if (testGroup == null)
                                testGroup = new AOPTypeTestGroup()
                                {
                                    src = t
                                };

                            // Register test
                            var test = new AOPTest();
                            test.method = m;
                            
                            testGroup.AddTest(test);
                        }
                    }

                    // If type group exists
                    if (testGroup != null)
                    {
                        // Check if assembly group exists, if not create one
                        if (assemblyGroup == null)
                            assemblyGroup = new AOPAssemblyTestGroup()
                            {
                                src = assembly
                            };
                        
                        // Register type group in assembly group
                        assemblyGroup.AddTypeGroup(testGroup);
                    }
                }

                // If assembly group exists, register it
                if (assemblyGroup != null)
                    result.Add(assemblyGroup);
            }

            // Return results
            return result;
        }

    }
}