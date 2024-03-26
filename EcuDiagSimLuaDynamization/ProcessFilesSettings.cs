#region License

// // MIT License
// //
// // Copyright (c) ${CurrentDate.Year} Joerg Frank
// // http://www.diagprof.com/
// //
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// //
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// //
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.

#endregion

using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using Loretta.CodeAnalysis;
using Spectre.Console.Cli;
using Loretta.CodeAnalysis.Lua;
using Loretta.CodeAnalysis.Lua.Syntax;
using Spectre.Console;

namespace EcuDiagSimLuaDynamization
{
    public class ProcessFilesCommand : Command<ProcessFilesSettings>
    {
        public override int Execute(CommandContext context, ProcessFilesSettings settings)
        {
            // Ensure output directory exists
            Directory.CreateDirectory(settings.OutputDir);

            if (settings.Protocol.Equals("UDS"))
            {
                // Process each file
                foreach (var file in settings.Files)
                {
                    Console.WriteLine($"Processing file: {file} with protocol: {settings.Protocol}");
                    
                    // LUA-Code laden
                    var code = File.ReadAllText(file);

                    // LUA-Code in AST parsen
                    var syntaxTree = LuaSyntaxTree.ParseText(code, new LuaParseOptions(LuaSyntaxOptions.Lua53));


                    var hasErrors = false;
                    foreach (var diagnostic in syntaxTree.GetDiagnostics().OrderByDescending(diag => diag.Severity))
                    {
                        Console.WriteLine(diagnostic.ToString());
                        hasErrors |= diagnostic.Severity == DiagnosticSeverity.Error;
                    }
                    if (hasErrors)
                    {
                        Console.WriteLine($"File {file} has errors! Exiting...");
                        break;
                    }

                    var firstLevelTablesWithRawInside = RawTableContainerTableCollector.Collect(syntaxTree.GetRoot());

                    foreach (var rawTableInsideFirstLevelTable in firstLevelTablesWithRawInside)
                    {
     
                        var identifierKeyedRawTables = IdentifierKeyedRawTableCollector.Collect(rawTableInsideFirstLevelTable);
                        var expressionKeyedRawTables = ExpressionKeyedRawTableCollector.Collect(rawTableInsideFirstLevelTable);

                        if ( identifierKeyedRawTables.Length + expressionKeyedRawTables.Length > 1 )
                        {
                            Console.WriteLine("In a container table only one 'Raw' table is permitted.");
                            break;
                        }

                        if (identifierKeyedRawTables.Length + expressionKeyedRawTables.Length < 1)
                        {
                            //this shout never happen normaly, because wo collect only Container Table with a 'Raw' Table
                            Console.WriteLine("In a container table no 'Raw' table found.");
                            break;
                        }

                        TableConstructorExpressionSyntax? tableConstructorExpressionSyntax = null;
                        //For IdentifierKeyed
                        if ( !identifierKeyedRawTables.IsEmpty )
                        {
                            tableConstructorExpressionSyntax = (identifierKeyedRawTables.First().Value as TableConstructorExpressionSyntax);
                        }

                        //For ExpressionKeyed
                        if (!expressionKeyedRawTables.IsEmpty)
                        {
                            tableConstructorExpressionSyntax = (expressionKeyedRawTables.First().Value as TableConstructorExpressionSyntax);
                        }

                        if (tableConstructorExpressionSyntax == null)
                        {
                            Console.WriteLine("Something is wrong with the Raw table; this should not happen..");
                            break;
                        }

                        //Test do get the EcuName from the Raw Table
                        string ecuName = "notFoundEcuName";
                        var parentTable = tableConstructorExpressionSyntax.Ancestors().OfType<TableConstructorExpressionSyntax>().FirstOrDefault()?.Parent?.Parent;
                        if (parentTable.IsKind(SyntaxKind.AssignmentStatement))
                        {
                            var firstVariable = ((parentTable as AssignmentStatementSyntax)!).Variables.FirstOrDefault();
                            if ( firstVariable.IsKind(SyntaxKind.IdentifierName) )
                            {
                               
                                ecuName = ((firstVariable as IdentifierNameSyntax)!).Name;
                            }
                        }
                        Console.WriteLine($"{ecuName}");
                       


                        var rawTableFields = tableConstructorExpressionSyntax.Fields;
                        var dicDids = new Dictionary<string, DidReadAndWritePair>();
                        FindDidsOfInterestInRawTable(rawTableFields, dicDids);


                        var keysToRemove = new List<string>();
                        foreach ( var writeAndReadPair in dicDids )
                        {
                            if ( writeAndReadPair.Value.WriteSequenceField == null || writeAndReadPair.Value.ReadSequenceField == null )
                            {
                                keysToRemove.Add(writeAndReadPair.Key);
                            }
                            else
                            {
                                if ( writeAndReadPair.Value.WriteSequenceField.Key.ToString().Replace(" ", "").Length !=
                                     writeAndReadPair.Value.ReadSequenceField.Value.ToString().Replace(" ", "").Length )
                                {
                                    //We also exclude any DIDs where the length of the read data does not match the length of the written data.
                                    //Such cases do occur occasionally.
                                    keysToRemove.Add(writeAndReadPair.Key);
                                }

                            }
                        }

                        foreach (var key in keysToRemove)
                        {
                            dicDids.Remove(key);
                        }

                        foreach (var writeAndReadPair in dicDids)
                        {
                            var rw = WriteDidRewriter.Rewrite(writeAndReadPair.Key,writeAndReadPair.Value.WriteSequenceField);
                            //ToDo rework
                        }

                        
                    
                    }




                    //got help https://github.com/orgs/LorettaDevs/discussions/123  
                    // var root = syntaxTree.GetCompilationUnitRoot();
                    //var tables = root.DescendantNodes().OfType<TableConstructorExpressionSyntax>();
                    //var toAnalyze = new List<TableConstructorExpressionSyntax>();

                    //foreach (var table in tables)
                    //{
                    //    // This checks that the table field is in the form Raw = { ... }
                    //    if (table.Fields.Any(x => x is IdentifierKeyedTableFieldSyntax { Identifier.Value: "Raw", Value: TableConstructorExpressionSyntax }))
                    //    {
                    //        toAnalyze.Add(table);
                    //    }
                    //    // If you want to handle ["Raw"] = { ... } case as well
                    //    else if (table.Fields.Any(x => x is ExpressionKeyedTableFieldSyntax
                    //             {
                    //                 Key: LiteralExpressionSyntax { RawKind: (int)SyntaxKind.StringLiteralExpression, Token.Value: "Raw" },
                    //                 Value: TableConstructorExpressionSyntax
                    //             }))
                    //    {
                    //        toAnalyze.Add(table);
                    //    }
                    //}

                }
            }

            return 0;
        }

