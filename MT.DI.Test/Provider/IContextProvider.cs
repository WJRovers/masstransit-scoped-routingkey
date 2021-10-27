namespace MT.DI.Test.Provider
{
    public interface IContextProvider
    {
        string GetHeader(string key = "foo");
    }
}