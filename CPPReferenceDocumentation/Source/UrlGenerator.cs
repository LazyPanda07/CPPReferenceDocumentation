using CPPReferenceDocumentation.Source.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CPPReferenceDocumentation
{
    class UrlGenerator :
        IContainerUrlGenerator,
        IAlgorithmUrlGenerator,
        IStringUrlGenerator,
        IStreamsUrlGenerator,
        IMemoryUrlGenerator,
        IUtilityUrlGenerator,
        IThreadUrlGenerator,
        IFilesystemUrlGenerator,
        ITypesUrlGenerator,
        IDateTimeUrlGenerator,
        IIteratorUrlGenerator,
        IRangesUrlGenerator,
        IAtomicUrlGenerator,
        ICoroutineUrlGenerator,
        IRegexUrlGenerator,
        IErrorUrlGenerator,
        IConceptsUrlGenerator
    {
        private readonly string baseRoute = "https://en.cppreference.com/w/cpp";
        private string data;
        private HashSet<string> containers;
        private List<Func<string>> offlineMethods;
        private List<Func<string>> onlineMethods;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits", Justification = "<Pending>")]
        private string HttpCheckUrl(string section)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{baseRoute}/{section}/{data}";

                var task = client.GetAsync(url);

                if (task != null && task.Result.IsSuccessStatusCode)
                {
                    return url;
                }
            }

            return "";
        }

        public string RawView
        {
            set
            {
                data = value.Contains("std::") ? value.Substring(5) : value;

                if (offlineMethods == null)
                {
                    offlineMethods = new List<Func<string>>()   // offline calculation
                    {
                        GenerateContainerUrl,
                        GenerateStreamsUrl,
                        GenerateStringUrl,
                        GenerateCoroutineUrl,
                        GenerateRegexUrl,
                        GenerateErrorUrl,
                        GenerateConceptsUrl
                    };

                    onlineMethods = new List<Func<string>>()    // online check
                    {
                        GenerateAlgorithmUrl,
                        GenerateFilesystemUrl,
                        GenerateMemoryUrl,
                        GenerateThreadUrl,
                        GenerateUtilityUrl,
                        GenerateTypesUrl,
                        GenerateDateTimeUrl,
                        GenerateIteratorUrl,
                        GenerateRangesUrl,
                        GenerateAtomicUrl
                    };
                }
            }
            get
            {
                string result = "";

                foreach (var method in offlineMethods)
                {
                    result = method();

                    if (result.Length != 0)
                    {
                        break;
                    }
                }

                if (result.Length == 0)
                {
                    ConcurrentBag<string> onlineChecks = new ConcurrentBag<string>();

                    Parallel.ForEach(onlineMethods, (onlineMethod, state) =>
                    {
                        string tem = onlineMethod();

                        onlineChecks.Add(tem);

                        if (tem.Length != 0)
                        {
                            state.Break();
                        }
                    });

                    foreach (var i in onlineChecks)
                    {
                        if (i.Length != 0)
                        {
                            result = i;

                            break;
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
            return this.HttpCheckUrl("algorithm");
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
            return this.HttpCheckUrl("filesystem");
        }

        public string GenerateMemoryUrl()
        {
            return this.HttpCheckUrl("memory");
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

        public string GenerateThreadUrl()
        {
            return this.HttpCheckUrl("thread");
        }

        public string GenerateUtilityUrl()
        {
            return this.HttpCheckUrl("utility");
        }

        public string GenerateTypesUrl()
        {
            return this.HttpCheckUrl("types");
        }

        public string GenerateDateTimeUrl()
        {
            return this.HttpCheckUrl("chrono");
        }

        public string GenerateIteratorUrl()
        {
            return this.HttpCheckUrl("iterator");
        }

        public string GenerateRangesUrl()
        {
            return this.HttpCheckUrl("ranges");
        }

        public string GenerateAtomicUrl()
        {
            return this.HttpCheckUrl("atomic");
        }

        public string GenerateCoroutineUrl()
        {
            if (data == "coroutine_traits" || data == "coroutine_handle" ||
                 data == "noop_coroutine" || data == "noop_coroutine_promise" || data == "noop_coroutine_handle" ||
                 data == "suspend_never" || data == "suspend_always")
            {
                return $"{baseRoute}/coroutine/{data}";
            }

            return "";
        }

        public string GenerateRegexUrl()
        {
            if (data == "basic_regex" || data == "sub_match" || data == "match_results" ||
                 data == "regex_match" || data == "regex_search" || data == "regex_replace" ||
                 data == "regex_iterator" || data == "regex_token_iterator" ||
                 data == "regex_error" ||
                 data == "regex_traits" ||
                 data == "syntax_option_type" || data == "match_flag_type" || data == "error_type")
            {
                return $"{baseRoute}/regex/{data}";
            }

            return "";
        }

        public string GenerateErrorUrl()
        {
            if (data == "exception" ||
                data == "logic_error" || data == "domain_error" || data == "length_error" || data == "out_of_range" || data == "future_error" ||
                data == "bad_optional_access" ||
                data == "runtime_error" || data == "range_error" || data == "overflow_error" || data == "underflow_error" || data == "system_error" || data == "tx_exception" ||
                data == "bad_exception")
            {
                return $"{baseRoute}/error/{data}";
            }

            return "";
        }

        public string GenerateConceptsUrl()
        {
            if (data == "same_as" || data == "derived_from" || data == "convertible_to" || data == "common_reference_with" || data == "common_width" ||
                data == "integral" || data == "signed_integral" || data == "unsigned_integral" || data == "floating_point" || data == "assignable_from" ||
                data == "swappable" || data == "swappable_with" || data == "destructible" || data == "constructible_from" || data == "default_initializable" ||
                data == "move_constructible" || data == "copy_constructible" ||
                data == "boolean_testable" || data == "equality_comparable" || data == "equality_comparable_with" || data == "totally_ordered" || data == "totatlly_ordered_with" ||
                data == "movable" || data == "copyable" || data == "semiregular" || data == "regular" ||
                data == "invocable" || data == "regular_invocable" || data == "predicate" || data == "relation" || data == "equivalence_relation" || data == "strict_weak_order")
            {
                return $"{baseRoute}/concepts/{data}";
            }

            return "";
        }
    }
}
