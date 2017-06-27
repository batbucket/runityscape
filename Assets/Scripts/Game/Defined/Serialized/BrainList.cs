using System;
using System.Collections.Generic;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using UnityEngine;
using Scripts.Model.Characters;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Items;
using Scripts.Game.Defined.Serialized.Spells;

namespace Scripts.Game.Defined.Serialized.Characters {

    public class Player : Brain {
        public static readonly Attack Attack = new Attack();
        private Battle battle;

        public override void DetermineAction(Action<IPlayable> addPlay) {
            Grid main = new Grid("Main");
            SpellParams ownerParams = new SpellParams(Owner);
            main.Array = new IButtonable[] {
                PageUtil.GenerateTargets(battle, main, ownerParams, Attack, addPlay),
                PageUtil.GenerateSpellBooks(battle, main, ownerParams, Attack, ownerParams.Spells, ownerParams.Spells.Count, addPlay),
                PageUtil.GenerateItems(battle, main, ownerParams, null, ownerParams.Inventory, ownerParams.Inventory.UniqueItemCount, addPlay),
                PageUtil.GenerateUnequipGrid(main, ownerParams, addPlay)
            };
            battle.Actions = main.Array;
        }

        protected override void PageSetupHelper(Battle battle) {
            this.battle = battle;
        }
    }

    public class DebugAI : Brain {
        public static readonly Attack Attack = new Attack();

        public override void DetermineAction(Action<IPlayable> addPlay) {
            //addPlay(Spells.CreateSpell(Attack, Owner, foes.PickRandom()));
        }
    }
}