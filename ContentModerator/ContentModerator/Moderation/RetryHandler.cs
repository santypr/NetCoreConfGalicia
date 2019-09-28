using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ContentModerator.Moderation
{
    public class RetryHandler : DelegatingHandler
    {
        public int MaxRetries { get; set; }

        public RetryHandler(int maxRetries)
        {
            this.MaxRetries = maxRetries;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < MaxRetries; i++)
            {
                response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    Thread.Sleep((int)response.Headers.RetryAfter.Delta.Value.TotalMilliseconds);
                }
            }

            return response;
        }
    }
}
