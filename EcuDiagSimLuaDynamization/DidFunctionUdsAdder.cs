#region License

// // MIT License
// //
// // Copyright (c) 2024 Joerg Frank
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

namespace EcuDiagSimLuaDynamization
{
    internal class DidFunctionUdsAdder : LuaSyntaxRewriter
    {
        public static SyntaxNode Rewrite(SyntaxNode node)
        {
            var rewriter = new DidFunctionUdsAdder();
            return rewriter.Visit(node);
        }

        private bool _isUpdateDidData = false;
        private bool _isGetDidData = false;


        private DidFunctionUdsAdder()
        {
        }

        public override SyntaxNode? VisitCompilationUnit(CompilationUnitSyntax node)
        {
            var statements = VisitList(node.Statements.Statements);

            if (!_isUpdateDidData)
            {
                const string func = """

                            -- Function to update DIDs
                            function updateDidData(ecu, request)
                                local key = request:sub(4,8)  -- Extract the DID key from the request
                                local value = request:sub(10)  -- Extract the value to be assigned to the DID key
                                ecu.DIDs[key] = value  -- Update the value in the DIDs container
                                return "6E " .. key  -- Return Response
                            end

                            """;
                var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(func);
                var funcDeclarationSyntax = parsedSyntaxTree.GetRoot().DescendantNodes().OfType<FunctionDeclarationStatementSyntax>().First();
                //var leadingTrivia = SyntaxFactory.TriviaList(SyntaxFactory.EndOfLine("\n\n"));
                statements = statements.Add(funcDeclarationSyntax);
            }

            if (!_isGetDidData)
            {
                const string func = """
                            
                            -- Function to retrieve a value from DIDs
                            function getDidData(ecu, request)
                                local key = request:sub(4)  -- Extract the key from the request starting from the 4th character
                                local value = ecu.DIDs[key]  -- Retrieve the value associated with the key in the DIDs table
                                if value then
                                    return "62 " .. key .. " " .. value  -- Return the Response with the retrieved value
                                else
                                    return "7F 22 11"  -- Return a default error message if the key is not found
                                end
                            end

                            """;
                var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(func);
                var funcDeclarationSyntax = parsedSyntaxTree.GetRoot().DescendantNodes().OfType<FunctionDeclarationStatementSyntax>().First();
                statements = statements.Add(funcDeclarationSyntax);
            }

            var statementList = node.Statements.WithStatements(statements);
            return node.WithStatements(statementList);
        }

        public override SyntaxNode? VisitFunctionDeclarationStatement(FunctionDeclarationStatementSyntax node)
        {

            if (node is { Name.Name.Value: "updateDidData" })
            {
                _isUpdateDidData = true;
            }

            if (node is { Name.Name.Value: "getDidData" })
            {
                _isGetDidData = true;
            }

            return base.VisitFunctionDeclarationStatement(node);
        }
    }
}
