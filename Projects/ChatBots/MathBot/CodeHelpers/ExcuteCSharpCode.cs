using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MathBot.CodeHelpers
{
    public class ExcuteCSharpCode
    {
        public object Excute()
        {
            // Load code from file  
            //StreamReader sReader = new StreamReader(@"~/CodeSnips/CSharp.txt");
            //string input = sReader.ReadToEnd();
            //sReader.Close();

            // Code literal  
            string code =
                @"using System;  
  
                  namespace WinFormCodeCompile  
                  {  
                      public class Transform  
                      {  
                         
                           public string Hello(string input)  
                           {
                             return 1.ToString();
                           }  
                       }  
                   }";

            // Compile code  
            CSharpCodeProvider cProv = new CSharpCodeProvider();
            CompilerParameters cParams = new CompilerParameters();
            cParams.ReferencedAssemblies.Add("mscorlib.dll");
            cParams.ReferencedAssemblies.Add("System.dll");
            cParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cParams.GenerateExecutable = false;
            cParams.GenerateInMemory = true;

            CompilerResults cResults = cProv.CompileAssemblyFromSource(cParams, code);

            // Check for errors  
            if (cResults.Errors.Count != 0)
            {
                foreach (var er in cResults.Errors)
                {
                    Console.WriteLine(er.ToString());
                }
                return "Error";
            }
            else
            {
                // Attempt to execute method.  
                object obj = cResults.CompiledAssembly.CreateInstance("WinFormCodeCompile.Transform");
                Type t = obj.GetType();
                object[] arg = { "" }; // Pass our textbox to the method  
                return t.InvokeMember("Hello", BindingFlags.InvokeMethod, null, obj, arg);
            }
        }
    }
}