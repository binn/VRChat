using System;
using System.Net.Http;

namespace VRChat.NET.Models
{
    public class VRChatResponse<T>
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }

        public int WafCode { get; set; }
        public string WafList { get; set; }
        
        public HttpResponseMessage RawResponse { get; set; }
        
        public bool Success => StatusCode >= 200 && StatusCode < 300;
        public bool Error => StatusCode >= 400 && StatusCode < 500;
        public bool Waf => WafCode > 0;

        public T Value { get; set; }
    }
}