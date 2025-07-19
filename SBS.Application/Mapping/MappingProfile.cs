using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SBS.Application.DTOs.BookingDto;
using SBS.Application.DTOs.ResourceDto;
using SBS.Domain.Entities;

namespace SBS.Application.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Booking, BookingDto>();
			CreateMap<BookingRequestDto, Booking>();
			CreateMap<Resource, ResourceDto>()
				.ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type != null ? src.Type.Name : string.Empty)).ReverseMap();
			CreateMap<CreateResourceDto, Resource>().ReverseMap();
			CreateMap<UpdateResourceDto, Resource>().ReverseMap();
		}
	}
}
