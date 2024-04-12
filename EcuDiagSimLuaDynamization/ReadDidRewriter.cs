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
    class ReadDidRewriter : LuaSyntaxRewriter
    {
        public static SyntaxNode Rewrite(SyntaxNode rawTableSyntaxNode, string ecuName, string did, ExpressionKeyedTableFieldSyntax field)
        {
            var rewriter = new ReadDidRewriter(ecuName, did,  field);
            return rewriter.Visit(rawTableSyntaxNode);
        }

        private readonly string _ecuName;
        private readonly string _did;
        private readonly ExpressionKeyedTableFieldSyntax _field;

        private ReadDidRewriter(string ecuName, string did, ExpressionKeyedTableFieldSyntax field)
        {
            _ecuName = ecuName;
            _did = did;
            _field = field;
        }

        public override SyntaxNode? VisitExpressionKeyedTableField(ExpressionKeyedTableFieldSyntax node)
        {
            // Input (node)
            //e.g.ExpressionKeyedTableFieldSyntax ExpressionKeyedTableField  ["22 22 35"] = "62 22 35 47 11",
            if (node.IsEquivalentTo(_field ))
            {
                var func = $"\tDummy = {{\r\n\t\t[\"22 {_did.Substring(0, 2) + " " + _did.Substring(2, 2)} *\"] = function(request) return getDidData({_ecuName},request ) end,\r\n    }}";
                var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(func);
                var newExpressionKeyedTableFieldSyntax = parsedSyntaxTree.GetRoot().DescendantNodes().OfType<ExpressionKeyedTableFieldSyntax>().First();
                return newExpressionKeyedTableFieldSyntax;
            }
            return base.VisitExpressionKeyedTableField(node);
        }

     
    }
}
