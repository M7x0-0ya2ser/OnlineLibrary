using AutoMapper;
using OnlineLibrary.Models;

namespace OnlineLibrary.DTOs
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<BorrowedBook,BorrowedBookDto>();
            CreateMap<BorrowedBookDto,BorrowedBook>();
            CreateMap<ISBNdto, BorrowedBook>();
            CreateMap<UserRegisterDto, User>().ReverseMap();
        }
    }
}
