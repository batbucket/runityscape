using Scripts.Model.Items.Named;
using Scripts.Model.Spells.Named;
using Scripts.Model.Stats.Resources;
using UnityEngine;

namespace Scripts.Model.Characters.Named {

    /// <summary>
    /// Test character used in Debug mode.
    /// </summary>
    public class Amit : PlayerCharacter {

        public Amit() : base(new Displays { Loc = "fox-head", Name = "Amit", Color = Color.white, Check = "Yourself" }, new StartStats { Lvl = 2, Str = 1, Agi = 1, Int = 1, Vit = 1 }) {
            Side = false;
            AddResource(new NamedResource.Skill());
            AddResource(new NamedResource.Experience());
            this.Attack = new Attack();
            this.Spells.Add(new Meditate());
            this.Actions.Add(new Check());
            this.Inventory.Add(new Lobster());
            this.Inventory.Add(new Lobster());
            this.Inventory.Add(new Scimitar());
            this.Flees.Add(new Spare());
            this.Spells.Add(new Smite());
            this.Spells.Add(new Poison());
            this.Spells.Add(new Petrify());
        }
    }
}