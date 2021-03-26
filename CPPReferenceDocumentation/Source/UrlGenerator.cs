using CPPReferenceDocumentation.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPPReferenceDocumentation
{
    class UrlGenerator : IContainerUrlGenerator, IAlgorithmUrlGenerator, IStringUrlGenerator, IStreamsUrlGenerator
    {
        private readonly string baseRoute = "https://en.cppreference.com/w/cpp/";
        private string data;
        private HashSet<string> containers;

        public string RawView
        {
            set
            {
                data = value.Contains("std::") ? value.Substring(5) : value;
            }
            get
            {
                string result;

                result = this.GenerateContainerUrl();

                if (result.Length == 0)
                {

                }

                return result;
            }
        }

        public string GenerateAlgorithmUrl()
        {
            throw new NotImplementedException();
        }

        public string GenerateContainerUrl()
        {
            if (containers == null)
            {
                containers = new HashSet<string>(17)
                {
                    "array",
                    "vector",
                    "deque",
                    "forward_list",
                    "list",

                    "set",
                    "map",
                    "multiset",
                    "multimap",

                    "unordered_set",
                    "unordered_map",
                    "unordered_multiset",
                    "unordered_multimap",

                    "stack",
                    "queue",
                    "priority_queue",

                    "span"
                };
            }

            if (!containers.Contains(data))
            {
                return "";
            }

            return baseRoute + "container/" + data;
        }

        public string GenerateStreamsUrl()
        {
            throw new NotImplementedException();
        }
        public string GenerateStringUrl()
        {
            throw new NotImplementedException();
        }
    }
}
