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
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace EcuDiagSimLuaDynamization
{
    class WriteDidRewriter : LuaSyntaxRewriter
    {
        public static SyntaxNode Rewrite(string did, ExpressionKeyedTableFieldSyntax field)
        {
            var rewriter = new WriteDidRewriter(did,  field);
            return rewriter.Visit(field);
        }

        private WriteDidRewriter(string Did, ExpressionKeyedTableFieldSyntax field)
        {
            
        }

        public override SyntaxNode? VisitExpressionKeyedTableField(ExpressionKeyedTableFieldSyntax node)
        {
            // Input 
            //e.g.ExpressionKeyedTableFieldSyntax ExpressionKeyedTableField ["2E 22 35 47 11"] = "6E 22 35",

            //ToDo rework  e.g. -> ["2E 22 35 *"] = function(request) return updateDidData(YourEcuName,request ) end,  
            //node.Update();

            return base.VisitExpressionKeyedTableField(node);
        }

     
    }
}
