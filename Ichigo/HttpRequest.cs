namespace Ichigo
{
    /// <summary>
    /// https://tools.ietf.org/html/rfc2616#section-5
    /// </summary>
    public class HttpRequest
    {
        public RequestLine RequestLine { get; set; }
        public static HttpRequest Parse(string request)
        {
            var httpRequest = new HttpRequest();
            var lines = request.Split("\r\n");
            for(int i = 0; i < lines.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        httpRequest.RequestLine = RequestLine.Parse(lines[i]);
                        break;
                }
            }
            return httpRequest;
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
