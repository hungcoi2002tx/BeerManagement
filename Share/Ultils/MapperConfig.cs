using AutoMapper;
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
                var editModelClass = assembly.ExportedTypes.FirstOrDefault(x => x.Name == type.Name + "EditModel");
                if (editModelClass != null)
                {
                    CreateMap(type, editModelClass).ReverseMap();
                }
                var viewModelClass = assembly.ExportedTypes.FirstOrDefault(x => x.Name == type.Name + "ViewModel");
                if (viewModelClass != null)
                {
                    CreateMap(type, viewModelClass).ReverseMap();
                }
            }
            #endregion

            #region Custome Map
            #endregion
        }
    }
}
