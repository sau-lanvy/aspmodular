using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace AspModular.Infrastructure
{
    public class ModuleInfo
    {
        public string Name { get; set; }
        public Assembly Assembly { get; set; }
        public string ShortName
        {
            get
            {
                return Name.Split('.').Last();
            }
        }
        public string Path { get; set; }
    }
}
