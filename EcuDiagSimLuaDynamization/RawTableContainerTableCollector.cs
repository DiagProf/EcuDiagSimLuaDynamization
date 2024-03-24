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

using System.Collections.Immutable;
using Loretta.CodeAnalysis;
using Loretta.CodeAnalysis.Lua;
using Loretta.CodeAnalysis.Lua.Syntax;

namespace EcuDiagSimLuaDynamization
{
    // The RawTableContainerTableCollector class is collecting
    // Tables that contain a specific table named "Raw" within their structure.
    internal class RawTableContainerTableCollector : LuaSyntaxWalker
    {
        public static ImmutableArray<TableConstructorExpressionSyntax> Collect(SyntaxNode node)
        {
            var collector = new RawTableContainerTableCollector();
            collector.Visit(node);
            return collector._tableConstructors.ToImmutable();
        }

        private readonly ImmutableArray<TableConstructorExpressionSyntax>.Builder _tableConstructors;

        private RawTableContainerTableCollector() : base(SyntaxWalkerDepth.Node)
        {
            _tableConstructors = ImmutableArray.CreateBuilder<TableConstructorExpressionSyntax>();
        }

        public override void VisitTableConstructorExpression(TableConstructorExpressionSyntax table)
        {
            // This checks that the table field is in the form Raw = { ... }
            if (table.Fields.Any(x => x is IdentifierKeyedTableFieldSyntax { Identifier.Value: "Raw", Value: TableConstructorExpressionSyntax }))
            {
                _tableConstructors.Add(table);
                return; //We've found what we were looking for, so there's no need to go deeper. don't call base
            }
            // If you want to handle ["Raw"] = { ... } case as well
            else if (table.Fields.Any(x => x is ExpressionKeyedTableFieldSyntax
                     {
                         Key: LiteralExpressionSyntax { RawKind: (int)SyntaxKind.StringLiteralExpression, Token.Value: "Raw" },
                         Value: TableConstructorExpressionSyntax
                     }))
            {
                _tableConstructors.Add(table);
                return; //We've found what we were looking for, so there's no need to go deeper. don't call base;
            }

            base.VisitTableConstructorExpression(table);
        }
    }
}
