﻿#pragma warning disable CS0626 // orig_ method is marked external and has no attributes on it.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod;
using System.IO;
using System.Reflection;
using UnityEngine;
using Mirror;
using System.Net;
using GameCore;

using PheggMod.API.Events;

namespace PheggMod.EventTriggers
{
    [MonoModPatch("global::AlphaWarheadNukesitePanel")]
    internal class PMAlphaWarheadNukesitePanel : AlphaWarheadNukesitePanel
    {
        public extern bool orig_AllowChangeLevelState();
        public new bool AllowChangeLevelState()
        {
            if (PMAlphaWarheadController.nukeLock) return false;

            return orig_AllowChangeLevelState();
        }

        public static void Enable() => FindObjectOfType<AlphaWarheadNukesitePanel>().Networkenabled = true;
        public static void Disable() => FindObjectOfType<AlphaWarheadNukesitePanel>().Networkenabled = false;
    }
}
