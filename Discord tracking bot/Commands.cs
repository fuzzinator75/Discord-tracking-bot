using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Discord_tracking_bot
{
    public class Commands : ModuleBase<SocketCommandContext>
    {


        #region Utilities

        [Command("!WipeAll")]
        [Summary("Wipe all current challange data.")]
        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task WipeAll()
        {

        }

        [Command("!commands")]
        [Summary("Lists All Available Commands")]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task ShowCommands()
        {

        }
        #endregion

        #region Miles
        [Command("!MilesStat")]
        [Summary("Shows Users Current Miles")]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task MileStat()
        {

        }

        [Command("!MilesAdd")]
        [Summary("Adds to Users Current Miles")]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task MileAdd()
        {

        }

        [Command("!MilesRemove")]
        [Summary("Removes Users Current Miles")]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task MileRem()
        {

        }
        #endregion

        #region Hours
        [Command("!HourStat")]
        [Summary("Shows Users Current Miles")]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task HourStat()
        {

        }

        [Command("!HourAdd")]
        [Summary("Adds to Users Current Miles")]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task HourAdd()
        {

        }

        [Command("!HourRemove")]
        [Summary("Removes Users Current Miles")]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task HourRem()
        {

        }
        #endregion

        #region Minutes
        [Command("!MinStat")]
        [Summary("Shows Users Current Miles")]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task MinStat()
        {

        }

        [Command("!MinAdd")]
        [Summary("Adds to Users Current Miles")]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task MinAdd()
        {

        }

        [Command("!MinRemove")]
        [Summary("Removes Users Current Miles")]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task MinRem()
        {

        }

        #endregion
    }
}
