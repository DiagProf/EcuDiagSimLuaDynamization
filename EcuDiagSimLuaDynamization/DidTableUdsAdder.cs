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

using Loretta.CodeAnalysis;
using Loretta.CodeAnalysis.Lua;
using Loretta.CodeAnalysis.Lua.Syntax;
using System.Collections.Generic;
using System.Text;

namespace EcuDiagSimLuaDynamization
{
    internal class DidTableUdsAdder : LuaSyntaxRewriter
    {
        public static SyntaxNode Rewrite(SyntaxNode node, Dictionary<string, DidReadAndWritePair> didReadAndWritePairs)
        {
            var rewriter = new DidTableUdsAdder(didReadAndWritePairs);
            return rewriter.Visit(node);
        }

        private bool _isDidTablealalreadyThere = false;
        private Dictionary<string, DidReadAndWritePair> _didReadAndWritePairs;

        private DidTableUdsAdder(Dictionary<string, DidReadAndWritePair> didReadAndWritePairs)
        {
            _didReadAndWritePairs = didReadAndWritePairs;
        }

        public override SyntaxNode? VisitCompilationUnit(CompilationUnitSyntax node)
        {
            var statements = VisitList(node.Statements.Statements);

            //if (!_isDidTablealalreadyThere)
            //{

            //    StringBuilder output = new StringBuilder();

            //    // Ausgabe des Dictionary-Inhalts in den StringBuilder
            //    foreach (var entry in _didReadAndWritePairs)
            //    {
            //        output.AppendFormat("[\"{0}\"] = \"{1}\",\n", entry.Key, entry.Value.ReadSequenceField.Value.ToString());
            //    }

            //    // Umwandlung des StringBuilder in einen String
            //    string result = output.ToString();


            //    const string func = """

            //                -- Function to update DIDs
            //                function updateDidData(ecu, request)
            //                    local key = request:sub(4,8)  -- Extract the DID key from the request
            //                    local value = request:sub(10)  -- Extract the value to be assigned to the DID key
            //                    ecu.DIDs[key] = value  -- Update the value in the DIDs container
            //                    local oldValue = ecu.DIDs[key]  -- Read the old value
            //                    if oldValue and #oldValue == #value then  -- Check if lengths match
            //                        ecu.DIDs[key] = value  -- Update the value in the DIDs container
            //                        return "6E " .. key  -- Return Response
            //                    else
            //                        return "7F 6E 13"  -- Handle error or mismatch case with 13 -> Incorrect message length or invalid format
            //                    end
            //                end

            //                """;
            //    var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(func);
            //    var funcDeclarationSyntax = parsedSyntaxTree.GetRoot().DescendantNodes().OfType<FunctionDeclarationStatementSyntax>().First();
               
            //    statements = statements.Add(funcDeclarationSyntax);
            //}

            var statementList = node.Statements.WithStatements(statements);
            return node.WithStatements(statementList);
        }

        public override SyntaxNode? VisitIdentifierKeyedTableField(IdentifierKeyedTableFieldSyntax field)
        {
            //This checks that the table field is in the form Raw = { ... }
            if (field is { Identifier.Value: "DIDs", Value: TableConstructorExpressionSyntax })
            {
                _isDidTablealalreadyThere = true;
            }

            return base.VisitIdentifierKeyedTableField(field);
        }

        public override SyntaxNode? VisitExpressionKeyedTableField(ExpressionKeyedTableFieldSyntax field)
        {
            if (field is ExpressionKeyedTableFieldSyntax
                {
                    Key: LiteralExpressionSyntax { RawKind: (int)SyntaxKind.StringLiteralExpression, Token.Value: "DIDs" },
                    Value: TableConstructorExpressionSyntax
                })
            {
                _isDidTablealalreadyThere = true;
            }

            return base.VisitExpressionKeyedTableField(field);
        }

        //public override SyntaxNode? VisitTableConstructorExpression(TableConstructorExpressionSyntax node)
        //{

        //    if (node is { Name.Value: "updateDidData" })
        //    {
        //        _isDidTablealalreadyThere = true;
        //    }

        //    if (node is { Name.Name.Value: "getDidData" })
        //    {
        //        _isGetDidData = true;
        //    }

        //    return base.VisitFunctionDeclarationStatement(node);
        //}
    }
}
