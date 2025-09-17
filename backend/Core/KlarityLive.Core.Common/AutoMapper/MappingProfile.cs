using AutoMapper;
using KlarityLive.Core.Common.DTOs.Building;
using KlarityLive.Core.Common.DTOs.Tenant;
using KlarityLive.Core.Common.DTOs.User;
using KlarityLive.Domain.Core.Entities.Amin;
using KlarityLive.Domain.Core.Entities.BMS;
using System.Reflection;

namespace KlarityLive.Core.Common.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            RegisterMappings(this);

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Tenant, TenantDto>().ReverseMap();
            CreateMap<Building, BuildingDto>().ReverseMap();
        }

        private static void RegisterMappings(Profile profile)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            var entityTypes = types.Where(
                t => t.IsClass
                && !t.IsAbstract
                && t.Namespace != null
                && t.Namespace.StartsWith("KlarityLive.Domain.Core.Entities"));

            var dtoTypes = types.Where(
                t => t.IsClass
                && !t.IsAbstract
                && t.Namespace != null
                && t.Namespace.StartsWith("KlarityLive.Core.Common.DTOs")
                && t.Name.EndsWith("Dto"));

            foreach (var entityType in entityTypes)
            {
                var dtoType = dtoTypes.FirstOrDefault(t => t.Name.Replace("Dto", "") == entityType.Name);

                if (dtoType != null)
                {
                    profile.CreateMap(dtoType, entityType).ReverseMap();
                }
            }
        }
    }
}
