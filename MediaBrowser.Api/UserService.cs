﻿using MediaBrowser.Common.Extensions;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Dto;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Net;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Connect;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Users;
using ServiceStack;
using ServiceStack.Text.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaBrowser.Api
{
    /// <summary>
    /// Class GetUsers
    /// </summary>
    [Route("/Users", "GET", Summary = "Gets a list of users")]
    [Authenticated]
    public class GetUsers : IReturn<List<UserDto>>
    {
        [ApiMember(Name = "IsHidden", Description = "Optional filter by IsHidden=true or false", IsRequired = false, DataType = "bool", ParameterType = "query", Verb = "GET")]
        public bool? IsHidden { get; set; }

        [ApiMember(Name = "IsDisabled", Description = "Optional filter by IsDisabled=true or false", IsRequired = false, DataType = "bool", ParameterType = "query", Verb = "GET")]
        public bool? IsDisabled { get; set; }

        [ApiMember(Name = "IsGuest", Description = "Optional filter by IsGuest=true or false", IsRequired = false, DataType = "bool", ParameterType = "query", Verb = "GET")]
        public bool? IsGuest { get; set; }
    }

    [Route("/Users/Public", "GET", Summary = "Gets a list of publicly visible users for display on a login screen.")]
    public class GetPublicUsers : IReturn<List<UserDto>>
    {
    }

    /// <summary>
    /// Class GetUser
    /// </summary>
    [Route("/Users/{Id}", "GET", Summary = "Gets a user by Id")]
    [Authenticated(EscapeParentalControl = true)]
    public class GetUser : IReturn<UserDto>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [ApiMember(Name = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "GET")]
        public string Id { get; set; }
    }

    /// <summary>
    /// Class DeleteUser
    /// </summary>
    [Route("/Users/{Id}", "DELETE", Summary = "Deletes a user")]
    [Authenticated(Roles = "Admin")]
    public class DeleteUser : IReturnVoid
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [ApiMember(Name = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "DELETE")]
        public string Id { get; set; }
    }

    /// <summary>
    /// Class AuthenticateUser
    /// </summary>
    [Route("/Users/{Id}/Authenticate", "POST", Summary = "Authenticates a user")]
    public class AuthenticateUser : IReturn<AuthenticationResult>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [ApiMember(Name = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "POST")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [ApiMember(Name = "Password", IsRequired = true, DataType = "string", ParameterType = "body", Verb = "POST")]
        public string Password { get; set; }
    }

    /// <summary>
    /// Class AuthenticateUser
    /// </summary>
    [Route("/Users/AuthenticateByName", "POST", Summary = "Authenticates a user")]
    public class AuthenticateUserByName : IReturn<AuthenticationResult>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [ApiMember(Name = "Username", IsRequired = true, DataType = "string", ParameterType = "body", Verb = "POST")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [ApiMember(Name = "Password", IsRequired = true, DataType = "string", ParameterType = "body", Verb = "POST")]
        public string Password { get; set; }

        [ApiMember(Name = "PasswordMd5", IsRequired = true, DataType = "string", ParameterType = "body", Verb = "POST")]
        public string PasswordMd5 { get; set; }
    }

    /// <summary>
    /// Class UpdateUserPassword
    /// </summary>
    [Route("/Users/{Id}/Password", "POST", Summary = "Updates a user's password")]
    [Authenticated]
    public class UpdateUserPassword : IReturnVoid
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>The new password.</value>
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [reset password].
        /// </summary>
        /// <value><c>true</c> if [reset password]; otherwise, <c>false</c>.</value>
        public bool ResetPassword { get; set; }
    }

    /// <summary>
    /// Class UpdateUser
    /// </summary>
    [Route("/Users/{Id}", "POST", Summary = "Updates a user")]
    [Authenticated]
    public class UpdateUser : UserDto, IReturnVoid
    {
    }

    /// <summary>
    /// Class UpdateUser
    /// </summary>
    [Route("/Users/{Id}/Policy", "POST", Summary = "Updates a user policy")]
    [Authenticated(Roles = "admin")]
    public class UpdateUserPolicy : UserPolicy, IReturnVoid
    {
        [ApiMember(Name = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "POST")]
        public string Id { get; set; }
    }

    /// <summary>
    /// Class UpdateUser
    /// </summary>
    [Route("/Users/{Id}/Configuration", "POST", Summary = "Updates a user configuration")]
    [Authenticated]
    public class UpdateUserConfiguration : UserConfiguration, IReturnVoid
    {
        [ApiMember(Name = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "POST")]
        public string Id { get; set; }
    }

    /// <summary>
    /// Class CreateUser
    /// </summary>
    [Route("/Users/New", "POST", Summary = "Creates a user")]
    [Authenticated(Roles = "Admin")]
    public class CreateUserByName : IReturn<UserDto>
    {
        [ApiMember(Name = "Name", IsRequired = true, DataType = "string", ParameterType = "body", Verb = "POST")]
        public string Name { get; set; }
    }

    [Route("/Users/ForgotPassword", "POST", Summary = "Initiates the forgot password process for a local user")]
    public class ForgotPassword : IReturn<ForgotPasswordResult>
    {
        [ApiMember(Name = "EnteredUsername", IsRequired = false, DataType = "string", ParameterType = "body", Verb = "POST")]
        public string EnteredUsername { get; set; }
    }

    [Route("/Users/ForgotPassword/Pin", "POST", Summary = "Redeems a forgot password pin")]
    public class ForgotPasswordPin : IReturn<PinRedeemResult>
    {
        [ApiMember(Name = "Pin", IsRequired = false, DataType = "string", ParameterType = "body", Verb = "POST")]
        public string Pin { get; set; }
    }

    /// <summary>
    /// Class UsersService
    /// </summary>
    public class UserService : BaseApiService, IHasAuthorization
    {
        /// <summary>
        /// The _user manager
        /// </summary>
        private readonly IUserManager _userManager;
        private readonly IDtoService _dtoService;
        private readonly ISessionManager _sessionMananger;
        private readonly IServerConfigurationManager _config;
        private readonly INetworkManager _networkManager;

        public IAuthorizationContext AuthorizationContext { get; set; }

        public UserService(IUserManager userManager, IDtoService dtoService, ISessionManager sessionMananger, IServerConfigurationManager config, INetworkManager networkManager)
        {
            _userManager = userManager;
            _dtoService = dtoService;
            _sessionMananger = sessionMananger;
            _config = config;
            _networkManager = networkManager;
        }

        public object Get(GetPublicUsers request)
        {
            // If the startup wizard hasn't been completed then just return all users
            if (!_config.Configuration.IsStartupWizardCompleted)
            {
                return Get(new GetUsers
                {
                    IsDisabled = false
                });
            }

            // TODO: Uncomment once clients can handle an empty user list (and below)
            //if (Request.IsLocal || IsInLocalNetwork(Request.RemoteIp))
            {
                return Get(new GetUsers
                {
                    IsHidden = false,
                    IsDisabled = false
                });
            }

            //// Return empty when external
            //return ToOptimizedResult(new List<UserDto>());
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.Object.</returns>
        public object Get(GetUsers request)
        {
            var users = _userManager.Users;

            if (request.IsDisabled.HasValue)
            {
                users = users.Where(i => i.Configuration.IsDisabled == request.IsDisabled.Value);
            }

            if (request.IsHidden.HasValue)
            {
                users = users.Where(i => i.Configuration.IsHidden == request.IsHidden.Value);
            }

            if (request.IsGuest.HasValue)
            {

                users = users.Where(i => (i.ConnectLinkType.HasValue && i.ConnectLinkType.Value == UserLinkType.Guest) == request.IsGuest.Value);
            }

            var result = users
                .OrderBy(u => u.Name)
                .Select(i => _userManager.GetUserDto(i, Request.RemoteIp))
                .ToList();

            return ToOptimizedSerializedResultUsingCache(result);
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.Object.</returns>
        public object Get(GetUser request)
        {
            var user = _userManager.GetUserById(request.Id);

            if (user == null)
            {
                throw new ResourceNotFoundException("User not found");
            }

            var result = _userManager.GetUserDto(user, Request.RemoteIp);

            return ToOptimizedSerializedResultUsingCache(result);
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Delete(DeleteUser request)
        {
            var task = DeleteAsync(request);

            Task.WaitAll(task);
        }

        public async Task DeleteAsync(DeleteUser request)
        {
            var user = _userManager.GetUserById(request.Id);

            if (user == null)
            {
                throw new ResourceNotFoundException("User not found");
            }

            await _sessionMananger.RevokeUserTokens(user.Id.ToString("N")).ConfigureAwait(false);

            await _userManager.DeleteUser(user).ConfigureAwait(false);
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public object Post(AuthenticateUser request)
        {
            var user = _userManager.GetUserById(request.Id);

            if (user == null)
            {
                throw new ResourceNotFoundException("User not found");
            }

            return Post(new AuthenticateUserByName
            {
                Username = user.Name,
                Password = request.Password
            });
        }

        public async Task<object> Post(AuthenticateUserByName request)
        {
            var auth = AuthorizationContext.GetAuthorizationInfo(Request);

            if (string.IsNullOrWhiteSpace(auth.Client))
            {
                auth.Client = "Unknown app";
            }
            if (string.IsNullOrWhiteSpace(auth.Device))
            {
                auth.Device = "Unknown device";
            }
            if (string.IsNullOrWhiteSpace(auth.Version))
            {
                auth.Version = "Unknown version";
            }
            if (string.IsNullOrWhiteSpace(auth.DeviceId))
            {
                auth.DeviceId = "Unknown device id";
            }

            var result = await _sessionMananger.AuthenticateNewSession(new AuthenticationRequest
            {
                App = auth.Client,
                AppVersion = auth.Version,
                DeviceId = auth.DeviceId,
                DeviceName = auth.Device,
                PasswordSha1 = request.Password,
                PasswordMd5 = request.PasswordMd5,
                RemoteEndPoint = Request.RemoteIp,
                Username = request.Username

            }).ConfigureAwait(false);

            return ToOptimizedResult(result);
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Post(UpdateUserPassword request)
        {
            var task = PostAsync(request);
            Task.WaitAll(task);
        }

        public async Task PostAsync(UpdateUserPassword request)
        {
            var user = _userManager.GetUserById(request.Id);

            if (user == null)
            {
                throw new ResourceNotFoundException("User not found");
            }

            if (request.ResetPassword)
            {
                await _userManager.ResetPassword(user).ConfigureAwait(false);
            }
            else
            {
                var success = await _userManager.AuthenticateUser(user.Name, request.CurrentPassword, Request.RemoteIp).ConfigureAwait(false);

                if (!success)
                {
                    throw new ArgumentException("Invalid user or password entered.");
                }

                await _userManager.ChangePassword(user, request.NewPassword).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Post(UpdateUser request)
        {
            var task = PostAsync(request);

            Task.WaitAll(task);
        }

        public async Task PostAsync(UpdateUser request)
        {
            // We need to parse this manually because we told service stack not to with IRequiresRequestStream
            // https://code.google.com/p/servicestack/source/browse/trunk/Common/ServiceStack.Text/ServiceStack.Text/Controller/PathInfo.cs
            var pathInfo = PathInfo.Parse(Request.PathInfo);
            var id = new Guid(pathInfo.GetArgumentValue<string>(1));

            var dtoUser = request;

            var user = _userManager.GetUserById(id);

            // If removing admin access
            if (!dtoUser.Configuration.IsAdministrator && user.Configuration.IsAdministrator)
            {
                if (_userManager.Users.Count(i => i.Configuration.IsAdministrator) == 1)
                {
                    throw new ArgumentException("There must be at least one user in the system with administrative access.");
                }
            }

            // If disabling
            if (dtoUser.Configuration.IsDisabled && user.Configuration.IsAdministrator)
            {
                throw new ArgumentException("Administrators cannot be disabled.");
            }

            // If disabling
            if (dtoUser.Configuration.IsDisabled && !user.Configuration.IsDisabled)
            {
                if (_userManager.Users.Count(i => !i.Configuration.IsDisabled) == 1)
                {
                    throw new ArgumentException("There must be at least one enabled user in the system.");
                }

                await _sessionMananger.RevokeUserTokens(user.Id.ToString("N")).ConfigureAwait(false);
            }

            var task = user.Name.Equals(dtoUser.Name, StringComparison.Ordinal) ?
                _userManager.UpdateUser(user) :
                _userManager.RenameUser(user, dtoUser.Name);

            await task.ConfigureAwait(false);

            user.UpdateConfiguration(dtoUser.Configuration);
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.Object.</returns>
        public object Post(CreateUserByName request)
        {
            var dtoUser = request;

            var newUser = _userManager.CreateUser(dtoUser.Name).Result;

            var result = _userManager.GetUserDto(newUser, Request.RemoteIp);

            return ToOptimizedResult(result);
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.Object.</returns>
        public object Post(ForgotPassword request)
        {
            var isLocal = Request.IsLocal || _networkManager.IsInLocalNetwork(Request.RemoteIp);

            return _userManager.StartForgotPasswordProcess(request.EnteredUsername, isLocal);
        }

        public object Post(ForgotPasswordPin request)
        {
            return _userManager.RedeemPasswordResetPin(request.Pin);
        }

        public void Post(UpdateUserConfiguration request)
        {
            var user = _userManager.GetUserById(request.Id);
            user.UpdateConfiguration(request);
        }

        public void Post(UpdateUserPolicy request)
        {
            var task = _userManager.UpdateUserPolicy(request.Id, request);
            Task.WaitAll(task);
        }
    }
}
