﻿using CPPReferenceDocumentation.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace CPPReferenceDocumentation
{
    class UrlGenerator :
        IContainerUrlGenerator, 
        IAlgorithmUrlGenerator,
        IStringUrlGenerator,
        IStreamsUrlGenerator,
        IMemoryUrlGenerator,
        IUtilityUrlGenerator,
        IThreadsUrlGenerator,
        IFilesystemUrlGenerator
    {
        private readonly string baseRoute = "https://en.cppreference.com/w/cpp";
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
                    result = this.GenerateStringUrl();

                    if (result.Length == 0)
                    {
                        result = this.GenerateStreamsUrl();

                        if (result.Length == 0)
                        {
                            result = this.GenerateAlgorithmUrl();

                            if (result.Length == 0)
                            {

                            }
                        }
                    }
                }

                return result.Length == 0 ?
                    $"https://duckduckgo.com/?sites=cppreference.com&q={data}&ia=web" :
                    result;
            }
        }

        public string GenerateAlgorithmUrl()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{baseRoute}/algorithm/{data}";

                if (client.GetAsync(url).Result.IsSuccessStatusCode)
                {
                    return url;
                }
            }

            return "";
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

            return $"{baseRoute}/container/{data}";
        }

        public string GenerateFilesystemUrl()
        {
            throw new NotImplementedException();
        }

        public string GenerateMemoryUrl()
        {
            throw new NotImplementedException();
        }

        public string GenerateStreamsUrl()
        {
            #region Input streams
            if (data == "cin" || data == "wcin")
            {
                return $"{baseRoute}/io/cin";
            }

            if (data == "istream" || data == "wistream" || data == "basic_istream")
            {
                return $"{baseRoute}/io/basic_istream";
            }

            if (data == "ifstream" || data == "wifstream" || data == "basic_ifstream")
            {
                return $"{baseRoute}/io/basic_ifstream";
            }

            if (data == "istringstream" || data == "wistringstream" || data == "basic_istringstream")
            {
                return $"{baseRoute}/io/basic_istringstream";
            }
            #endregion

            #region Output streams
            if (data == "cout" || data == "wcout")
            {
                return $"{baseRoute}/io/cout";
            }

            if (data == "ostream" || data == "wostream" || data == "basic_ostream")
            {
                return $"{baseRoute}/io/basic_ostream";
            }

            if (data == "ofstream" || data == "wofstream" || data == "basic_ofstream")
            {
                return $"{baseRoute}/io/basic_ofstream";
            }

            if (data == "ostringstream" || data == "wostringstream" || data == "basic_ostringstream")
            {
                return $"{baseRoute}/io/basic_ostringstream";
            }

            if (data == "osyncstream" || data == "wosyncstream" || data == "basic_osyncstream")
            {
                return $"{baseRoute}/io/basic_osyncstream";
            }
            #endregion

            #region Utility streams
            if (data == "cerr" || data == "wcerr")
            {
                return $"{baseRoute}/io/cerr";
            }

            if (data == "clog" || data == "wclog")
            {
                return $"{baseRoute}/io/clog";
            }
            #endregion

            #region I/O streams
            if (data == "iostream" || data == "wiostream" || data == "basic_iostream")
            {
                return $"{baseRoute}/io/basic_iostream";
            }

            if (data == "stringstream" || data == "wstringstream" || data == "basic_stringstream")
            {
                return $"{baseRoute}/io/basic_stringstream";
            }
            #endregion

            return "";
        }
        public string GenerateStringUrl()
        {
            if (data == "string" || data == "wstring" || data == "u8string" || data == "u16string" || data == "u32string" || data == "basic_string")
            {
                return $"{baseRoute}/string/basic_string";
            }
            else if (data == "string_view" || data == "wstring_view" || data == "u8string_view" || data == "u16string_view" || data == "u32string_view" || data == "basic_string_view")
            {
                return $"{baseRoute}/string/basic_string_view";
            }

            return "";
        }

        public string GenerateThreadsUrl()
        {
            throw new NotImplementedException();
        }

        public string GenerateUtilityUrl()
        {
            throw new NotImplementedException();
        }
    }
}
