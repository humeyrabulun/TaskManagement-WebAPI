namespace TodoApi.Services
{
   
        public interface ITransientService { Guid GetGuid(); }
        public interface IScopedService { Guid GetGuid(); }
        public interface ISingletonService { Guid GetGuid(); }

        public class GuidService : ITransientService, IScopedService, ISingletonService
        {
            private readonly Guid _id;

            public GuidService()
            {
                _id = Guid.NewGuid();
            }
            
            public Guid GetGuid()
            {
                return _id;
            }
        }


    }

