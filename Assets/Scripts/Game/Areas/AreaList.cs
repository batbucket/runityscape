using Scripts.Game.Areas;
using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.SFXs;
using Scripts.Game.Dungeons;
using Scripts.Game.Pages;
using Scripts.Game.Serialized;
using Scripts.Game.Stages;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.TextBoxes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Areas {

    public static class AreaList {

        /// <summary>
        /// Flags = World flags for current save
        /// Party = current party
        /// Page = Camp reference
        /// Page = DungeonPages reference
        /// Area = Area to return
        /// </summary>
        public static readonly ReadOnlyDictionary<AreaType, Func<Flags, Party, Page, Page, Area>> ALL_AREAS
            = new ReadOnlyDictionary<AreaType, Func<Flags, Party, Page, Page, Area>>(
                new Dictionary<AreaType, Func<Flags, Party, Page, Page, Area>>() {
                    { AreaType.TINY_WOODS, (flags, party, camp, dungeonPages) => CreateRuins(flags, party, camp, dungeonPages) }
        });

        private static Area CreateRuins(Flags flags, Party party, Page camp, Page quests) {
            return new Area(AreaType.TINY_WOODS,
                    new Stage[] {
                        GetSampleScene(party),
                        new BattleStage(
                            "Start of adventure",
                            () => new Encounter[] {
                                new Encounter(FieldNPCs.Villager()),
                                new Encounter(FieldNPCs.Villager(), FieldNPCs.Villager())
                            }),
                        new BattleStage(
                            "Stronger monsters",
                            () => new Encounter[] {
                                new Encounter(FieldNPCs.Villager(), FieldNPCs.Villager()),
                                new Encounter(FieldNPCs.Villager(), FieldNPCs.Knight())
                            }),
                        new BattleStage(
                            "Restoration",
                            () => new Encounter[] {
                                new Encounter(FieldNPCs.Healer(), FieldNPCs.Healer()),
                                new Encounter(FieldNPCs.Healer(), FieldNPCs.Knight(), FieldNPCs.BlackShuck())
                            }),
                        new BattleStage(
                            "VS " + FieldNPCs.BigKnight().Look.Name,
                            () => new Encounter[] {
                                new Encounter(Music.BOSS, FieldNPCs.Healer(), FieldNPCs.BigKnight(), FieldNPCs.Healer())
                            }),
                        new BattleStage(
                            "Ancient Magicks",
                            () => new Encounter[] {
                                new Encounter(FieldNPCs.Wizard()),
                                new Encounter(FieldNPCs.Wizard(), FieldNPCs.Wizard())
                            }),
                        new BattleStage(
                            "The Replicant",
                            () => new Encounter[] {
                                new Encounter(Music.CREEPY, FieldNPCs.Healer(), FieldNPCs.Replicant(), FieldNPCs.Healer())
                            }),
                    },
                    new PageGroup[] { FieldNPCs.AppleDealer(camp, flags, party) }
                );
        }

        private static SceneStage GetSampleScene(Party party) {
            Character hero = party.GetCharacter(c => c.HasFlag(Flag.HERO));
            Character partner = party.GetCharacter(c => c.HasFlag(Flag.PARTNER));
            Page page = new Page("Test Location");

            SceneStage scene = new SceneStage(
                page,
                "Scene example",
                new TextAct(hero, Side.LEFT, "I will now appear on the LEFT side."),
                new ActionAct(() => page.AddCharacters(Side.LEFT, hero)),
                new TextAct(hero, Side.LEFT, "Wow! Amazing!!!"),
                new TextAct(partner, Side.RIGHT, "I will now appear on the RIGHT side."),
                new ActionAct(() => page.AddCharacters(Side.RIGHT, partner)),
                new TextAct(partner, Side.RIGHT, "Neato!"),
                new CoroutineAct(SFX.DoMeleeEffect(hero, partner, 1.0f, "Slash_0")),
                new TextAct(partner, Side.RIGHT, "Ouch."),
                new TextAct(hero, Side.LEFT, "<color=lime>Wow</color> <color=red>look</color> <color=magenta>at</color> <color=green>this</color> <color=cyan>colored</color> <color=yellow>text</color>!")
                );

            return scene;
        }
    }
}