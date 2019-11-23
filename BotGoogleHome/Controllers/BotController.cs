// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EmptyBot v4.6.2

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bot.Builder.Community.Adapters.Google.Integration.AspNet.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;

namespace BotGoogleHome.Controllers
{
    // This ASP Controller is created to handle a request. Dependency Injection will provide the Adapter and IBot
    // implementation at runtime. Multiple different IBot implementations running at different endpoints can be
    // achieved by specifying a more specific type for the bot constructor argument.
    [Route("api/Conversation")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IGoogleHttpAdapter _adapter;
        private readonly IBot _bot;

        public BotController(IBot bot, IHttpContextAccessor contextAccessor)
        {
            try
            {
                _adapter = new GoogleHttpAdapter(contextAccessor)
                {
                    OnTurnError = async (context, exception) =>
                    {
                        await context.SendActivityAsync("Sorry, something went wrong");
                    },
                    ShouldEndSessionByDefault = true,
                };

                _bot = bot;
            }catch (Exception e)
            {
               
            }
        }

        [HttpPost]
        public async Task PostAsync()
        {
            await _adapter.ProcessAsync(Request, Response, _bot);
           
        }
    }
}
