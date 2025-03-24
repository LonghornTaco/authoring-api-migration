namespace AuthoringApi.NetCoreApp.Communication
{
    public interface IClientLogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Debug(string message);
    }
}
