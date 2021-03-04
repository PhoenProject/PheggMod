# PheggMod
 
# We have moved!
We have moved to a new repo over on GitLab. 
You can find us here: https://github.com/DragonSCP-Developers/PheggMod

# Plugin Directory
There are 2 locations where plugins can be loaded from:
 - config/ServerPort/plugins - This allows you to launch multiple servers using different plugins, without having all servers run them.
 - serverinstall/plugins - This is the same way as done with Smod, and allows for multiple servers to use the same plugin setup if `universal_config_file` is set to true


# Configs
As always, these configs all go inside of the `config_gameplay` file

| Config Option | Default value  | Description  |
|:-------------:|:---------------:|:---------------:|
| auto_restart_time | 04:30 | Specifies (on 24-hour format) when the server should automatically restart itself (E.g 05:30) |
| pheggmod_debug | false | Enables debug mode for Pheggmod and supporting plugins |
| universal_config_file | false | Specifies if the server should load plugins from your config location, or the plugins folder from the server's install directory |
| announce_chaos_spawn | true | Announces Chaos respawn |
| chaos_announcement | `ATTENTION ALL PERSONNEL . CHAOS INSURGENCY BREACH IN PROGRESS` | Message that plays when Chaos spawn |
| cassie_glitch | false | Disable's CASSIE's random glitching during (non-decontamination) announcements |
| cassie_glitch_post_detonation | false | Limit's CASSIE's random glitching to post-warhead detonation |
| fix_sticky_round | true | Fixes sticky rounds (Rounds that stay active despite no players being on the server) |
| report_message_content | string.empty | Message content (not from the embed) for the ingame report webhook on discord |
| notify_096_target | true | sends a BC when a someone becomes a target of 096 |
| scp173_door_cooldown | 25f | Cooldown in seconds until the 173 blastdoor can be opened |

# Commands
These are the available custom commands that Pheggmod offers
| Command (Aliases) | Variables | Permission | Description |
|:---------------:|:---------------:|:---------------:|:---------------:|
| oban (offlineban ltapban) | [UserID] [Duration (e.g 3d, 1y)] [Reason] | BanningUpToDay | Adds the UserID to the UserIdBans file |
| nukelock (nlock nukel locknuke) |  | WarheadEvents | Locks the nuke, preventing it from being enabled/disabled |
| nuke | [Enable\disable] | WarheadEvents | Enables/Disables the nuke's lever |
| pbc (personalbroadcast privatebroadcast) | [PlayerId] [Seconds] [Message] |  | Sends a broadcast to only the specified player |
| drop (dropall dropinv) | [PlayerID] | PlayersManagement | Drops all items from a player's inventory |
| kill | [PlayerID] | PlayersManagement | Kills the targeted player |
| lightsout | | FacilityManagement | Causes the lights to go out inside the facility (Due to the limitations of the game's code, this is quite buggy ATM) |
| pluginreload (reloadplugins plreload) | | ServerConfigs | Reloads the server's plugins on the next round restart |
| nevergonna | [Give] [You] [Up] | | See for yourself |
| status | | | Shows some of the current stats for the server |
| help | [CommandName] | | Shows available command information |
| curpos | [PlayerID] | | Prints the player's current position into RA |
| tower2 | [PlayerId] | | Teleports the player into a different surface tower |
| pocket | [PlayerId] | | Teleports the player into the pocket dimension |
| grenade |[PlayerId] | | Spawns a grenade at a player |
| flash | [PlayerId] | | Spawns a flashbang at a player |
| ball | [PlayerId] | | Spawns 018 at a player |

# Events
These are the available events that Pheggmod offers

**Admin Events**
| Event | Parameters |
|:-----:|:----------:|
| PlayerBanEvent | Player (PheggPlayer), Admin (PheggPlayer), Duration (Int), Reason (String) |
| PlayerKickEvent | Player (PheggPlayer), Admin (PheggPlayer), Reason (String) |
| GlobalBanEvent | Player (PheggPlayer) |
| AdminQueryEvent | Admin (PheggPlayer), Query (String) |
| RefreshAdminPermEvent | Admin (PheggPlayer) |

**Player Events**
| Event | Parameters |
|:-----:|:----------:|
| PlayerHurtEvent | Player (PheggPlayer), Attacker (PheggPlayer), Damage (Float), DamageType (DamageType) |
| PlayerDeathEvent | Player (PheggPlayer), Attacker (PheggPlayer), Damage(Float), Damagetype (DamageType) |
| PlayerSpawnEvent | Player (PheggPlayer), Role (Role), Team (Team) |
| PlayerEscapeEvent | Player (PheggPlayer), Role (Role), NewRole (Role), Team (Team)|
| PlayerJoinEvent | Player (PheggPlayer), Name (String), UserId (String), IpAddress (String) |
| PlayerLeaveEvent | Player (PheggPlayer), Name (String), UserId (String), IpAddress (String) |
| PlayerThrowGrenadeEvent | Player (PheggPlayer), Name (String), Grenade (String) |
| PlayerReportEvent | Reporter (PheggPlayer), Target (PheggPlayer), Reason (String) |

**Round Events**
| Event | Parameters |
|:-----:|:----------:|
| WaitingForPlayersEvent | |
| RoundStartEvent |  |
| RoundEndEvent | SCP (SCPs), ClassD (ClassD), Scientist (Scientists), Roundtime (String), LeadingTeam (LeadingTeam) |
| WarheadStartEvent | InitialStart (Bool), TimeToDetonation (Float) |
| WarheadCancelEvent | Disabler (PheggPlayer) |
| WarheadDetonationEvent |  |
| RespawnEvent | IsChaos (Bool) |

**Misc. Events**
| Event | Parameters |
|:-----:|:----------:|
| PreauthEvent | Endpoint (IPEndPoint), UserID (string) |
