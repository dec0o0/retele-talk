using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IClient
    {
        [OperationContract(IsOneWay = true)]
        void OnReceive(string message, string user, bool mass);

        [OperationContract(IsOneWay = true)]
        void OnRegister(string user);

        [OperationContract]
        void OnUnregister(string user);

        [OperationContract]
        void ReceiveFromConf(int cod, string user, string message);
    }
}
