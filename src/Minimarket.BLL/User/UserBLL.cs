using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        if (!(entity.Role == UserRoles.Admin || entity.Role == UserRoles.Employee))
        {
            return Result<ResponseUserDTO>.Failure(
                [$"El rol que ingreso no existe, los roles disponibles son {UserRoles.Admin} y {UserRoles.Employee}"]
            );
        }

        var checkEmail = repo.Query(usr => usr.Email == entity.Email).Any();
        if (checkEmail)
        {
            return Result<ResponseUserDTO>.Failure(["El correo electronico ya existe"]);
        }

        var neoEntity = mapper.Map<UserProfile>(entity);
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
        var dbEntity = await repo.Query(usr => usr.Id == entity.Id).FirstOrDefaultAsync();
        if (dbEntity != null)
        {
            if (!string.IsNullOrEmpty(entity.Name))
                dbEntity.Name = entity.Name;

            if (!string.IsNullOrEmpty(entity.Email))
                dbEntity.Email = entity.Email;

            if (!string.IsNullOrEmpty(entity.Password))
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

        if (role == "na" || role == "all") role = "";
        if (seach == "na" || seach == "all") seach = "";

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
        if (entities == null || entities.Count == 0)
        {
            return Result<List<ResponseUserDTO>>.Failure(["Sin resultados"]);
        }
        List<ResponseUserDTO> resList = mapper.Map<List<ResponseUserDTO>>(entities);
        return Result<List<ResponseUserDTO>>.Success(resList);
    }

    // TODO: Añadir refresh token para mayor seguridad.
    public async Task<Result<SessionDTO>> Auth(LoginDTO entity)
    {
        var usrDB = await repo.Query(u => u.Email == entity.Email).FirstOrDefaultAsync();
        if (usrDB == null)
        {
            return Result<SessionDTO>.Failure(["Correo electronico no registrado"]);
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(entity.Password, usrDB!.Password, BCrypt.Net.HashType.SHA384))
        {
            return Result<SessionDTO>.Failure(["Contraseña incorrecta"]);
        }

        var jwt = config.GetSection("JWT").GetSection("Key").Get<string>();
        var now = DateTime.UtcNow;
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, entity.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(now).ToString(), ClaimValueTypes.Integer64),
            new Claim("Id", usrDB.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt!));
        var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);
        var token = new JwtSecurityToken(
            null,
            null,
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: singIn
        );

        return Result<SessionDTO>.Success(new SessionDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Profile = mapper.Map<ResponseUserDTO>(usrDB)
        });
    }

    private static int GetUserByClaims(HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        var id = identity!.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!;
        return int.Parse(id);
    }

    public async Task<Result<ResponseUserDTO>> Profile(HttpContext context)
    {
        int id = GetUserByClaims(context);
        var usr = await repo.Query(u => u.Id == id).FirstOrDefaultAsync();

        if (usr == null)
            return Result<ResponseUserDTO>.Failure(["Id de usuario no encontrado"]);

        return Result<ResponseUserDTO>.Success(mapper.Map<ResponseUserDTO>(usr));
    }
}