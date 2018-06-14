namespace Ichigo
{
    /// <summary>
    /// https://tools.ietf.org/html/rfc2616#section-5.1.1
    /// </summary>
    public class HttpMethods
    {
        /// <summary>
        /// https://tools.ietf.org/html/rfc2616#section-9.2
        /// </summary>
        public const string Options = "OPTIONS";
        /// <summary>
        /// https://tools.ietf.org/html/rfc2616#section-9.3
        /// </summary>
        public const string Get = "GET";
        /// <summary>
        /// https://tools.ietf.org/html/rfc2616#section-9.4
        /// </summary>
        public const string Head = "HEAD";
        /// <summary>
        /// https://tools.ietf.org/html/rfc2616#section-9.5
        /// </summary>
        public const string Post = "POST";
        /// <summary>
        /// https://tools.ietf.org/html/rfc2616#section-9.6
        /// </summary>
        public const string Put = "PUT";
        /// <summary>
        /// https://tools.ietf.org/html/rfc2616#section-9.7
        /// </summary>
        public const string Delete = "DELETE";
        /// <summary>
        /// https://tools.ietf.org/html/rfc2616#section-9.8
        /// </summary>
        public const string Trace = "TRACE";
        /// <summary>
        /// https://tools.ietf.org/html/rfc2616#section-9.9
        /// </summary>
        public const string Connect = "CONNECT";

        public static readonly string[] All = new[]
        {
            Options,
            Get,
            Head,
            Post,
            Put,
            Delete,
            Trace,
            Connect,
        };
    }
}
