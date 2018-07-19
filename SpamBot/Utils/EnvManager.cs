using System;
using System.Collections.Generic;
using System.Text;
using DotNetEnv;

namespace SpamBot.Utils
{
    static class EnvManager
    {
        internal static readonly string DiscordToken;
        internal static readonly ulong CommandChannel;

        static EnvManager()
        {
            Env.Load();
            DiscordToken = Env.GetString("DISCORD_TOKEN");
            CommandChannel = ulong.Parse(Env.GetString("COMMAND_CHANNEL"));
        }
    }
}
