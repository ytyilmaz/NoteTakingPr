using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using NoteTaking.Services.Note;

namespace NoteTaking.Services
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection service)
        {
            service.AddScoped<INoteService, NoteService>();
            service.AddSingleton(TypeAdapterConfig.GlobalSettings);
            service.AddScoped<IMapper, ServiceMapper>();
        }
    }
}
