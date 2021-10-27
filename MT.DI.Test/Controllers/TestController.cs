using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MT.DI.Test.Messages;

namespace MT.DI.Test.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly IRequestClient<PingRequest> _client;
        private readonly IPublishEndpoint _endpoint;

        public TestController(
            IRequestClient<PingRequest> client,
            IPublishEndpoint endpoint
        )
        {
            _client = client;
            _endpoint = endpoint;
        }

        [HttpPost("http")]
        public async Task<string> ViaHttp()
        {
            var response = await _client.GetResponse<PongResponse>(new
            {
                Message = "Send via HTTP"
            });

            return response.Message.Message;
        }

        [HttpPost("message")]
        public async Task<string> ViaMessages()
        {
            await _endpoint.Publish<StartPingRequest>(new
            {
                Message = "Send via Message"
            });

            return "Published";
        }
    }
}