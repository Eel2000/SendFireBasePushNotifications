using Application.Services.Pushs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PushNotification.Commands
{
    public class SendPushCommand : IRequest<object>
    {
        public SendToParameter Parameter { get; set; }

        public SendPushCommand(SendToParameter parameter)
        {
            Parameter = parameter;
        }
    }

    public class SendPushCommandHandler : IRequestHandler<SendPushCommand, object>
    {
        private readonly IFcmPushService _pushService;
        public SendPushCommandHandler(IFcmPushService pushService)
        {
            _pushService = pushService;
        }

        public async Task<object> Handle(SendPushCommand request, CancellationToken cancellationToken)
        {
            var result = await _pushService.SendAsync(request.Parameter);
            return result;
        }
    }

}
