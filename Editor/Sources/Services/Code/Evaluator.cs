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

        public event Action<object> OnEvaluationSuccess = delegate { };
        public event Action<string, CompilationErrorException> OnEvaluationError = delegate { };
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
            CompilationErrorException error = null;
            try
            {
                State = await State.ContinueWithAsync(code);
            }
            catch (CompilationErrorException e)
            {
                error = e;
            }

            // Don't call delegate method in try/catch block to avoid silencing throws
            if (error == null)
                OnEvaluationSuccess(State.ReturnValue);
            else
            {
                var message = string.Join(Environment.NewLine, error.Diagnostics);
                OnEvaluationError(message, error);                
            }
        }
    }
}
