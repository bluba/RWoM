﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using Verse.AI;
using UnityEngine;

namespace TorannMagic
{
    public class Verb_BrandProtection : Verb_UseAbility
    {

        private int verVal = 0;
        private int pwrVal = 0;
        private float arcaneDmg = 1f;
        bool validTarg;
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    validTarg = true;
                }
                else
                {
                    //out of range
                    validTarg = false;
                }
            }
            else
            {
                validTarg = false;
            }
            return validTarg;
        }

        protected override bool TryCastShot()
        {
            bool flag = false;
            Pawn caster = this.CasterPawn;
            Pawn hitPawn = this.currentTarget.Thing as Pawn;
            if(hitPawn != null && hitPawn.RaceProps != null)
            {
                CompAbilityUserMagic casterComp = caster.TryGetComp<CompAbilityUserMagic>();
                CompAbilityUserMagic targetComp = hitPawn.TryGetComp<CompAbilityUserMagic>();

                if (casterComp != null && hitPawn.health != null && hitPawn.health.hediffSet != null && hitPawn != caster)
                {
                    Hediff oldBrand = hitPawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_ProtectionBrandHD);
                    if(oldBrand != null)
                    {
                        HediffComp_BrandingProtection hd_br = oldBrand.TryGetComp<HediffComp_BrandingProtection>();
                        CompAbilityUserMagic branderComp = hd_br.BranderPawn.TryGetComp<CompAbilityUserMagic>();
                        branderComp.BrandedPawns.Remove(hitPawn);
                    }
                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_ProtectionBrandHD, .05f);
                    casterComp.BrandedPawns.Add(hitPawn);
                    Hediff newBrand = hitPawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_ProtectionBrandHD);
                    newBrand.TryGetComp<HediffComp_BrandingProtection>().BranderPawn = caster;

                    TargetInfo ti = new TargetInfo(hitPawn.Position, hitPawn.Map, false);
                    TM_MoteMaker.MakeOverlay(ti, TorannMagicDefOf.TM_Mote_PsycastAreaEffect, hitPawn.Map, Vector3.zero, .1f, 0f, .1f, .3f, .3f, -2f);

                    for (int i =0; i < 15; i++)
                    {
                        Vector3 drawPos = hitPawn.DrawPos;
                        drawPos.x += Rand.Range(-.6f, .6f);
                        drawPos.z += Rand.Range(-.6f, .6f);
                        TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_GlowingRuneA, drawPos, hitPawn.Map, Rand.Range(.05f, .15f), Rand.Range(.2f, .3f), Rand.Range(.1f, .25f), Rand.Range(.2f, .7f), Rand.Range(-20, 20), 0f, 0f, Rand.Range(0, 360));
                    }
                }
                else
                {
                    Messages.Message("TM_InvalidTarget".Translate(CasterPawn.LabelShort, this.Ability.Def.label), MessageTypeDefOf.RejectInput);
                }
            }
            else
            {
                Messages.Message("TM_InvalidTarget".Translate(CasterPawn.LabelShort, this.Ability.Def.label), MessageTypeDefOf.RejectInput);
            }

            this.PostCastShot(flag, out flag);
            return flag;
        }        
    }
}
