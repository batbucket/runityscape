using Scripts.Game.Defined.Characters;
using Scripts.Game.Pages;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Dungeons {
    public static class AreaList {

        /// <summary>
        /// Index of the stage with a miniboss
        /// </summary>
        public const int MINIBOSS_INDEX = 3;
        public static readonly Color MINIBOSS_STAGE_TEXT_COLOR = Color.yellow;

        /// <summary>
        /// Index of the stage with the area's boss
        /// </summary>
        public const int BOSS_INDEX = 7;
        public static readonly Color BOSS_STAGE_TEXT_COLOR = Color.cyan;

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
                    { AreaType.RUINS, (f, p, c, q) => CreateField(f, p, c, q) }
        });

        private static Area CreateField(Flags flags, Party party, Page camp, Page quests) {
            return new Area(flags, party, camp, quests, AreaType.RUINS,
                    new Stage(
                        "Lots of stages",
                        () => new Encounter[] {
                            new Encounter(FieldNPCs.Villager()),
                            new Encounter(FieldNPCs.Villager(), FieldNPCs.Villager())
                        }),
                    new Stage(
                        "Illusionist",
                        () => new Encounter[] {
                            new Encounter(FieldNPCs.Illusionist())
                        }),
                    new Stage(
                        "Big Knight",
                        () => new Encounter[] {
                            new Encounter(FieldNPCs.BigKnight())
                        }),
                    new Stage(
                        "Replicant",
                        () => new Encounter[] {
                            new Encounter(FieldNPCs.Replicant(), FieldNPCs.Healer(), FieldNPCs.Healer())
                        })
                );
        }
    }
}