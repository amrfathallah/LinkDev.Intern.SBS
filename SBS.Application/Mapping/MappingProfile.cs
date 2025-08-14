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


			CreateMap<Booking, ViewAllBookingDto>()
					 .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User!.FullName))
			.ForMember(dest => dest.ResourceName, opt => opt.MapFrom(src => src.Resource!.Name))
			.ForMember(dest => dest.ResourceType, opt => opt.MapFrom(src => src.Resource!.Type!.Name))
			.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status != null ? src.Status.Name : string.Empty))
			.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.BookingSlots.Min(bs => bs.Slot!.StartTime)))
			.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.BookingSlots.Max(bs => bs.Slot!.EndTime)));


			CreateMap<BookingStatus, BookingStatusDto>();
			CreateMap<ResourceType, ResourceTypeDto>();
			CreateMap<ApplicationUser, BookingsUsersDto>();
		}
	}
}
