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

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;

namespace Pickaxe.Runtime
{
    public class DownloadPage : IRow
    {
        public static DownloadPage CreateEmpty()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("");
            return new DownloadPage() { url = string.Empty, nodes = new[] { doc.DocumentNode }, date = DateTime.MinValue, size = 0 };
        }

        public virtual string url { get; set; }

        public virtual IEnumerable<HtmlNode> nodes { get; set; }
        public virtual DateTime? date { get; set; }
        public virtual int? size { get; set; }

        public virtual void Clear()
        {
            nodes = null;
        }

        public virtual DownloadPage CssWhere(string selector)
        {
            var newNodes = nodes.First().QuerySelectorAll(selector).ToArray();
            return new DownloadPage() { date = date, nodes = newNodes, size = size, url = url };
        }

        public static TableDescriptor Columns
        {
            get
            {
                var propertyInfos = typeof(DownloadPage).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var variablePair = propertyInfos.Select(x => new VariableTypePair() { Variable = x.Name, Primitive = TablePrimitive.FromType(x.PropertyType) }).ToList();
                return new TableDescriptor(typeof(DownloadPage)) { Variables = variablePair };
            }
        }
    }
}
