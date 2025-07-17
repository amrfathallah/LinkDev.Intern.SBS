using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SBS.Application.DTOs;
using SBS.Domain.Entities;

namespace SBS.Application.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Booking, BookingDto>();
			CreateMap<BookingRequestDto, Booking>();
		}
	}
}
