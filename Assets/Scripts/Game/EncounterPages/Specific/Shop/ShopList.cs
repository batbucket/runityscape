using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Serialized.Items.Consumables;
using Scripts.Game.Defined.Serialized.Items.Equipment;
using Scripts.Game.Serialized;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Game.Pages {
    public static class ShopList {
        public static Shop Ruins(Page previous, Flags flags, Party party) {
            Character maple = CharacterList.Ruins.Maple();
            Shop shop = new Shop(
                previous,
                "Maple's McIntosh Market",
                flags,
                party,
                0.33f,
                3f,
                maple
                );
            shop.AddBuys(new Buy(new Apple()))
                .AddOnEnter(
                        () => {
                            if (flags.Ruins == RuinsProgression.FIRST_VISIT) {
                                flags.Ruins = RuinsProgression.ENCOUNTER_MAPLE;
                                ActUtil.SetupScene(
                                    shop.Root,
                                    ActUtil.LongTalk(shop.Root, maple,
                                    "<t>You come across a woman covered in white desert robes and a hood lying on her back. Tiny cuts cover the front of the robes, making it slightly bloody.<t>From the opening in her hood, as well as her exposed hands, you see that her skin is a dark brown.<t>A massive backpack is right next to her, with a sign hooked onto it.<a>Money is in the top pouch...<t>She taps on the sign which reads:\n\"Maple's McIntosh Market.\"\n(Her prices are listed below it.)<a>Take one more or put in one less and I'll put you on the list...")
                                    );
                            }
                        })
                 .AddTalks(
                    new Talk("Her injuries", "<a>A <color=magenta>crazy fox-lady</color> attacked me with a bunch of her hair-tail... things.<a>I think they're poisonous or venomous or something...<a>I think it'll wear off in a while.").AddSetFlags(f => f.Ruins = RuinsProgression.ASK_ABOUT_INJURIES),
                    new Talk("Fox-lady", "<a>A real messed-up monster.<a>I think <color=magenta>she</color> went in there...<t>Maple points at the entrance of the Cathedral.<a>It's been a few minutes since <color=magenta>she</color> walked in there.<a>Why do you need to know? Are you planning on going in there?")
                        .AddCondition(f => f.Ruins >= RuinsProgression.ASK_ABOUT_INJURIES)
                        .AddSetFlags(f => f.Ruins = RuinsProgression.ASK_ABOUT_FOX)

                );
            return shop;
        }

    }
}