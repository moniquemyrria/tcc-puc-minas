using Kodigos.API.ModelView.ServicesUser;
using Kodigos.Dominio.ModelsData.Administration.Users;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using NetDevPack.Security.Jwt.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Dapper;
using System.Globalization;

namespace Kodigos.API.Controllers.Services.Register
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceUserController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ServiceUserController> _logger;
        private readonly UserManager<UsersDTO> _userManager;
        private readonly IJwtService _jsonWebKeySetService;
        public ServiceUserController(KodigosContext context, IConfiguration configuration, ILogger<ServiceUserController> logger, UserManager<UsersDTO> userManager, IJwtService jsonWebKeySetService)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _userManager = userManager;
            _jsonWebKeySetService = jsonWebKeySetService;
        }


        /// <summary>
        /// Serviço - Lista de usuários 
        /// </summary>
        [HttpGet]
        public async Task<KRetorno<List<UserModel?>>> GetUserList()
        {

            var kResult = new KRetorno<List<UserModel>>();

            try
            {
                var users = _context.Users.ToList();

                var listUsers = new List<UserModel>();

                foreach (var item in users)
                {
                    listUsers.Add(
                           new UserModel
                           {
                               Id = item.Id,
                               UserName = item.UserName,
                               Email = item.Email,
                               Name = item.name,
                               IsActive = item.isActive,
                               lastAccessDate = item.lastAccessDate != null ? item.lastAccessDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) :  "",
                           }) ;

                };

                kResult.Sucesso = true;
                kResult.Message = "Dados dos usuários retornados com sucesso!";
                kResult.TRetorno = listUsers;

            }
            catch (Exception e)
            {
                kResult.Sucesso = false;
                kResult.Message = "Não foi possível retornar a lista de usuários. Erro: " + e.Message;
            }
            return kResult;

        }

        /// <summary>
        /// Serviço - Get Id do Usuario
        /// </summary>
        [HttpGet("{id}")]
        public async Task<KRetorno<UsersDTO>> GetUserId(string id)
        {

            var result = new KRetorno<UsersDTO>();

            try
            {
                if (UsersIdExists(id))
                {
                    UsersDTO user = _context.Users.FirstOrDefault(t => t.Id == id);
                    user.PasswordHash = "";
                    user.SecurityStamp = "";
                    user.ConcurrencyStamp = "";

                    result.Sucesso = true;
                    result.TRetorno = user;
                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Usuário não localizado.";
                }


            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados do Usuário selecionado. Erro: " + e.Message;
            }

            return result;
        }



        /// <summary>
        /// Serviço - Login de Usuário
        /// </summary>
        [HttpPost]
        [Route("login")]
        public async Task<KRetorno<dynamic>> Login(LoginModel loginModel)
        {

            var user = await _userManager.FindByNameAsync(loginModel.user);
            var result = new KRetorno<dynamic>();

            if (user != null && !user.isActive && user.lastAccessDate == null)
            {

                result.Sucesso = false;
                result.Message = "Este usuário está bloqueado. Entre em contato com o administrador do sistema.";

                return result;
            }


            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.pass))
            {

                if (user.lastAccessDate != null)
                {
                    DateTime lastAccessDate = (DateTime)(user.lastAccessDate != null ? user.lastAccessDate : DateTime.Now.Date);
                    TimeSpan daysLastAccess = DateTime.Now.Subtract(lastAccessDate);
                    int qtyDaysLastAccess = daysLastAccess.Days;

                    if (qtyDaysLastAccess <= 100)
                    {
                        user.lastAccessDate = DateTime.Now.Date;
                        _context.Update(user);
                        _context.SaveChanges();
                    }
                    else
                    {
                        user.isActive = false;
                        _context.Update(user);
                        _context.SaveChanges();

                        result.Sucesso = false;
                        result.Message = "Usuário bloqueado, último acesso realizado há " + qtyDaysLastAccess + " dias. Entre em contato com o administrador do sistema.";//, ou através da opção de Ajuda (Como podemos ajudá-lo).";

                        return result;
                    }


                }
                else
                {
                    user.lastAccessDate = DateTime.Now.Date;
                    _context.Update(user);
                    _context.SaveChanges();
                }
                
               
                
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = await GetToken(authClaims);

                var userData = new UserModel
                {
                    Id = user.Id,
                    Name = user.name,
                    Email = user.Email,
                    IsActive = user.isActive,
                    UserName = user.UserName,
                    language = "P" // P - PORTUGUESE / E - ENGLISH / S - SPANISH

                };

                return KRetorno<dynamic>.Success(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = userData
                });


            }

            result.Sucesso = false;
            result.Message = "Usuário ou senha informados, são inválidos, ou o usuário não está cadastrado.";

            return result;//KRetorno<dynamic>.Failed(new KError { Code = "999", Description = "Usuário ou senha informados, são inválidos, ou o usuário não está cadastrado." });
        }


        /// <summary>
        /// Serviço - Nova senha
        /// </summary>
        //[Authorize]
        [HttpPut]
        [Route("newpass")]

        public async Task<KRetorno> NewPass(UserNewPassModel userNewPass)
        {
            var kResult = new KRetorno();
            try
            {
                var usuario = await _userManager.FindByNameAsync(userNewPass.userName);

                if (usuario == null)
                {
                    throw new Exception("Usuário inválido!");
                }

                var result = await _userManager.ChangePasswordAsync(usuario, userNewPass.oldPass, userNewPass.newPass);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }


                await _userManager.UpdateAsync(usuario);


                kResult.Sucesso = true;
                kResult.Message = "Senha alterada com sucesso. Você será redirecionado para a Tela de Login.";

            }
            catch (Exception e)
            {
                kResult.Sucesso = false;
                kResult.Message = e.Message == "Incorrect password." ? "A Senha atual não confere, verifique os seus dados e tente novamente." : e.Message == "Passwords must be at least 8 characters." ? "A Nova senha informada deve conter no mínimo 8 caractéres. Uma sugestão de senha segura, prcure utilizar letras maiúsculas e/ou minúsculas, números e caractéres especiais." : "Não foi possível alterar a senha. Error: " + e.Message;
            }

            return kResult;

        }

        /// <summary>
        /// Serviço - Resetar senha
        ///</summary>
        //[Authorize]
        [HttpPost]
        [Route("refreshpass")]
        public async Task<KRetorno> RefreshPass(RefreshPassModel refreshPassModel)
        {
            var kResult = new KRetorno();
            try
            {
                //buscar 1 - por userName (Usuario de Login)
                var user = await _userManager.FindByNameAsync(refreshPassModel.userNameOrEmail.ToLower().Trim());

                if (user == null)
                {
                    //busca 2 - por email (Cadastro do usuario)
                    user = await _context.Users.FirstOrDefaultAsync(t => t.Email.ToLower().Trim() == refreshPassModel.userNameOrEmail.ToLower().Trim());

                    if (user == null)
                        throw new Exception("Usuário não encontrado!");
                }



                var tokenRefresh = await _userManager.GeneratePasswordResetTokenAsync(user);
                var pass = _userManager.GenerateNewAuthenticatorKey();


                var result = await _userManager.ResetPasswordAsync(user, tokenRefresh, pass);

                //send email

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                await _userManager.UpdateAsync(user);

                if (user != null && user.Email != null && user.Email.Trim() != "")
                {
                    var body = Utils.Resource.Resource.EmailSendCode_ForgetPass.Replace("{0}", user.name).Replace("{1}", user.UserName).Replace("{2}", pass);
                    var resultEmail = await Utils.Core.SendEmail.SendEmailText(_configuration, user.Email, "Acesso ao Controle de Orçamento", body, null, null);
                }


                kResult.Sucesso = true;
                kResult.Message = "Nova Senha gerada com sucesso e encaminhada para seu e-mail do cadastro '" + user.Email + "'.";


            }
            catch (Exception e)
            {
                kResult.Sucesso = false;
                kResult.Message = "Erro ao resetar a senha do usuário. " + e.Message;
            }

            return kResult;

        }


        /// <summary>
        /// Serviço - Editar dados Usuário
        /// </summary>
        [HttpPut("{id}")]
        public async Task<KRetorno<UsersDTO>> PutUser(string id, UsersDTO user)
        {

            var result = new KRetorno<UsersDTO>();

            try
            {
                if (UsersIdExists(id))
                {
                    var userUpdate = _context.Users.First(t => t.Id == id);

                    if (!userUpdate.isActive && user.isActive)
                    {
                        userUpdate.lastAccessDate = null;

                    }

                    userUpdate.isActive = user.isActive;
                    userUpdate.dateUpdate = user.dateUpdate;
                    userUpdate.name = user.name;
                    userUpdate.Email = user.Email;
                    userUpdate.NormalizedEmail = user.Email.ToUpper();

                    _context.Update(userUpdate);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = user;
                    result.Message = "Dados do Usuário alterado com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Nível do Catgo não localizado.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados do Usuário. Erro: " + e.Message;
            }

            return result;
        }


        /// <summary>
        /// Serviço - Registrar Usuário
        /// </summary>
        [HttpPost]
        [Route("register")]
        public async Task<KRetorno> Register(UsersDTO model)
        {

            var kResult = new KRetorno();
            var dbTransaction = _context.Database.BeginTransaction();

            try
            {
                var username = model.UserName.Trim();
                var userExists = await _userManager.FindByNameAsync(username);
                model.Id = Guid.NewGuid().ToString();
                if (userExists != null)
                {
                    kResult.Sucesso = true;
                    kResult.Message = "Este usuário já existe cadastrado na base de dados";
                }

                UserModel user = new()
                {
                    Email = model.Email,
                    UserName = username,
                    Id = model.Id,
                };


                var pass = model.passwordRegister;//_userManager.GenerateNewAuthenticatorKey();

                var result = await _userManager.CreateAsync(model, pass);
                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);

                dbTransaction.Commit();

                kResult.Sucesso = true;
                kResult.Message = "Seja bem-vindo(a), seu cadastro foi realizado com sucesso. Obrigada por escolher o Payment On Time :)";
            }
            catch (Exception e)
            {
                dbTransaction.Rollback();
                kResult.Sucesso = false;

                kResult.Message = e.Message == "Passwords must be at least 8 characters." ? "A sua senha de acesso deve conter no mínimo 8 caractéres. Sugestão de senha segura: procure utilizar letras maiúsculas e/ou minúsculas, números e caractéres especiais." : e.Message == "Username '" + model.UserName + "' is already taken." ? "O login '" + model.UserName + "' já existe cadastrado na base de dados. Por favor informe outro login para este usuário e tente novamente" : "Erro ao registrar usuário." + e.Message;
            }

            return kResult;
        }

        /// <summary>
        /// Serviço - Token
        /// </summary>
        [HttpGet]
        [Route("getToken")]
        private async Task<JwtSecurityToken> GetToken(List<Claim> authClaims)
        {
            var key = await _jsonWebKeySetService.GetCurrentSigningCredentials();

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                authClaims,
                expires: DateTime.UtcNow.AddDays(15),
                signingCredentials: key);
            return token;
        }

        /// <summary>
        /// Ativa / Desativar Usuario
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<KRetorno<UsersDTO>> DeleteUser(string id)
        {

            var result = new KRetorno<UsersDTO>();

            try
            {
                if (UsersIdExists(id))
                {
                    UsersDTO user = _context.Users.FirstOrDefault(t => t.Id == id);
                    if (user != null)
                    {
                        user.isActive = !user.isActive;
                        _context.Entry(user).State = EntityState.Modified;
                        _context.SaveChanges();

                        result.Sucesso = true;
                        result.TRetorno = user;
                        result.Message = user.isActive ? "O Usuário '" + user.UserName + "' ativado com sucesso." : "O Usuário '" + user.UserName + "' desativado com sucesso.";
                    }
                    else
                    {
                        result.Sucesso = false;
                        result.Message = "Usuário não localizado.";
                    }

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Usuário não localizado.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível desativar/ativar o Usuário . Erro: " + e.Message;
            }

            return result;
        }

        private bool UsersIdExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
