﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using UnityEngine;

namespace TorannMagic.Golems
{
    public class JobGiver_EngageThreatInRange : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            CompGolem cg = pawn.TryGetComp<CompGolem>();
            Pawn meleeThreat = TM_Calc.FindNearbyEnemy(pawn, Mathf.RoundToInt(cg.threatRange));
            if (meleeThreat == null)
            {
                return null;
            }
            if (meleeThreat.IsInvisible())
            {
                return null;
            }
            if (IsHunting(pawn, meleeThreat))
            {
                return null;
            }
            if (IsDueling(pawn, meleeThreat))
            {
                return null;
            }
            if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
            {
                return null;
            }
            if (pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse && pawn.playerSettings.hostilityResponse != HostilityResponseMode.Attack)
            {
                return null;
            }
            if (!pawn.WorkTagIsDisabled(WorkTags.Violent))
            {
                Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, meleeThreat);
                job.maxNumMeleeAttacks = 1;
                job.expiryInterval = 300;
                job.reactingToMeleeThreat = true;
                return job;
            }
            return null;
        }

        private bool IsDueling(Pawn pawn, Pawn other)
        {
            LordJob_Ritual_Duel lordJob_Ritual_Duel;
            if ((lordJob_Ritual_Duel = (pawn.GetLord()?.LordJob as LordJob_Ritual_Duel)) != null)
            {
                return lordJob_Ritual_Duel.Opponent(pawn) == other;
            }
            return false;
        }

        private bool IsHunting(Pawn pawn, Pawn prey)
        {
            if (pawn.CurJob == null)
            {
                return false;
            }
            JobDriver_Hunt jobDriver_Hunt = pawn.jobs.curDriver as JobDriver_Hunt;
            if (jobDriver_Hunt != null)
            {
                return jobDriver_Hunt.Victim == prey;
            }
            JobDriver_PredatorHunt jobDriver_PredatorHunt = pawn.jobs.curDriver as JobDriver_PredatorHunt;
            if (jobDriver_PredatorHunt != null)
            {
                return jobDriver_PredatorHunt.Prey == prey;
            }
            return false;
        }
    } 
}
