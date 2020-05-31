using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using RemoteAdmin;
using Telepathy;
using Mirror;
using PheggMod.API;
using System.Text.RegularExpressions;

namespace PheggMod
{
    public class PheggPlayer
    {
        private static Regex _filterNames = new Regex("[(\\*)|(_)|({)|(})|(@)|(<)|(>)|(\")]");

        //Basic user info
        public string name { get; internal set; }
        public string nameClean { get; internal set; }
        public string userId { get; internal set; }
        public string domain { get; internal set; }
        public string ipAddress { get; internal set; }
        public int playerId { get; internal set; }

        //Components
        public Components commonComponents;
        #region components
        private CharacterClassManager _CharacterClassManager { get; set; }
        private ServerRoles _serverRoles { get; set; }
        private NicknameSync _nicknameSync { get; set; }
        private QueryProcessor _queryProcessor { get; set; }
        private Handcuffs _handcuffs { get; set; }
        private PlayerStats _playerStats { get; set; }
        private AmmoBox _ammoBox { get; set; }
        private Inventory _inventory { get; set; }
        private PlayerMovementSync _plyMovementSync { get; set; }
        private BanPlayer _banPlayer { get; set; }
        private NetworkConnection _networkConnection { get; set; }
        private Broadcast _broadcast { get; set; }
        #endregion

        public GameObject gameObject { get; internal set; }


        public class TeamClass
        {
            public RoleType role { get; internal set; }
            public Team team { get; internal set; }
            public CleanTeam cleanTeam { get; internal set; }

        }
        public class Components
        {
            public CharacterClassManager ccm;
            public ServerRoles sr;
            public NicknameSync ns;
            public QueryProcessor qp;
            public Handcuffs hc;
            public PlayerStats ps;
            public AmmoBox ab;
            public Inventory inv;
            public PlayerMovementSync pms;
            public BanPlayer bp;
            public NetworkConnection nc;
            public Broadcast bc;
        }

