using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Minimarket.BLL.User.Interfaces;
using Minimarket.DAL.Repository.Interfaces;
using Minimarket.DTO.User;
using Minimarket.Entity;

namespace Minimarket.BLL.User;

public class UserBLL(IGenericRepo<UserProfile> repo, IMapper mapper, IConfiguration config) : IUserBLL
{
    public async Task<Result<ResponseUserDTO>> Create(UserProfileDTO entity)
    {
        // TODO: Añadir comprobación de formato para correo electrónico.

        // Comprobamos que el rol exista.
        if (!(entity.Role == UserRoles.Admin || entity.Role == UserRoles.Employee))
        {
            return Result<ResponseUserDTO>.Failure(
                [$"El rol que ingreso no existe, los roles disponibles son {UserRoles.Admin} y {UserRoles.Employee}"]
            );
        }

        // Comprobamos si el correo electrónico existe.
        var checkEmail = repo.Query(usr => usr.Email == entity.Email).Any();
        if (checkEmail)
        {
            return Result<ResponseUserDTO>.Failure(["El correo electronico ya existe"]);
        }

        // Creamos el nuevo modelo que introduciremos en la base de datos.
        var neoEntity = mapper.Map<UserProfile>(entity);

        // Encriptamos la contraseña.
        neoEntity.Password = HashPassword(entity.Password!);

        var createdModel = await repo.Create(neoEntity);
        if (!createdModel.IsSucess)
            return Result<ResponseUserDTO>.Failure(createdModel.Errors);

        return Result<ResponseUserDTO>.Success(mapper.Map<ResponseUserDTO>(createdModel.Value));
    }

    private static string HashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password, BCrypt.Net.HashType.SHA384, 11);

    public async Task<Result<ResponseUserDTO>> Delete(int id)
    {
        // Comprobamos que el Id del usuario existe.
        var entity = await repo.Query(usr => usr.Id == id).FirstOrDefaultAsync();
        if (entity == null)
        {
            return Result<ResponseUserDTO>.Failure(["Usuario no encontrado"]);
        }

        var deletedEntity = await repo.Delete(entity);
        if (!deletedEntity.IsSucess)
            return Result<ResponseUserDTO>.Failure(deletedEntity.Errors);

        return Result<ResponseUserDTO>.Success(mapper.Map<ResponseUserDTO>(deletedEntity.Value));
    }

    public async Task<Result<ResponseUserDTO>> Edit(EditUserProfileDTO entity)
    {
        // Comprobamos que el Id del usuario existe.
        var dbEntity = await repo.Query(usr => usr.Id == entity.Id).FirstOrDefaultAsync();
        if (dbEntity != null)
        {
            if (!entity.Name.IsNullOrEmpty())
                dbEntity.Name = entity.Name;

            if (!entity.Email.IsNullOrEmpty())
                dbEntity.Email = entity.Email;

            if (!entity.Password.IsNullOrEmpty())
            {
                var existPassword = BCrypt.Net.BCrypt.EnhancedVerify(entity.Password, dbEntity!.Password, BCrypt.Net.HashType.SHA384);

                dbEntity.Password = entity.Password != "" && !existPassword ?
                     dbEntity.Password = HashPassword(entity.Password!) : dbEntity.Password;
            }

            var editedEntity = await repo.Edit(dbEntity);
            if (!editedEntity.IsSucess)
                return Result<ResponseUserDTO>.Failure(editedEntity.Errors);

            return Result<ResponseUserDTO>.Success(mapper.Map<ResponseUserDTO>(editedEntity.Value));
        }
        return Result<ResponseUserDTO>.Failure(["Usuario no encontrado"]);
    }

    public async Task<Result<ResponseUserDTO>> GetUser(int id)
    {
        var user = await repo.Query(usr => usr.Id == id).FirstOrDefaultAsync();
        return user != null ? Result<ResponseUserDTO>.Success(mapper.Map<ResponseUserDTO>(user))
            : Result<ResponseUserDTO>.Failure(["Usuario no encontrado"]);
    }

    public async Task<Result<List<ResponseUserDTO>>> UserList(string role, string seach)
    {
        seach = seach.ToLower();
        role = role.Trim().ToLower();

        if(role == "na" || role == "all") role = "";
        if(seach == "na" || seach == "all") seach = "";
        
        async Task<List<UserProfile>> GetEntities(bool usrRole)
        {
            return await repo.Query(usr => (usrRole != true || (usr.Role == role)) 
                && string
                .Concat(usr.Name!
                .ToLower(), usr.Email!
                .ToLower())
                .Contains(seach.ToLower())
            ).ToListAsync();
        }

        var entities = role != "" ? await GetEntities(true) : await GetEntities(false);
        if (entities.IsNullOrEmpty())
        {
           return Result<List<ResponseUserDTO>>.Failure(["Sin resultados"]);
        }

        List<ResponseUserDTO> resList = mapper.Map<List<ResponseUserDTO>>(entities);
        return Result<List<ResponseUserDTO>>.Success(resList);
    }
}