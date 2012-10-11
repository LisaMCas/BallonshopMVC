﻿using Rhino.ServiceBus.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalloonShop.Messages
{
    public class InitialNotificationMessage : ISagaMessage
    {
        public int OrderId { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
