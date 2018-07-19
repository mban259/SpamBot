using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SpamBot.Utils;

namespace SpamBot.Events.Command
{
    public class CommandModule : ModuleBase
    {
        [Command(CommandString.Spam)]
        internal async Task Spam()
        {
            await Context.User.SendMessageAsync("おくるないよう");
            MessageMonitor.Map[Context.User.Id] = Context.Guild.Id;
        }
    }
}
