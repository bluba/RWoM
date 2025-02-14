﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using Verse.AI;


namespace TorannMagic
{
    public class Verb_BrandEmotion : Verb_UseAbility
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
                    Hediff oldBrand = hitPawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_EmotionBrandHD);
                    if(oldBrand != null)
                    {
                        HediffComp_BrandingEmotion hd_br = oldBrand.TryGetComp<HediffComp_BrandingEmotion>();
                        CompAbilityUserMagic branderComp = hd_br.BranderPawn.TryGetComp<CompAbilityUserMagic>();
                        branderComp.BrandedPawns.Remove(hitPawn);
                    }
                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_EmotionBrandHD, .05f);
                    casterComp.BrandedPawns.Add(hitPawn);
                    Hediff newBrand = hitPawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_EmotionBrandHD);
                    newBrand.TryGetComp<HediffComp_BrandingEmotion>().BranderPawn = caster;

                    Effecter effect = EffecterDefOf.Skip_EntryNoDelay.Spawn();
                    effect.Trigger(new TargetInfo(caster), new TargetInfo(hitPawn));
                    effect.Cleanup();

                    Effecter effectExit = EffecterDefOf.Skip_ExitNoDelay.Spawn();
                    effectExit.Trigger(new TargetInfo(hitPawn), new TargetInfo(hitPawn));
                    effectExit.Cleanup();
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
