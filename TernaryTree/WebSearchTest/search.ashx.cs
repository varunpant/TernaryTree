using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace WebSearchTest
{
    /// <summary>
    /// Summary description for search
    /// </summary>
    public class search : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;

            try
            {
                var Query = context.Request.QueryString.Get("key");
                var bounds = context.Request.QueryString.Get("bounds");
                var loc = context.Request.QueryString.Get("LOC");



                var words = Global.GetWords(Query.Trim().ToLower());
                IEnumerable<int> results = null;
                foreach (var w in words)
                {
                    if (results != null)
                    {
                        results = System.Linq.Enumerable.Intersect(results, Global.Index.Search(w));
                    }
                    else
                    {
                        results = Global.Index.Search(w);
                    }
                }


                List<string> res = new List<string>();

                foreach (var r in results)
                {
                    res.Add(Utf8ToUtf16(Global.Data[r].Trim()));
                }
                string json = oSerializer.Serialize(res);
                context.Response.Write(json);

            }
            catch
            {
                context.Response.Write("{Error:true}");
                context.Response.StatusCode = 404;
            }


        }
        public static string Utf8ToUtf16(string utf8String)
        {
            // Get UTF8 bytes by reading each byte with ANSI encoding
            byte[] utf8Bytes = Encoding.Default.GetBytes(utf8String);

            // Convert UTF8 bytes to UTF16 bytes
            byte[] utf16Bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);

            // Return UTF16 bytes as UTF16 string
            return Encoding.Unicode.GetString(utf16Bytes);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}