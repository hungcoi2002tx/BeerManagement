using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Share.Models.Domain;
using Share.Models.Dtos.AddDtos;
using Share.Models.Dtos.ViewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Share.Ultils
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            #region Auto Map Config
            var assembly = Assembly.GetAssembly(typeof(MapperConfig));
            var classes = assembly.ExportedTypes.Where(x => x.Namespace.Equals("Share.Models.Domain"));

            foreach (var type in classes)
            {
                CreateMap(type, type).ReverseMap();

                var editDtoClass = assembly.ExportedTypes.FirstOrDefault(x => x.Name == type.Name + "EditDto");
                if (editDtoClass != null)
                {
                    CreateMap(type, editDtoClass).ReverseMap();
                }

                var viewDtoClass = assembly.ExportedTypes.FirstOrDefault(x => x.Name == type.Name + "ViewDto");
                if (viewDtoClass != null)
                {
                    CreateMap(type, viewDtoClass).ReverseMap();
                }

                var addDtoClass = assembly.ExportedTypes.FirstOrDefault(x => x.Name == type.Name + "AddDto");
                if (addDtoClass != null)
                {
                    CreateMap(type, addDtoClass).ReverseMap();
                }
            }
            #endregion

            #region Custome Map
            CreateMap<Product, ImportHistory>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ReverseMap();

            CreateMap<ProductViewDto, OrderDetailViewDto>()
              .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
              .ReverseMap();

            CreateMap<OrderDetailViewDto, OrderDetailAddDto>()
             .ReverseMap();
            #endregion
        }
    }
}
