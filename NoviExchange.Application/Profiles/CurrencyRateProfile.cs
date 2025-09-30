using AutoMapper;
using NoviExchange.Domain.Entities;
using NoviExchange.Domain.Models;

namespace NoviExchange.Application.Profiles
{
    public class CurrencyRateProfile : Profile
    {
        public CurrencyRateProfile()
        {
            CreateMap<CurrencyRateEntity, CurrencyRate>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.ToCurrency));

            CreateMap<CurrencyRate, CurrencyRateEntity>()
                .ForMember(dest => dest.ToCurrency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.FromCurrency, opt => opt.MapFrom(_ => "EUR"))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
