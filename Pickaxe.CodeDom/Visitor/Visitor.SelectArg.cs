﻿/* Copyright 2015 Brock Reeve
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Pickaxe.Sdk;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pickaxe.CodeDom.Visitor
{
    public partial class CodeDomGenerator : IAstVisitor
    {
        public void Visit(SelectArg arg)
        {
            var childDomArgs = new List<CodeDomArg>();
            foreach (var childArg in arg.Args)
            {
                var domArgs = VisitChild(childArg, new CodeDomArg() { Scope = _codeStack.Peek().Scope });
                childDomArgs.Add(domArgs);
            }
            
            var expression = childDomArgs[0].CodeExpression;
            var scope = childDomArgs[0].Scope;
            for (int x = 1; x < childDomArgs.Count; x++)
            {
                expression = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(string)), "Concat", expression, childDomArgs[x].CodeExpression);
                scope = childDomArgs[x].Scope;
            }

            if(arg.Args.Length == 1) //only add column name if only one arg. It could be arg + arg
                _codeStack.Peek().ParentStatements.AddRange(childDomArgs[0].ParentStatements);
            else
            {
                _codeStack.Peek()
                 .ParentStatements.Add(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("result"),
                     "AddColumn",
                     new CodePrimitiveExpression("(No column name)")));
            }

            foreach(var childDomArg in childDomArgs)
            {
                if (childDomArg.Tag != null)
                    _codeStack.Peek().Tag = childDomArg.Tag;
            }

            _codeStack.Peek().Scope = scope;
            _codeStack.Peek().CodeExpression = expression;
        }
    }
}
