﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;
using TorannMagic.TMDefs;

namespace TorannMagic.Golems
{
    public class TM_Golem : IExposable
    {
        //Golem Core Defs
        public int activationTicks = 240;
        public int processorEvaluationTicks = 100;
        public int shardLevel = 0;

        public ThingDef workstationDef = null;
        public ThingDef pawnDef = null;

        public List<TM_GolemUpgrade> upgrades = new List<TM_GolemUpgrade>();
        public List<NeedDef> needs = new List<NeedDef>();
        public List<HediffDef> hediffs = new List<HediffDef>();

        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref activationTicks, "activationTicks", 240);
            Scribe_Values.Look<int>(ref processorEvaluationTicks, "processorEvaluationTicks", 100);
            Scribe_Values.Look<int>(ref shardLevel, "shardLevel", 0);
            Scribe_Collections.Look<TM_GolemUpgrade>(ref upgrades, "upgrades", LookMode.Deep);
            Scribe_Collections.Look<NeedDef>(ref needs, "needs", LookMode.Def);
            Scribe_Collections.Look<HediffDef>(ref hediffs, "hediffs", LookMode.Def);
        }

        public TM_Golem()
        {

        }

        public TM_Golem(Thing thing)
        {
            TM_GolemDef gd = TM_GolemUtility.GetGolemDefFromThing(thing);
            activationTicks = gd.activationTicks;
            processorEvaluationTicks = gd.processorEvaluationTicks;
            workstationDef = gd.golemWorkstationDef;
            pawnDef = gd.golemWorkstationDef;
            upgrades.Clear();
            foreach(TM_GolemUpgradeDef gud in gd.upgrades)
            {
                TM_GolemUpgrade gu = new TM_GolemUpgrade(gud);
                upgrades.Add(gu);
            }
            needs.Clear();
            needs.AddRange(gd.needs);
            hediffs.Clear();
            hediffs.AddRange(gd.hediffs);
        }

        public TM_Golem(TM_Golem fromGolem, Thing thing)
        {
            TM_GolemDef gd = TM_GolemUtility.GetGolemDefFromThing(thing);
            activationTicks = gd.activationTicks;
            processorEvaluationTicks = gd.processorEvaluationTicks;
            workstationDef = gd.golemWorkstationDef;
            pawnDef = gd.golemWorkstationDef;
            upgrades.Clear();
            upgrades.AddRange(fromGolem.upgrades);
            needs.Clear();
            needs.AddRange(gd.needs);
            hediffs.Clear();
            hediffs.AddRange(gd.hediffs);
        }
    }
}