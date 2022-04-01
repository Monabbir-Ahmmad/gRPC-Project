namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            UserActions userActions = new();

            while (true)
            {
                Console.WriteLine("\n______Available Options______\n");

                Console.WriteLine("1. Register new account");
                Console.WriteLine("2. Login");
                Console.Write("\nYour choice: ");

                int option = Convert.ToInt32(Console.ReadLine());

                if (option == 1)
                {
                    await userActions.CreateAccount();
                }
                else if (option == 2)
                {
                    await userActions.Login();
                }
                else
                {
                    break;
                }

            }

        }
    }
}