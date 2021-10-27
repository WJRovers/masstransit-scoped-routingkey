using MassTransit;

namespace MT.DI.Test.Provider
{
    public class ConsumeContextProvider : IContextProvider
    {
        private readonly ConsumeContext _context;

        public ConsumeContextProvider(ConsumeContext context)
        {
            _context = context;
        }

        public string GetHeader(string key = "foo")
        {
            return _context.Headers.Get<string>(key);
        }
    }
}