        public PheggPlayer(GameObject player)
        {
            if (player == null)
                throw new Exception("Cannot create PheggPlayer from null game object");
            else if (player.GetComponent<CharacterClassManager>().isLocalPlayer)
            {
                Base.Debug("Cannot create PheggPlayer for server");
                return;
            }
            else
            {
                try
                {
                    #region components
                    _CharacterClassManager = player.GetComponent<CharacterClassManager>();
                    _serverRoles = player.GetComponent<ServerRoles>();
                    _nicknameSync = player.GetComponent<NicknameSync>();
                    _queryProcessor = player.GetComponent<QueryProcessor>();
                    _handcuffs = player.GetComponent<Handcuffs>();
                    _playerStats = player.GetComponent<PlayerStats>();
                    _ammoBox = player.GetComponent<AmmoBox>();
                    _inventory = player.GetComponent<Inventory>();
                    _plyMovementSync = player.GetComponent<PlayerMovementSync>();
                    _banPlayer = player.GetComponent<BanPlayer>();
                    _networkConnection = player.GetComponent<NetworkConnection>();
                    _broadcast = player.GetComponent<Broadcast>();
                    #endregion

                    name = _nicknameSync.MyNick;
                    nameClean = _filterNames.Replace(name, string.Empty);
                    userId = _CharacterClassManager.UserId;
                    domain = _CharacterClassManager.UserId.Split('@')[1].ToUpper();
                    ipAddress = _nicknameSync.connectionToClient.address;
                    playerId = _queryProcessor.PlayerId;

                    gameObject = player;

                    commonComponents = new Components
                    {
                        ccm = _CharacterClassManager,
                        sr = _serverRoles,
                        ns = _nicknameSync,
                        qp = _queryProcessor,
                        hc = _handcuffs,
                        ps = _playerStats,
                        ab = _ammoBox,
                        inv = _inventory,
                        pms = _plyMovementSync,
                        bp = _banPlayer,
                        nc = _networkConnection,
                        bc = _broadcast
                    };
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public override string ToString()
        {
            return $"{_filterNames.Replace(name, string.Empty)} ({userId})";
        }

        public bool GodMode()
        {
            return _CharacterClassManager.GodMode;
        }
        public void GodMode(bool godmode)
        {
            _CharacterClassManager.GodMode = (bool)godmode;
        }

        public bool Bypass()
        {
            return gameObject.GetComponent<ServerRoles>().BypassMode;
        }
        public void Bypass(bool bypass)
        {
            gameObject.GetComponent<ServerRoles>().BypassMode = bypass;
        }

        public int Disarmed()
        {
            return _handcuffs.CufferId;
        }
        public void Disarmed(int playerid)
        {
            _handcuffs.CufferId = this.playerId;
        }

        public float Health()
        {
            return _playerStats.Health;
        }
        public void Health(float hp)
        {
            _playerStats.Health = hp;
        }

        public TeamClass Teamclass()
        {
            RoleType role = _CharacterClassManager.CurClass;
            Team team = _CharacterClassManager.Classes.SafeGet(_CharacterClassManager.CurClass).team;
            CleanTeam cTeam;

            if (team == Team.MTF || team == Team.RSC)
                cTeam = CleanTeam.NineTailedFox;
            else if (team == Team.CHI || team == Team.CDP)
                cTeam = CleanTeam.ChaosInsurgency;
            else if (team == Team.SCP)
                cTeam = CleanTeam.SCP;
            else if (team == Team.TUT)
                cTeam = CleanTeam.Tutorial;
            else cTeam = CleanTeam.Spectator;

            return new TeamClass { 
                role = role,
                team = team,
                cleanTeam = cTeam
            };
        }
        public void Teamclass(RoleType role)
        {
            _CharacterClassManager.SetClassID(role);
        }

        //public void SetAmmo(int type, int ammount)
        //{
        //    _ammoBox.SetOneAmount(type, ammount.ToString());
        //}

        public void Kill()
        {
            _playerStats.HurtPlayer(new PlayerStats.HitInfo(10000, "WORLD", DamageTypes.Nuke, playerId), gameObject);
        }

        public void Ban(int duration, string reason = "No reason provided", string issuer = "SERVER", bool banIP = true)
        {
            BanHandler.IssueBan(new BanDetails
            {
                OriginalName = name,
                Id = userId,
                Issuer = issuer,
                IssuanceTime = DateTime.UtcNow.Ticks,
                Expires = DateTimeOffset.UtcNow.AddMinutes(duration).Ticks,
                Reason = reason
            }, BanHandler.BanType.UserId);

            BanHandler.IssueBan(new BanDetails
            {
                OriginalName = name,
                Id = ipAddress,
                Issuer = issuer,
                IssuanceTime = DateTime.UtcNow.Ticks,
                Expires = DateTimeOffset.UtcNow.AddMinutes(duration).Ticks,
                Reason = reason
            }, BanHandler.BanType.IP);
        }

        public void Kick(string reason = "No reason provided", string issuer = "SERVER")
        {
            _banPlayer.KickUser(gameObject, reason, issuer, false);
        }

        public void GiveItem(ItemType type)
        {
            _inventory.AddNewItem(type);
        }
        public void GiveItems(ItemType[] items)
        {
            foreach (ItemType item in items)
                _inventory.AddNewItem(item);
        }
        public void ClearItems()
        {
            _inventory.Clear();
        }

        public void Teleport(Vector3 vector3, float rotation = 0, bool forcegound = true)
        {
            _plyMovementSync.OverridePosition(vector3, rotation, forcegound);
        }

        public void PersonalBroadcast(ushort duration, string message, Broadcast.BroadcastFlags flag = Broadcast.BroadcastFlags.Normal)
        {
            _broadcast.TargetAddElement(_networkConnection, message, duration, flag);
        }

        public void SendConsoleMessage(string message, string color = "green")
        {
            _CharacterClassManager.TargetConsolePrint(_networkConnection, message, color);
        }

        public void SetTag(string Text, TagColour colour, ulong permissions = 0)
        {
            _serverRoles.SetText(Text);

            if(colour != TagColour.DEFAULT) {
                _serverRoles.SetColor(Base.colours[(int)colour]);
            }
            
            _serverRoles.Permissions = permissions;
        }
    }
}
