namespace ExampleClient
{
    interface IMessenger<T>
    {
        void Send(T msg);
    }
}
