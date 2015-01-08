using System.ServiceModel;
using System;
using System.Collections.Generic;

namespace Common
{
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IService
    {
        [OperationContract(IsOneWay = true)]
        void Register(string user);

        [OperationContract(IsOneWay = true)]
        void Unregister();

        [OperationContract(IsOneWay = true)]
        void Send(string message, string user, bool mass);

        [OperationContract(IsOneWay = false)]
        int AddConf(List<string> l);

        [OperationContract(IsOneWay = true)]
        void StergeConf(int cod);

        [OperationContract(IsOneWay = true)]
        void SendToConf(int cod, string message);

        [OperationContract()]
        string RequstUsersOfConf(int cod);
    }
}
