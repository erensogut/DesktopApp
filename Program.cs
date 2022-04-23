// See https://aka.ms/new-console-template for more information
using System.Security;
using System.Text;
using DesktopApp;
using DesktopApp.Cron;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


//await HttpClientCall.ProcessRepositories();
using IHost host = Host.CreateDefaultBuilder(args).Build();

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
var url = config.GetSection("url").Value;
int checkFilePeriod = int.Parse(config.GetSection("checkPeriodInSecond").Value);

Console.WriteLine("Please Login To Desktop App!");
Console.Write("User Name: ");
string userName = Console.ReadLine();
Console.Write("Password: ");
var password = new StringBuilder();
while (true)
{
    ConsoleKeyInfo info = Console.ReadKey(true);
    if (info.Key == ConsoleKey.Enter)
    {
        break;
    }
    else if (info.KeyChar != '\u0000') 
    {
        password = password.Append(info.KeyChar);
        Console.Write("*");
    }
}

var client = new HttpClientCall(url);
var token = await client.Login(userName, password.ToString());

await Scheduler.StartAsync(checkFilePeriod, client);

//Console readline wait for turnoff program
Console.ReadLine();

