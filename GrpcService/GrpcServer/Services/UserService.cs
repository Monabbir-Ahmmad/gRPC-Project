using Grpc.Core;
using GrpcServer.Database;

namespace GrpcServer.Services
{
    public class UserService : User.UserBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public override Task<AccountCreationReply> CreateAccount(AccountCreationRequest request, ServerCallContext context)
        {
            DatabaseHelper dbHelper = new();

            bool result = dbHelper.InsertUserRecord(new UserModel() { Username = request.Name, Email = request.Email, Password = request.Password });

            return Task.FromResult(new AccountCreationReply
            {
                Success = result,
                Message = result ? "Hello " + request.Name + ", your account has been created." : "Account creation failed. Email already exists."
            });
        }

        public override Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
        {
            DatabaseHelper dbHelper = new();

            int userID = dbHelper.FindOneUser(new UserModel() { Email = request.Email, Password = request.Password });

            return Task.FromResult(new LoginReply
            {
                Success = userID >= 0,
                Message = userID >= 0 ? "Login successful." : "Login failed. Wrong credentials.",
                UserID = userID,
            });
        }
    }
}