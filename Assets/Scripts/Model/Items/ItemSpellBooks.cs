using Scripts.Game.Defined.Spells;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using System.Collections.Generic;
using Scripts.View.Portraits;
using System.Collections;
using Scripts.Game.Defined.SFXs;
using System;

namespace Scripts.Model.Items {
    public class CastEquipItem : ItemSpellBook {
        private EquippableItem equip;

        public CastEquipItem(EquippableItem equip) : base(equip, "Equip") {
            this.equip = equip;
        }

        protected override bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return caster.Inventory.HasItem(equip) && (!target.Equipment.Contains(equip.Type) || caster.Inventory.IsAddable(target.Equipment.PeekItem(equip.Type)));
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new EquipItemEffect(new EquipParams(caster.Inventory, target.Equipment, equip), new Buffs.BuffParams(caster.Stats, caster.CharacterId))
                };
        }
    }

    public class CastUnequipItem : ItemSpellBook {
        private Inventory caster;
        private Equipment targetEq;
        private new EquippableItem item;

        public CastUnequipItem(Inventory caster, Equipment targetEq, EquippableItem item) : base(item, "Unequip") {
            this.caster = caster;
            this.targetEq = targetEq;
            this.item = item;
        }

        protected override bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return target.Equipment.Contains(item.Type) && caster.Inventory.IsAddable(item);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new UnequipItemEffect(new EquipParams(caster.Inventory, target.Equipment, item))
                };
        }
    }

    public class Dummy : ItemSpellBook {
        public Dummy(BasicItem basic) : base(basic, string.Empty) { }

        protected override string CreateDescriptionHelper() {
            return string.Format("{0}", item.Description);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[0];
        }
    }

    public class UseItem : ItemSpellBook {
        private readonly ConsumableItem consume;

        public UseItem(ConsumableItem consume) : base(consume, "Use") {
            this.consume = consume;
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            IList<SpellEffect> itemEffects = consume.GetEffects(caster, target);
            SpellEffect[] allEffects = new SpellEffect[itemEffects.Count + 1];
            allEffects[0] = new ConsumeItemEffect(consume, caster.Inventory);
            for (int i = 0; i < itemEffects.Count; i++) {
                allEffects[i + 1] = itemEffects[i];
            }
            return allEffects;
        }

        protected override bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return caster.Inventory.HasItem(consume);
        }

        protected override IList<IEnumerator> GetHitSFX(PortraitView caster, PortraitView target) {
            return new IEnumerator[] { SFX.PlaySound("healspell1") };
        }
    }

    public class TossItem : ItemSpellBook {
        private Inventory inventory;

        public TossItem(Item item, Inventory inventory) : base(item, "Dispose") {
            this.inventory = inventory;
        }

        protected override bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return caster.Inventory.HasItem(item);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                new ConsumeItemEffect(item, inventory)
            };
        }
    }
}