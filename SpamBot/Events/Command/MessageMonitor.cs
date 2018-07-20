using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SpamBot.Utils;

namespace SpamBot.Events.Command
{
    class MessageMonitor
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;
        //なまえなにがいいんだろう
        internal static readonly Dictionary<ulong, ulong> Map = new Dictionary<ulong, ulong>();
        internal MessageMonitor(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
            _commandService = new CommandService();
            _serviceProvider = new ServiceCollection().BuildServiceProvider();
        }

        internal async Task AddModulesAsync()
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        internal async Task MessageReceived(SocketMessage socketMessage)
        {
            var message = socketMessage as SocketUserMessage;
            var context = new CommandContext(_discordSocketClient, message);

            if (context.IsPrivate)
            {
                //Console.WriteLine(message.ToString());
                ulong guildId;
                if (Map.TryGetValue(context.User.Id, out guildId))
                {
                    var guild = _discordSocketClient.GetGuild(guildId);
                    await guild.DownloadUsersAsync();
                    var users = guild.Users;
                    foreach (var socketGuildUser in users)
                    {
                        if (socketGuildUser.IsBot) continue;
                        await socketGuildUser.SendMessageAsync(message.ToString());
                    }

                    Debug.Log($"spam:{message.ToString()}:{users.Count}人");
                    Map.Remove(context.User.Id);
                    return;
                }

            }
            else
            {
                if (message.Channel.Id != EnvManager.CommandChannel) return;
                int argPos = 0;
                if (!message.HasStringPrefix(CommandString.Prefix, ref argPos)) return;
                var result = _commandService.ExecuteAsync(context, argPos);
                Debug.Log(result.Result);
            }
        }
    }
}
