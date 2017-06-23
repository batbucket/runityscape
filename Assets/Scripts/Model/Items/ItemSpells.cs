using Scripts.Game.Defined.Spells;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using System.Collections.Generic;

namespace Scripts.Model.Items {
    public class CastEquipItem : ItemSpellbook {
        private EquippableItem equip;

        public CastEquipItem(EquippableItem equip) : base(equip) {
            this.equip = equip;
        }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return string.Format("{0}\n\nEquip [{1}] in a target's [{2}] equipment slot.", equip.Description, equip.Name, equip.Type);
        }

        protected override bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return !target.Equipment.Contains(equip.Type) || caster.Inventory.IsAddable(target.Equipment.PeekItem(equip.Type));
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new EquipItemEffect(new EquipParams(caster.Inventory, target.Equipment, equip), new Buffs.BuffParams() { Caster = caster.Stats, CasterId = caster.CharacterId })
                };
        }
    }

    public class CastUnequipItem : ItemSpellbook {
        private Inventory caster;
        private Equipment targetEq;
        private new EquippableItem item;

        public CastUnequipItem(Inventory caster, Equipment targetEq, EquippableItem item) : base(item) {
            this.caster = caster;
            this.targetEq = targetEq;
            this.item = item;
        }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return string.Format("{0}\n\nUnequip [{1}] from the [{2}] equipment slot.", item.Description, item.Name, item.Type);
        }

        protected override bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return target.Equipment.Contains(item.Type) || caster.Inventory.IsAddable(item);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new EquipItemEffect(new EquipParams(caster.Inventory, target.Equipment, item), new Buffs.BuffParams() { Caster = caster.Stats, CasterId = caster.CharacterId})
                };
        }
    }

    public class UseItem : ItemSpellbook {
        private readonly ConsumableItem consume;

        public UseItem(ConsumableItem consume) : base(consume) {
            this.consume = consume;
        }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return string.Format("{0}\n\nUse [{1}] on a target.", item.Description, item.Name);
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
    }
}