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
                    { AreaType.RUINS, (flags, party, camp, dungeonPages) => CreateRuins(flags, party, camp, dungeonPages) },
                    { AreaType.SEA_WORLD, (flags, party, camp, dungeonPages) => SeaWorld(flags, party, camp, dungeonPages) }
        });

        public static readonly ReadOnlyDictionary<AreaType, Sprite> AREA_SPRITES
            = new ReadOnlyDictionary<AreaType, Sprite>(
                new Dictionary<AreaType, Sprite>() {
                    { AreaType.RUINS, Util.GetSprite("skull-crack") },
                    { AreaType.SEA_WORLD, Util.GetSprite("at-sea") }
                });

        private static Area CreateRuins(Flags flags, Party party, Page camp, Page quests) {
            return new Area(
                AreaType.RUINS,
                    new Stage[] {
                        new BattleStage(
                            "Start of adventure",
                            () => new Encounter[] {
                                new Encounter(RuinsNPCs.Villager()),
                                new Encounter(RuinsNPCs.Villager(), RuinsNPCs.Villager())
                            }),
                        new BattleStage(
                            "Stronger monsters",
                            () => new Encounter[] {
                                new Encounter(RuinsNPCs.Villager(), RuinsNPCs.Villager()),
                                new Encounter(RuinsNPCs.Villager(), RuinsNPCs.Knight())
                            }),
                        new BattleStage(
                            "Restoration",
                            () => new Encounter[] {
                                new Encounter(RuinsNPCs.Healer(), RuinsNPCs.Healer()),
                                new Encounter(RuinsNPCs.Healer(), RuinsNPCs.Knight())
                            }),
                        new BattleStage(
                            "Bigger monsters" + RuinsNPCs.BigKnight().Look.Name,
                            () => new Encounter[] {
                                new Encounter(Music.BOSS, RuinsNPCs.Healer(), RuinsNPCs.BigKnight(), RuinsNPCs.Healer())
                            }),
                        new BattleStage(
                            "Ancient Magicks",
                            () => new Encounter[] {
                                new Encounter(RuinsNPCs.Wizard()),
                                new Encounter(RuinsNPCs.Wizard(), RuinsNPCs.Wizard())
                            }),
                        new BattleStage(
                            "Wizards' Tower",
                            () => new Encounter[] {
                                new Encounter(RuinsNPCs.Wizard(), RuinsNPCs.Wizard()),
                                new Encounter(
                                    RuinsNPCs.Wizard(),
                                    RuinsNPCs.Wizard(),
                                    RuinsNPCs.Healer(),
                                    RuinsNPCs.Healer(),
                                    RuinsNPCs.Illusionist())
                            }),
                        new BattleStage(
                            "Premonition",
                            () => new Encounter[] {
                                new Encounter(RuinsNPCs.Villager()),
                                new Encounter(RuinsNPCs.BigKnight(), RuinsNPCs.BigKnight(), RuinsNPCs.Wizard(), RuinsNPCs.Wizard())
                            }),
                        new BattleStage(
                            "The Replicant",
                            () => new Encounter[] {
                                new Encounter(Music.CREEPY, RuinsNPCs.Healer(), RuinsNPCs.Replicant(), RuinsNPCs.Healer())
                            }),
                    },
                    new PageGroup[] { RuinsNPCs.RuinsShop(camp, flags, party) }
                );
        }

        private static Area SeaWorld(Flags flags, Party party, Page camp, Page quests) {
            return new Area(
                    AreaType.SEA_WORLD,
                    new Stage[] {
                        new BattleStage(
                            "Welcome to the ocean",
                            () => new Encounter[] {
                                new Encounter(OceanNPCs.SharkPirate())
                            }),
                        new BattleStage(
                            "Sinister singers",
                            () => new Encounter[] {
                                new Encounter(OceanNPCs.Siren()),
                                new Encounter(OceanNPCs.Siren(), OceanNPCs.SharkPirate())
                            }),
                        new BattleStage(
                            "GitKraken",
                            () => new Encounter [] {
                                new Encounter(Music.BOSS, OceanNPCs.Kraken())
                            }),
                        new BattleStage(
                            "Heart of the Swarm",
                            () => new Encounter[] {
                                new Encounter(OceanNPCs.Swarm(), OceanNPCs.Swarm(), OceanNPCs.Swarm()),
                                new Encounter(OceanNPCs.Swarm(), OceanNPCs.Swarm(), OceanNPCs.Swarm(), OceanNPCs.Swarm()),
                                new Encounter(OceanNPCs.Swarm(), OceanNPCs.Swarm(), OceanNPCs.Swarm(), OceanNPCs.Swarm(), OceanNPCs.Swarm()),
                            }),
                    },
                    new PageGroup[] { OceanNPCs.OceanShop(camp, flags, party) }
                );
        }
    }
}