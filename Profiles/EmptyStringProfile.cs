using AutoMapper;

namespace ContosoPizza.Profiles;

public class EmptyStringProfile : Profile
{
    public EmptyStringProfile()
    {
        CreateMap<string, string>().ConvertUsing(s => s ?? string.Empty);
    }
}