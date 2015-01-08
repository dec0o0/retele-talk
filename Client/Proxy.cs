using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Common;


namespace Client
{
    internal class Proxy:DuplexClientBase<IService>, IService
    {

        public void Register(string user)
        {
            Channel.Register(user);
        }

        public void Unregister()
        {
            Channel.Unregister();
        }

        public void Send(string message, string user, bool mass)
        {
            Channel.Send(message, user, mass);
        }

        public int AddConf(List<string> l)
        {
            return Channel.AddConf(l);
        }

        public void StergeConf(int cod)
        {
            Channel.StergeConf(cod);
        }

        public void SendToConf(int cod, string message)
        {
            Channel.SendToConf(cod, message);
        }

        public string RequstUsersOfConf(int cod)
        {
            return Channel.RequstUsersOfConf(cod);
        }

        public Proxy(IClient client)
            : base(new InstanceContext(client))
        {
        }

    }
}
