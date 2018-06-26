using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using UnityEditor;
using UnityEditor.ImmediateWindow.UI;

namespace UnityEditor.ImmediateWindow.Services
{
    // Evaluation Manager
    internal class Evaluator
    {
        private static Evaluator instance = new Evaluator();
        public static Evaluator Instance { get { return instance; } }

        public event Action<string> OnEvaluation = delegate { };
        public event Action<string> OnBeforeEvaluation = delegate { };
        
        private ScriptState State { get; set; }

        public static void Init(ref Evaluator value)
        {
            if (value == null)  // UI window opened
            {
                value = instance;
            }
            else // Domain reload
            {
                instance = value;
            }
        }

        public Evaluator()
        {
            Init();
        }

        public async void Init()
        {
            List<MetadataReference> references = new List<MetadataReference>();
            references.Add(MetadataReference.CreateFromFile(typeof(TestClass).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(UnityEditor.BuildOptions).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(UnityEngine.Application).Assembly.Location));
            var options = ScriptOptions.Default.WithReferences(references);
                
            State = await CSharpScript.RunAsync("", options);
        }

        public async void Evaluate(string code)
        {
            OnBeforeEvaluation(code);
            
            try
            {
                State = await State.ContinueWithAsync(code);
                var result = State.ReturnValue;
                if (result == null)
                    result = "(no result -- perhaps you ended your statement with a ';' ?)";
                
                OnEvaluation(result.ToString());
            }
            catch (CompilationErrorException e)
            {
                var message = string.Join(Environment.NewLine, e.Diagnostics);
                OnEvaluation(message);
            }
        }
    }
}
