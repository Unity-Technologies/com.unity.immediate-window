using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
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

        private Globals Globals { get; set; }
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
            SymbolCount = 1;
            Globals = new Globals();
            List<MetadataReference> references = new List<MetadataReference>();
            references.Add(MetadataReference.CreateFromFile(typeof(Evaluator).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(UnityEditor.BuildOptions).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(UnityEngine.Application).Assembly.Location));
            var options = ScriptOptions.Default.WithReferences(references);

            State = await CSharpScript.RunAsync("", options, globals: Globals);
        }
        
        public async void Evaluate(string code)
        {
            OnBeforeEvaluation(code);
            var error = await EvaluateSilently(code);

            // Don't call delegate method in try/catch block to avoid silencing throws
            if (error == null)
                OnEvaluationSuccess(State.ReturnValue);
            else
            {
                var message = string.Join(Environment.NewLine, error.Diagnostics);
                OnEvaluationError(message, error);                
            }
        }

        // Evaluate without telling the delegates
        async Task<CompilationErrorException> EvaluateSilently(string code)
        {
            CompilationErrorException error = null;
            try
            {
                State = await State.ContinueWithAsync(code);
            }
            catch (CompilationErrorException e)
            {
                error = e;
            }

            return error;
        }

        private Action OnUnRegister;
        private int SymbolCount = 1;        // 0 is reserved for generic objects
        
        /**
         * The idea here is to create a shorthand for an object that is typed as the object so as to be easily used
         * To achieve that, we assign the given object to a reserved global generic object, and then create a variable that is casted to the correct type for that symbol
         */
        public async Task<string> AddToNextGlobal(object obj, Action onUnRegister = null)
        {
            //UnityEditor.ImmediateWindow.UI.ComplexStruct a = _ImmediateWindowReservedGlobal;
            var targetType = obj.GetType();
            var symbol = "_0";

            if (targetType.IsVisible)
            {
                symbol = string.Format("_{0}", SymbolCount++);
                Globals._ImmediateWindowReservedGlobal = obj;
                var code = string.Format("var {0} = ({1})_ImmediateWindowReservedGlobal;", symbol, targetType.FullName);
                var error = await EvaluateSilently(code);
            
                if (error != null)
                    Debug.Log("Error while trying to add a symbol: " + error.Message);                
            }

            if (OnUnRegister != null)
                OnUnRegister();
            
            Globals._0 = obj;
            OnUnRegister = onUnRegister;
            
            return symbol;
        }
    }
}
