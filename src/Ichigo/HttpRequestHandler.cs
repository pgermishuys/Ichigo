using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo
{
    public class HttpRequestHandler
    {
        public Task<HttpResponse> HandleRequest(HttpRequest request)
        {
            return Task.FromResult(HttpResponse.Success);
        }
    }
}
