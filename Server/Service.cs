using System.ServiceModel;
using System.Collections.Generic;
using Common;
using System.Text;
using System.Threading;

namespace Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class Service : IService
    {
        private static Dictionary<IClient, string> clients = new Dictionary<IClient, string>();
        //private static IList<IClient> clients = new List<IClient>();
        private IClient client;

        public void Register(string user)
        {
            if (client == null)
            {
                client = OperationContext.Current.GetCallbackChannel<IClient>();
                if (clients.Count != 0)
                {
                    lock (clients)
                    {
                        foreach (var c in clients)
                        {
                            c.Key.OnRegister(user);
                            client.OnRegister(c.Value);
                        }   
                    }
                }
                clients.Add(client, user);
            }
        }

        public void Unregister()
        {
            if (client != null)
            {
                lock (clients)
                {
                    foreach(var c in clients)
                        if (c.Key != client)
                            c.Key.OnUnregister(clients[client]);
                }
                clients.Remove(client);
                client = null;
            }
        }

        public void Send(string message, string user, bool mass)
        {
            if (client != null)
            {
                lock (clients)
                {
                        foreach (var c in clients)
                        {
                            if (mass)
                                c.Key.OnReceive(message, clients[client], mass);
                            else
                                if (c.Value.ToLower().Equals(user.ToLower()))
                                    c.Key.OnReceive(message, clients[client], mass);
                        }
                }
            }
        }

        private static int codConf = 0;
        private static Dictionary<int, List<string>> conferinte = new Dictionary<int, List<string>>();

        public int AddConf(List<string> l){
            if (l != null)
            {
                codConf++;
                conferinte.Add(codConf, l);
                return codConf;
            }
            else
                return -1;
        }

        public void StergeConf(int cod)
        {
            if (conferinte.ContainsKey(cod))
                conferinte.Remove(cod);
        }

        public void SendToConf(int cod, string message)
        {
            if (client != null)
            {
                List<string> aux = conferinte[cod];
                lock (clients)
                {
                    foreach (var c in clients)
                    {
                        if (aux.Contains(c.Value) && c.Key != client)
                            c.Key.ReceiveFromConf(cod, clients[client], message);
                    }
                }
            }
        }

        public string RequstUsersOfConf(int cod)
        {
            StringBuilder sb = new StringBuilder();
            if (conferinte.ContainsKey(cod))
            {
                foreach (var c in conferinte[cod])
                    sb.Append(c + " ");
            }
            return sb.ToString();
        }
    }
}
