using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Recommendations;
using Microsoft.CodeAnalysis.Text;
using UnityEditor.ImmediateWindow.Services;

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

        private AdhocWorkspace Workspace { get; set; }
        private SourceText Text { get; set; }
        private string Code { get; set; }

        private Globals Globals { get; set; }
        private ScriptState ScriptState { get; set; }

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
            foreach(var assembly in Inspector.GetReferencableAssemblies())
                references.Add(MetadataReference.CreateFromFile(assembly.Location));

            /* For easier debugging
            references.Add(MetadataReference.CreateFromFile(typeof(Evaluator).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(UnityEditor.BuildOptions).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(UnityEngine.Application).Assembly.Location));
            */
            
            var options = ScriptOptions.Default.WithReferences(references);

            ScriptState = await CSharpScript.RunAsync("", options, globals: Globals);
            
            // Set some default common namespaces for easier debugging
            await AddNamespace(new List<string>
            {
                "UnityEngine",
                "UnityEditor",
                "System",
                "System.Collections",
                "System.Collections.Generic",
                "System.Linq"
            });

            /* Test using workspaces
        string _text = 
            @"using System;using UnityE
namespace TEST
{
    public class MyClass
    {
        public static void Print()
        {
            Console.WriteLine(""Hello World"");
        }
    }
}";
            Workspace = new AdhocWorkspace();
            string projName = "Immediate Window";
            ProjectId projId = ProjectId.CreateNewId();
            VersionStamp versionStamp = VersionStamp.Create();
            ProjectInfo projInfo = ProjectInfo.Create(projId, versionStamp, projName, projName, LanguageNames.CSharp);
            SourceText sourceText = SourceText.From(_text);

            projInfo = projInfo.WithMetadataReferences(references);
            Workspace.AddProject(projInfo);
            Document doc = Workspace.AddDocument(Workspace.CurrentSolution.ProjectIds[0], "console.cs", sourceText);

            SemanticModel semanticModel = doc.GetSemanticModelAsync().Result;

            IEnumerable<Diagnostic> diagnostics = semanticModel.GetDiagnostics();
            IEnumerable<ISymbol> symbols = Recommender.GetRecommendedSymbolsAtPositionAsync(semanticModel, 25, Workspace).Result;
            foreach (var symbol in symbols)
            {
                Debug.Log($"Symbol: {symbol.Name}");
            }
            */
        }
        
        public async void Evaluate(string code)
        {
            OnBeforeEvaluation(code);
            var error = await EvaluateSilently(code);

            // Don't call delegate method in try/catch block to avoid silencing throws
            if (error == null)
                OnEvaluationSuccess(ScriptState.ReturnValue);
            else
            {
                var message = string.Join(Environment.NewLine, error.Diagnostics);
                OnEvaluationError(message, error);                
            }
        }

        // Evaluate without telling the delegates
        public async Task<CompilationErrorException> EvaluateSilently(string code)
        {
            CompilationErrorException error = null;
            try
            {
                ScriptState = await ScriptState.ContinueWithAsync(code);
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
            var targetType = obj.GetType();
            var symbol = "_0";

            if (targetType.IsVisible)
            {
                var parsed = new ParsedAssemblyQualifiedName.ParsedAssemblyQualifiedName(targetType.AssemblyQualifiedName);

                symbol = string.Format("_{0}", SymbolCount++);
                Globals._ImmediateWindowReservedGlobal = obj;
                var code = string.Format("var {0} = _ImmediateWindowReservedGlobal as {1};", 
                    symbol, parsed.CSharpStyleName.Value);
                var error = await EvaluateSilently(code);
            
                if (error != null)
                    Debug.Log("Error while trying to add a symbol: " + error.Message + " -- Code: " + code + "\n");                
            }

            if (OnUnRegister != null)
                OnUnRegister();
            
            OnUnRegister = onUnRegister;
            
            return symbol;
        }

        // More then a little possibly not the best way to do this. There has to be a more
        // solid way to add a namespace without simply re-evaluating.
        public async Task<CompilationErrorException> AddNamespace(string ns)
        {
            return await AddNamespace(new List<string> {ns});
        }
        
        public async Task<CompilationErrorException> AddNamespace(IEnumerable<string> namespaces)
        {
            State.Instance.Namespaces.AddRange(namespaces);

            var code = "";
            foreach (var ns in namespaces)
                code += $"using {ns};\n";

            var error = await EvaluateSilently(code);
            if (error != null)
                Debug.Log("Error adding namespace: " + error.Message + "\n\n" + code);

            return error;
        }

        public void GetAutocomplete(string code)
        {
            /*
            var syntaxTree = ScriptState.Script.GetCompilation().SyntaxTrees.First();
            var semanticModel = ScriptState.Script.GetCompilation().GetSemanticModel(syntaxTree);

            AdhocWorkspace workspace = new AdhocWorkspace();
            var symbols = await Recommender.GetRecommendedSymbolsAtPositionAsync(semanticModel, code.Length - 2, workspace);
            foreach (var symbol in symbols)
            {
                Debug.Log($"Symbol: {symbol.Name}");
            }
            */
        }
    }
}
