using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    public class UserActions
    {
        readonly GrpcChannel channel;
        readonly User.UserClient? client;

        public UserActions()
        {
            this.channel = GrpcChannel.ForAddress("http://localhost:5233");
            this.client = new User.UserClient(channel);
        }

        public async Task CreateAccount()
        {
            Console.Write("Enter your name: ");
            var name = Console.ReadLine();

            Console.Write("Enter your email: ");
            var email = Console.ReadLine();

            Console.Write("Enter your password: ");
            var password = Console.ReadLine();

            AccountCreationReply? reply = await client.CreateAccountAsync(new AccountCreationRequest { Name = name, Email = email, Password = password });

            Console.WriteLine(reply.Message);
        }

        public async Task Login()
        {
            Console.Write("Enter your email: ");
            var email = Console.ReadLine();

            Console.Write("Enter your password: ");
            var password = Console.ReadLine();

            LoginReply? reply = await client.LoginAsync(new LoginRequest { Email = email, Password = password });

            Console.WriteLine(reply.Message);
        }
    }
}
