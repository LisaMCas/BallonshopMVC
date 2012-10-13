﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Rhino.ServiceBus.MessageModules;
using Rhino.ServiceBus.Internal;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Impl;

namespace BalloonShop.Server
{
    public class NHibernateMessageModule : IMessageModule, INHibernateSessionProvider
    {
        private readonly ISessionFactory sessionFactory;
        [ThreadStatic]
        private static ISession currentSession;

        public ISession CurrentSession
        {
            get { return currentSession ?? sessionFactory.OpenSession(); }
        }

        public NHibernateMessageModule(
          ISessionFactory sessionFactory)
        {

            this.sessionFactory = sessionFactory;
        }

        public void Init(ITransport transport,
          IServiceBus serviceBus)
        {

            transport.MessageArrived += TransportOnMessageArrived;
            transport.MessageProcessingCompleted
              += TransportOnMessageProcessingCompleted;
        }

        private static void
          TransportOnMessageProcessingCompleted(
          CurrentMessageInformation currentMessageInformation,
          Exception exception)
        {

            if (currentSession != null)
                currentSession.Dispose();
            currentSession = null;
        }

        private bool TransportOnMessageArrived(
          CurrentMessageInformation currentMessageInformation)
        {

            if (currentSession == null)
                currentSession = sessionFactory.OpenSession();
            return false;
        }


        public void Stop(ITransport transport, IServiceBus bus)
        {
            
        }
    }
}
