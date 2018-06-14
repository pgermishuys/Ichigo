using Machine.Specifications;
namespace Ichigo.Tests.HttpRequest
{
    [Subject("HttpRequest")]
    public class when_parsing_a_simple_get_request
    {
        private static Ichigo.HttpRequest _httpRequest;
        private static string getRequest =
      @"GET /docs/index.html HTTP/1.1
        Host: www.nowhere123.com
        Accept: image/gif, image/jpeg, */*
        Accept-Language: en-us
        Accept-Encoding: gzip, deflate
        User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)
        ";
        Because of = () => {
            _httpRequest = Ichigo.HttpRequest.Parse(getRequest);
        };
        It should_have_the_correct_method = () => {
            _httpRequest.RequestLine.Method.ShouldEqual(HttpMethods.Get);
        };
        It should_have_the_correct_http_version = () => {
            _httpRequest.RequestLine.HTTPVersion.ShouldEqual(HttpVersions.HTTP_1_1);
        };
        It should_have_the_correct_request_uri = () => {
            _httpRequest.RequestLine.RequestURI.ShouldEqual("/docs/index.html");
        };
        It should_have_the_correct_host = () => {
            _httpRequest.Headers.Host.ShouldEqual("www.nowhere123.com");
        };
        It should_have_the_correct_accept_header = () => {
            _httpRequest.Headers.Accept.ShouldEqual("image/gif, image/jpeg, */*");
        };
        It should_have_the_correct_accept_language_header = () => {
            _httpRequest.Headers.AcceptLanguage.ShouldEqual("en-us");
        };
        It should_have_the_correct_accept_encoding_header = () => {
            _httpRequest.Headers.AcceptEncoding.ShouldEqual("gzip, deflate");
        };
        It should_have_the_correct_user_agent_header = () => {
            _httpRequest.Headers.UserAgent.ShouldEqual("Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
        };
    }
}
