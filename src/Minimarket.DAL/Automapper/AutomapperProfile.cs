using Minimarket.Entity;
using AutoMapper;
using Minimarket.DTO.User;
using Minimarket.DTO.Product;

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
    }

}