        private static void FindDidsOfInterestInRawTable(SeparatedSyntaxList<TableFieldSyntax> rawTableFields, Dictionary<string, DidReadAndWritePair> dicDids)
        {
            //Now, let's inspect the fields of the Raw table.
            foreach ( var field in rawTableFields )
            {
                if ( field is ExpressionKeyedTableFieldSyntax tableFieldSyntax )
                {
                    //We are only interested in fields like ["22 22 35"] = "62 22 35 47 11" or ["2E F1 98 12 67"] = "6E F1 98"
                    if ( tableFieldSyntax.Key.IsKind((SyntaxKind.StringLiteralExpression)) &&
                         tableFieldSyntax.Value.IsKind(SyntaxKind.StringLiteralExpression) )
                    {
                        var cleanRequestString = tableFieldSyntax.Key.ToString().Replace("\"", "");
                        if ( cleanRequestString.StartsWith("2E") || cleanRequestString.StartsWith("22") )
                        {
                            cleanRequestString = cleanRequestString.Replace(" ", "");
                            if ( cleanRequestString.Length >= 6 )
                            {
                                var did = cleanRequestString.Substring(2, 4).ToUpper();

                                if ( dicDids.ContainsKey(did) )
                                {
                                    if ( cleanRequestString.StartsWith("2E") )
                                    {
                                        dicDids[did].WriteSequenceField = tableFieldSyntax;
                                    }
                                    else
                                    {
                                        dicDids[did].ReadSequenceField = tableFieldSyntax;
                                    }
                                }
                                else
                                {
                                    if ( cleanRequestString.StartsWith("2E") )
                                    {
                                        dicDids[did] = new DidReadAndWritePair(tableFieldSyntax, null);
                                    }
                                    else
                                    {
                                        dicDids[did] = new DidReadAndWritePair(null, tableFieldSyntax);
                                    }
                                }
                            }
                            //For printf Debug :-)
                            //Console.WriteLine(tableFieldSyntax.Key.ToFullString());
                            //Console.WriteLine(tableFieldSyntax.Value);
                        }
                    }
                    else
                    {
                        //For instance, if there's already a function (AnonymousFunctionExpression) there, we move on.
                        Console.WriteLine("skip");
                    }
                }
                else
                {
                    Console.WriteLine("ToDo ???");
                }
            }
        }

        // Example methods for processing and saving files (implement these based on your requirements)
        private void ProcessFile(string filePath, string protocol)
        {
            // Implement file processing logic here
        }

        private void SaveProcessedFile(string filePath, string outputDir)
        {
            // Implement logic to save processed file here
        }
    }
    public class ProcessFilesSettings : CommandSettings
    {
        [Description("The files to process.")]
        [CommandArgument(0, "[FILES]")]
        public string[] Files { get; set; }

        [Description("Specifies the output directory where processed files will be saved.")]
        [CommandOption("-o|--output <OUTPUTDIR>")]
        [DefaultValue("./output")]
        public string OutputDir { get; set; }

        [Description("Specifies the protocol to be used (KWP2000 or UDS). Default is UDS.")]
        [CommandOption("-p|--protocol <PROTOCOL>")]
        [DefaultValue("UDS")]
        public string Protocol { get; set; }
    }
}
