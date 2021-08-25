﻿using RimWorld;
using Verse;
using System.Collections.Generic;

namespace TorannMagic.Ideology
{
    public class TM_RitualRoleMage : RitualRole
    {
        public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
        {
            reason = null;
            if (!(p.Faction.IsPlayerSafe() || p.IsPrisoner))
            {
                if (!skipReason)
                {
                    reason = "TM_MessageRitualRoleMustBeColonistOrPrisoner".Translate(base.Label);
                }
                return false;
            }
            if (!p.RaceProps.Humanlike)
            {
                if (!skipReason)
                {
                    reason = "MessageRitualRoleMustBeHumanlike".Translate(base.LabelCap);
                }
                return false;
            }
            if(!TM_Calc.IsMagicUser(p))
            {
                if (!skipReason)
                {
                    reason = "TM_MessageRitualRoleMustBeMage".Translate(base.Label);
                }
                return false;
            }
            return true;
        }

        public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
        {
            reason = null;
            return false;
        }
    }
}
