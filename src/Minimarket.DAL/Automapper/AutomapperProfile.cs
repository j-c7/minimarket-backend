using Minimarket.Entity;
using AutoMapper;
using Minimarket.DTO.User;
using Minimarket.DTO.Product;
using Minimarket.DTO.Sale;
using System.Globalization;

namespace Minimarket.DAL.Automapper;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        // Usuario
        CreateMap<UserProfile, UserProfileDTO>();
        CreateMap<UserProfileDTO, UserProfile>();

        CreateMap<ResponseUserDTO, UserProfile>()
            .ForMember(d => d.Id, opt => opt.Ignore());

        CreateMap<UserProfile, ResponseUserDTO>();

        // Categoria
        CreateMap<ProductCategory, ProductCategoryDTO>();
        CreateMap<ProductCategoryDTO, ProductCategory>();

        CreateMap<ProductCategory, EditProductCategoryDTO>();
        CreateMap<EditProductCategoryDTO, ProductCategory>();

        CreateMap<ResProductCategoryDTO, ProductCategory>();
        CreateMap<ProductCategory, ResProductCategoryDTO>();

        // Producto
        CreateMap<Product, ProductDTO>();
        CreateMap<ProductDTO, Product>();

        CreateMap<Product, EditProductDTO>();
        CreateMap<EditProductDTO, Product>();

        CreateMap<Product, ResProductDTO>();
        CreateMap<ProductCategoryDTO, Product>();

        // Sale
        CreateMap<Sale, SaleDTO>()
        .ForMember(d => d.TotalText,
            opt =>
            opt.MapFrom(origin =>
                Convert.ToString(origin.Total!.Value, CultureInfo.InvariantCulture)
            )
        );

        CreateMap<SaleDTO, Sale>()
        .ForMember(d => d.Total,
            opt => opt.MapFrom(origin =>
                Convert.ToDecimal(origin.TotalText, CultureInfo.InvariantCulture)
            )
        );

        // Sale Detail
        CreateMap<SaleDetail, SaleDetailDTO>()
        .ForMember(d => d.IdProductDescription,
            opt => opt.MapFrom(origin => origin.IdProductNavigation!.Name)
        )
        .ForMember(d => d.PriceText,
            opt => opt.MapFrom(
                origin => Convert.ToString(origin.Price!.Value, CultureInfo.InvariantCulture))
        )
        .ForMember(d => d.TotalText,
            opt => opt.MapFrom(origin =>
                Convert.ToString(origin.Total!.Value, CultureInfo.InvariantCulture))
        );

        CreateMap<SaleDetailDTO, SaleDetail>()
        .ForMember(
            d => d.Price, opt => opt.MapFrom(origin =>
                Convert.ToDecimal(origin.PriceText, CultureInfo.InvariantCulture))
        )
        .ForMember(
            d => d.Total, opt => opt.MapFrom(origin =>
                Convert.ToDecimal(origin.TotalText, CultureInfo.InvariantCulture))
        );
    }
}