using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ichigo
{
    /// <summary>
    /// https://tools.ietf.org/html/rfc2616#section-5
    /// </summary>
    public class HttpRequest
    {
        public RequestLine RequestLine { get; set; }
        public Headers Headers { get; set; } = new Headers();
        public static HttpRequest Parse(string request)
        {
            var httpRequest = new HttpRequest();
            var lines = request.Split("\r\n");
            for(int i = 0; i < lines.Length; i++)
            {
                if(i == 0)
                    httpRequest.RequestLine = RequestLine.Parse(lines[i]);
                if (lines[i].Contains(":"))
                {
                    var parts = lines[i].Split(":").Select(part => part.Trim()).ToArray();
                    httpRequest.Headers.AddOrUpdate(parts[0], parts[1]);
                }
            }
            return httpRequest;
        }
    }

    public class Headers
    {
        public Dictionary<string, string> _values = new Dictionary<string, string>();
        public string Host { get { return _values[HttpHeaders.Host]; } }
        public string Accept { get { return _values[HttpHeaders.Accept]; } }
        public string AcceptLanguage { get { return _values[HttpHeaders.AcceptLanguage]; } }
        public string AcceptEncoding { get { return _values[HttpHeaders.AcceptEncoding]; } }
        public string UserAgent { get { return _values[HttpHeaders.UserAgent]; } }
        public string this[string key]
        {
            get
            {
                return _values[key];
            }
        }

        public void AddOrUpdate(string key, string value)
        {
            _values[key] = value;
        }
    }

    public class RequestLine
    {
        //Method {space} Request-URI {space} HTTP-Version {CRLF}
        public string Method { get; set; }
        public string RequestURI { get; set; }
        public string HTTPVersion { get; set; }
        public static RequestLine Parse(string line)
        {
            var parts = line.Split(" ");
            return new RequestLine
            {
                Method = parts[0],
                RequestURI = parts[1],
                HTTPVersion = parts[2],
            };
        }

        public override string ToString()
        {
            return $"{Method} {RequestURI} {HTTPVersion}";
        }
    }
}
