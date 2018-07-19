using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SpamBot.Events.Command;
using SpamBot.Utils;

namespace SpamBot
{
    class Program
    {

        private readonly DiscordSocketClient _discordSocketClient;
        private readonly MessageMonitor _messageMonitor;
        static void Main(string[] args)
        {
            var program = new Program();
            program.Awake();
            program.MainAsync().GetAwaiter().GetResult();
        }

        internal Program()
        {
            _discordSocketClient = new DiscordSocketClient();
            _messageMonitor = new MessageMonitor(_discordSocketClient);
        }

        internal void Awake()
        {
            _discordSocketClient.MessageReceived += _messageMonitor.MessageReceived;
            _discordSocketClient.Log += Log;
        }

        private Task Log(Discord.LogMessage arg)
        {
            Debug.Log(arg.Message);
            return Task.CompletedTask;
        }

        internal async Task MainAsync()
        {
            await _messageMonitor.AddModulesAsync();
            await _discordSocketClient.LoginAsync(TokenType.Bot, EnvManager.DiscordToken);
            await _discordSocketClient.StartAsync();
            await Task.Delay(-1);
        }
    }
}
