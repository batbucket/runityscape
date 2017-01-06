using UnityEngine;
using System.Collections;
using System;
using Scripts.Model.Stats.Attributes;
using Scripts.Model.Spells;
using System.Collections.Generic;
using Scripts.Model.Stats.Resources;
using Scripts.Model.Spells.Named;
using Script.Model.Characters.Named;
using Scripts.Presenter;
using Scripts.Model.Acts;

namespace Scripts.Model.Characters.Named {

    public class Kitsune : ComputerCharacter {
        private const int AGI = 2;
        private const string ATTACK_NAME = "Attack";
        private const float BISHOP1_SPAWN_PERCENTAGE = 0.75f;
        private const float BISHOP2_SPAWN_PERCENTAGE = 0.45f;
        private const int FLEE_LIFE_COUNT = 25;
        private const int INT = 25;
        private const string SMITE_NAME = "Smite";
        private const int STR = 4;
        private const float TIME_BETWEEN_RATIO_MENTIONS = 20f;
        private const int VIT = 25;

        // Spawn at 75%
        private bool bishops1Spawned;

        // Spawn at 35%
        private bool bishops2Spawned;

        private Mimic[] bishopSpawn = new Mimic[] {
            new Regenerator(), new Regenerator()
        };

        /// <summary>
        /// Boss cannot spam backturns
        /// </summary>
        private float lastTimeBackTurned;

        /// <summary>
        /// Boss will call out player if they Attack when they can Smite
        /// indicating suboptimal DPS
        /// </summary>
        private float lastTimeMentionedRatio;

        private IDictionary<string, int> spellsRecieved;

        public Kitsune()
            : base(
            new Displays { Loc = "fox-head", Name = "Kitsune", Color = Color.magenta, Check = "One bad fox. Attacking while her back is turned is not advised." },
            new StartStats { Lvl = 10, Str = 50, Int = 50, Agi = 50, Vit = 50 }
            ) {
            Spells.Add(new Attack());
            Spells.Add(new BackTurn());
            Spells.Add(new SummonTent(null));
            AddResource(new NamedResource.Skill());
            Attributes[AttributeType.STRENGTH].False = STR;
            Attributes[AttributeType.INTELLIGENCE].False = INT;
            Attributes[AttributeType.AGILITY].False = AGI;
            Attributes[AttributeType.VITALITY].False = VIT;
            this.spellsRecieved = new Dictionary<string, int>();
        }

        private int AttacksRecieved {
            get {
                if (spellsRecieved.ContainsKey(ATTACK_NAME)) {
                    return spellsRecieved[ATTACK_NAME];
                }
                return 0;
            }
        }

        private bool CanMentionRatio {
            get {
                return (BattleTimer - lastTimeMentionedRatio) > TIME_BETWEEN_RATIO_MENTIONS;
            }
        }

        private bool HitEscapeThreshold {
            get {
                return GetResourceCount(ResourceType.HEALTH, false) <= FLEE_LIFE_COUNT;
            }
        }

        private float LifePercentage {
            get {
                return ((float)GetResourceCount(ResourceType.HEALTH, false)) / GetResourceCount(ResourceType.HEALTH, true);
            }
        }

        private bool RatioFavorsAttacks {
            get {
                return AttacksRecieved / 2 > SmitesRecieved;
            }
        }

        private int SmitesRecieved {
            get {
                if (spellsRecieved.ContainsKey(SMITE_NAME)) {
                    return spellsRecieved[SMITE_NAME];
                }
                return 0;
            }
        }

        public override void OnVictory() {
            Speak("Hardly a challenge.", "How many is that, now?");
        }

        public override void React(Spell spell) {
            if (spell.Caster.Side != Side) {
                if (!spellsRecieved.ContainsKey(spell.SpellFactory.Name)) {
                    spellsRecieved.Add(spell.SpellFactory.Name, 1);
                }
                spellsRecieved[spell.SpellFactory.Name]++;

                if (RatioFavorsAttacks && CanMentionRatio && !HitEscapeThreshold) {
                    lastTimeMentionedRatio = BattleTimer;
                    Speak("Not a fan of spells, I see.",
                        "You really like attacking, huh?",
                        "You really like swinging that thing around, huh?",
                        "Not a spellcaster?",
                        "What a predictable pattern...");
                }
            }
        }

        protected override void DecideSpell() {
            if (HitEscapeThreshold && (new FakeFlee().IsCastable(this))) {
                Game.Instance.Cutscene(true,
                        new Act(Talk("Only a fool wouldn't flee from this battle.")),
                        new Act(() => (new FakeFlee()).Cast(this, this))
                    );
            }
            if (!bishops1Spawned && LifePercentage < BISHOP1_SPAWN_PERCENTAGE) {
                bishops1Spawned = true;
                QuickCast(new SummonTent(bishopSpawn));
            }
            if (!bishops2Spawned && LifePercentage < BISHOP2_SPAWN_PERCENTAGE) {
                bishops2Spawned = true;
                QuickCast(new SummonTent(bishopSpawn));
            }

            QuickCast(new BackTurn());
            QuickCast(new Attack());
        }

        protected override bool IsDefeated() {
            return false;
        }
    }
}