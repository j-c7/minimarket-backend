using Minimarket.DTO.User;
using Minimarket.Entity;
using Minimarket.DAL;

namespace Minimarket.BLL.User.Interfaces;

public interface IUserBLL
{
    Task<Result<ResponseUserDTO>> Create(UserProfileDTO entity);

    Task<Result<ResponseUserDTO>> Edit(EditUserProfileDTO entity);

    Task<Result<ResponseUserDTO>> Delete(int id);

    Task<Result<ResponseUserDTO>> GetUser(int id);

    Task<Result<List<ResponseUserDTO>>> UserList(string role, string seach);
}