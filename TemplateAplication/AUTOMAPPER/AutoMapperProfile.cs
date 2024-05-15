namespace $safeprojectname$.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ExampleDto, TExample>().ReverseMap();
        }
    }
}
