﻿#pragma warning disable CS0626 // orig_ method is marked external and has no attributes on it.
using MonoMod;
using PheggMod.API.Commands;
using PheggMod.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PheggMod.EventTriggers
{
    [MonoModPatch("global::RemoteAdmin.CommandProcessor")]
    class PMCommandProcessor
    {
        internal static string lastCommand;

        public static extern void orig_ProcessQuery(string q, CommandSender sender);
        public static void ProcessQuery(string q, CommandSender sender)
        {
            try
            {
                GameObject go = PlayerManager.players.Where(p => p.GetComponent<NicknameSync>().MyNick == sender.Nickname).FirstOrDefault();

                string[] query = q.Split(new char[] { ' ' });

                if (!q.ToUpper().Contains("SILENT"))
                {
                    if (go != null)
                    {
                        lastCommand = q;
                        PheggPlayer pheggPlayer = new PheggPlayer(go);

                        if (q.ToUpper().Contains("CASSIE"))
                            q = q.ToUpper() + " PITCH_1";

                        PluginManager.TriggerEvent<IEventHandlerAdminQuery>(new AdminQueryEvent(pheggPlayer, q));

                        List<string> cmds = new List<string>();

                        if (PluginManager.oldCommands.ContainsKey(query[0]))
                        {
                            PluginManager.TriggerCommand(PluginManager.oldCommands[query[0]], q, pheggPlayer.gameObject, sender);
                            return;
                        }
                        else
                        {
                            if (PluginManager.TriggerCommand(new CommandInfo(sender, pheggPlayer.gameObject, query[0], query))) return;
                        }
                    }
                    else
                    {
                        if (PluginManager.TriggerConsoleCommand(new CommandInfo(sender, null, query[0], query))) return;
                    }
                }
                orig_ProcessQuery(q, sender);
            }
            catch (Exception e)
            {
                Base.Error($"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
