using System.Collections.Generic;
using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Book, BookResponse>()
                .ForMember(x => x.Descr, opt => opt.MapFrom(src => src.ShortDescr));

            CreateMap<BookResponse, Book>()
                .ForMember(x => x.ShortDescr, opt => opt.MapFrom(src => src.Descr));

            CreateMap<SearchResponse<Book>, SearchResponse<BookResponse>>()
                .ForMember(x => x.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}