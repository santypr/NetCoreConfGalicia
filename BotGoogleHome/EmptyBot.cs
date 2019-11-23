// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace BotGoogleHome
{
    public class EmptyBot : ActivityHandler
    {

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            base.OnTurnAsync(turnContext, cancellationToken);
            turnContext.SendActivityAsync(MessageFactory.Text($"Hola Commit Conf"), cancellationToken);
        }
    }
}
