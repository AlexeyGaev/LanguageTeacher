using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;

namespace Translation.Http {
    public static class Converter {
        static string IsNull = "is Null";
        static string IsEmpty = "is Empty";
        static string Dot = ".";
        static string NewLine = "\n";
        static string Colon = ":";
        static string Indent = " ";
        
        public static string Convert(string indent, HttpContext value) {
            return ConvertObject(indent, "HttpContext", value, (i, v) => ConvertCore(i, v));
        }
        public static string Convert(string indent, IFeatureCollection value) {
            return ConvertObject(indent, "IFeatureCollection", value, (i,v) => ConvertCore(i, v));
        }
        public static string Convert(string indent, HttpRequest value) {
            return ConvertObject(indent, "HttpRequest", value, (i, v) => ConvertCore(i, v));
        }
        public static string Convert(string indent, HttpResponse value) {
            return ConvertObject(indent, "HttpResponse", value, (i, v) => ConvertCore(i, v));
        }
        
        static string ConvertCore(string indent, HttpContext value) {
            //public abstract ConnectionInfo Connection { get; }
            //public abstract WebSocketManager WebSockets { get; }
            //public abstract ClaimsPrincipal User { get; set; }
            //public abstract IServiceProvider RequestServices { get; set; }
            //public abstract CancellationToken RequestAborted { get; set; }
            //public abstract ISession Session { get; set; }
            return
                Convert(indent, value.Features) +
                Convert(indent, value.Request) +
                Convert(indent, value.Response) +
                Convert(indent, "Items (IDictionary<object, object>)", "(object)", "(object)", value.Items, t => Convert(t), v => Convert(v)) +
                Convert(indent, "TraceIdentifier", value.TraceIdentifier);
        }

        static string ConvertCore(string indent, HttpRequest value) {
            //public abstract HttpContext HttpContext { get; }
            
            //public abstract QueryString QueryString { get; set; }
            //public abstract Stream Body { get; set; }
            //public abstract IRequestCookieCollection Cookies { get; set; }
            //public abstract IHeaderDictionary Headers { get; }
            //public abstract IQueryCollection Query { get; set; }
            //public abstract IFormCollection Form { get; set; }
            //public abstract PathString Path { get; set; }
            //public abstract PathString PathBase { get; set; }
            //public abstract HostString Host { get; set; }
            return
                Convert(indent, "ContentType", value.ContentType) +
                Convert(indent, "ContentLength", value.ContentLength) +
                Convert(indent, "Protocol", value.Protocol) +
                Convert(indent, "IsHttps", value.IsHttps) +
                Convert(indent, "Scheme", value.Scheme) +
                Convert(indent, "Method", value.Method) +
                Convert(indent, "HasFormContentType", value.HasFormContentType);
        }

        static string ConvertCore(string indent, HttpResponse value) {
            //public abstract HttpContext HttpContext { get; }
           
            //public abstract IHeaderDictionary Headers { get; }
            //public abstract Stream Body { get; set; }
            //public abstract IResponseCookies Cookies { get; }
            return
                Convert(indent, "StatusCode", value.StatusCode) +
                Convert(indent, "ContentLength", value.ContentLength) +
                Convert(indent, "ContentType", value.ContentType) +
                Convert(indent, "HasStarted", value.HasStarted);
        }

        static string ConvertCore(string indent, IFeatureCollection value) {
            return
                Convert(indent, "IsReadOnly", value.IsReadOnly) +
                Convert(indent, "Revision", value.Revision) +
                Convert(indent, "this (IEnumerable<KeyValuePair<Type, object>>)", "(Type)", "(object)", value, t => Convert(t), v => Convert(v));
        }

        static string Convert<K, V>(string indent, string prefix, string postfixKey, string postfixValue, IEnumerable<KeyValuePair<K, V>> pairs, Func<K, string> convertKey, Func<V, string> convertValue) 
            where K : class
            where V : class {

            if (pairs == null)
                return GetObjectNullConvert(indent + Indent, prefix);

            List<string> keys = new List<string>();
            List<string> values = new List<string>();
            foreach (KeyValuePair<K, V> pair in pairs) {
                keys.Add("Key" + postfixKey + Colon + " " + convertKey(pair.Key));
                values.Add("Value" + postfixValue + Colon + " " + convertValue(pair.Value));
            }

            if (keys.Count == 0)
                return GetEmptyConvert(indent + Indent, prefix);

            int maxKeyLenght = -1;
            foreach (string key in keys) {
                if (key.Length > maxKeyLenght)
                    maxKeyLenght = key.Length;
            }

            string result = indent + prefix + Colon + NewLine;
            for (int i = 0; i < keys.Count; i++) 
                result += indent + Indent + keys[i].PadRight(maxKeyLenght) + " | " + values[i] + NewLine;

            return result;
        }

        static string ConvertObject<T>(string indent, string prefix, T value, Func<string, T, string> convertCore) where T : class {
            return value != null ? GetObjectDetailConvert(indent, prefix, value, v => convertCore(indent + Indent, v)) : GetObjectNullConvert(indent + Indent, prefix);
        }
        static string GetObjectNullConvert(string indent, string prefix) {
            return GetInValidFullPrefix(indent, prefix) + IsNull + Dot + NewLine;
        }
        static string GetEmptyConvert(string indent, string prefix) {
            return GetInValidFullPrefix(indent, prefix) + IsEmpty + Dot + NewLine;
        }
        static string GetObjectDetailConvert<T>(string indent, string prefix, T value, Func<T, string> convertCore) where T : class {
            return GetValidFullPrefix(indent, prefix) + Convert(value.GetType()) + NewLine + convertCore(value);
        }
        
        static string GetInValidFullPrefix(string indent, string prefix) {
            return indent + prefix + " ";
        }
        static string GetValidFullPrefix(string indent, string prefix) {
            return indent + prefix + Colon + " ";
        }
        static string Convert(object value) {
            return value.ToString();
        }
        static string Convert(Type value) {
            return value.FullName;
        }
        static string Convert(string indent, string prefix, string value) {
            if (value == null)
                return GetObjectNullConvert(indent, prefix + "(string)");
            if (String.IsNullOrEmpty(value))
                return GetEmptyConvert(indent, prefix + "(string)");
            return GetValidFullPrefix(indent, prefix) + value + NewLine;
        }
        static string Convert(string indent, string prefix, bool value) {
            return GetValidFullPrefix(indent, prefix) + value.ToString() + NewLine;
        }
        static string Convert(string indent, string prefix, int value) {
            return GetValidFullPrefix(indent, prefix) + value.ToString() + NewLine;
        }
        static string Convert(string indent, string prefix, long? value) {
            return value.HasValue ? GetValidFullPrefix(indent, prefix) + value.Value + NewLine : GetObjectNullConvert(indent, prefix + "(long)");
        }
    }
}