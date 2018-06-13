using System;
using System.Collections.Generic;
using System.Text;

namespace Ichigo
{
    /// <summary>
    /// https://tools.ietf.org/html/rfc2616#section-6
    /// </summary>
    public class HttpResponse
    {
        public StatusLine StatusLine { get; set; }
        public static HttpResponse Success = new HttpResponse
        {
            StatusLine = StatusLine.Success,
        };

        public override string ToString()
        {
            return $"{StatusLine}";
        }
    }

    public class StatusLine
    {
        //HTTP-Version {space} Status-Code {space} Reason-Phrase {CRLF}
        public string HTTPVersion { get; set; }
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public static StatusLine Success = new StatusLine
        {
            HTTPVersion = HttpVersions.HTTP_1_1,
            StatusCode = StatusCodes.Ok,
            ReasonPhrase = "Success",
        };
        public override string ToString()
        {
            return $"{HTTPVersion} {StatusCode} {ReasonPhrase}";
        }
    }
}
