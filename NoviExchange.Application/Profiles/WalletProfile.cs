using AutoMapper;
using NoviExchange.Domain.Entities;
using NoviExchange.Domain.Models;

namespace NoviExchange.Application.Profiles
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            // Entity → Domain
            CreateMap<WalletEntity, Wallet>();

            // Domain → Entity
            CreateMap<Wallet, WalletEntity>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
