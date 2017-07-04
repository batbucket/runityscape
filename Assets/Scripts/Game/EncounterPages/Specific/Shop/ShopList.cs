using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Serialized.Items.Consumables;
using Scripts.Game.Defined.Serialized.Items.Equipment;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Game.Pages {
    public static class ShopList {
        public static Shop Ruins(Page previous, Flags flags, Party party) {
            return new Shop(
                previous,
                "Maple's McIntosh Market",
                flags,
                party,
                0.33f,
                3f,
                CharacterList.Ruins.Maple()
                )
                .AddBuy(new Buy(new Apple()).AddPitch("Test description"))
                .AddBuy(new Buy(new PoisonArmor()).AddPitch("Other description"));
        }

    }
}