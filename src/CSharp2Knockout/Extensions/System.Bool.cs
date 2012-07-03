using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.IO;
using System.Text;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CSharp2Knockout.Extensions
{
    public static class BoolExtenssions
    {
        public static bool OrDefault(this bool? @this, bool defaultValue)
        {
            if(@this.HasValue)
            {
                return @this.Value;
            }

            return defaultValue;
        }
    }
}