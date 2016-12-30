using System.Collections.Generic;

public class Shop : ReadPage {
    private Flags flags;
    private Shopkeeper shopkeep;
    private Page camp;
    private Party party;
    private IList<Process> talks;


    public Shop(Flags flags, Page camp, Party party) : base(party, "", "", "", "Ruins", null, new PageActions { }) {
        this.flags = flags;
        this.shopkeep = new Shopkeeper(flags);
        this.camp = camp;
        this.party = party;
        AddCharacters(true, shopkeep);
        OnEnterAction += () => {
            Choose();
            if (!flags.Bools[Flag.SHOPKEEPER_GAVE_NAME]) {
                Game.Instance.Cutscene(false, Event.LongTalk(shopkeep,
                    ">You come across a woman in white desert clothing, complete with a loose hood. She seems to have no issues carrying what appears to be a massive backpack full of goods behind her despite being a head shorter than you.>From the opening in her hood you see she has brown hair, eyes, and skin. There is no doubt she spends a lot of time in the desert./Hey, are you gonna stare at me all day or are you gonna buy--/Oh, you're a new face around these parts, huh? I haven't seen you before./Actually, I think you're the first person I've seen in these parts./Ever./The name's Maple. Professional fruit dealer of the plateau./Don't want fruit? I also have non-fruit things on sale. Like a book, and other junk./Get your goods today at Maple's wandering emporium of fruit and non-fruit goods."),
                    new Event(() => flags.Bools[Flag.SHOPKEEPER_GAVE_NAME] = true)
                    );
            } else {
                Game.Instance.Cutscene(true,
                    new Event(shopkeep.Talk(
                        "Whaddya need?",
                        "Need something?",
                        "The best... and only fruit dealer on this plateau.",
                        "Fruit and fruit accessories.",
                        "All goods are at market price."
                        ))
                    );
            }
        };
    }

    private void Choose() {
        this.ClearActionGrid();
        ActionGrid[0] = new Process("Buy", "", () => Buy());
        ActionGrid[1] = new Process("Sell", "", () => Sell(), () => !flags.Bools[Flag.SHOPKEEPER_NO_SELL]);
        ActionGrid[2] = new Process("Talk", "", () => Talk());
        ActionGrid[3] = new Process("Fight", "", () => Fight());

        ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 2] = new ItemManagePage(this, party);
        ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = camp;
    }

    private void Buy() {
        this.ClearActionGrid();
        ActionGrid[0] = BuyItem(new Apple(), 5);
        ActionGrid[1] = BuyItem(new OldSword(), 15);
        ActionGrid[2] = BuyItem(new OldArmor(), 15);
        ActionGrid[3] = BuyItem(new SpellTome(new Smite()), int.MaxValue);

        ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "", () => Choose());
    }

    private Process BuyItem(Item i, int cost) {
        Inventory inven = party.Main.Items;
        return new Process(
            string.Format("{0} - {1}g", i.Name, cost),
            string.Format("{0} - {1}<color=yellow>g</color>\n{2}", i.Name, cost, i.Description),
            () => {
                inven.Gold -= cost;
                inven.Add(i);
                Game.Instance.TextBoxes.AddTextBox(
                    new TextBox(
                        string.Format("{0} was purchased for {1}<color=yellow>g</color>.\n{2}", i.Name, cost, party.Main.Items.SizeText)
                        )
                        );
            },
            () => !inven.IsFull && inven.Gold >= cost
            );
    }

    private void Sell() {
        Game.Instance.Cutscene(false, Event.LongTalk(shopkeep,
                "/Uhh yeah, I don't wanna buy anything from you./Not because I don't like you./Gotta keep my net profits up, ya know?/And I can't do that if I buy everything everyone tries to sell me./Feel free to free up your inventory by tossing items, though."
            ),
            new Event(() => flags.Bools[Flag.SHOPKEEPER_NO_SELL] = true)
            );
    }

    private void Talk() {
        this.ClearActionGrid();
        ActionGrid[0] = new Process("Plateau?", "", () =>
            Game.Instance.Cutscene(false, Event.LongTalk(shopkeep,
            "/This entire place is a plateau./Nobody that I know of has ever survived the descent--the cliffs are too high and steep./Some people see stars, others see ghosts./What do I see when I look down a cliff? Just black... nothingness./Unless if someone sprouts wings, goes down there, and returns to tell the tale, I guess we'll never know what's down there."
            ))
        );

        ActionGrid[4] = new Process("Where are you from?", "", () =>
        Game.Instance.Cutscene(false, Event.LongTalk(shopkeep,
            "/All the way across the desert.>{0} points./That's as specific as I'll get. Don't wanna get murdered, after all./Yep, that's happened before. Someone followed me home and tried to murder me./I cross the desert all the time to to maximize my market area./Cuts a lot of the distance. That saves time. And as the saying goes..."),
            new Event(() => flags.Bools[Flag.SHOPKEEPER_MENTIONED_MURDER] = true)
            ));

        ActionGrid[8] = new Process("Why are you so dark?", "", () =>
            Game.Instance.Cutscene(false, Event.LongTalk(shopkeep,
            "/Gets pretty boring, doing shopkeeper stuff all day. Gotta keep things interesting./Or did you mean that literally?/This desert outfit is a new buy./Guess what happens when you lug a bunch of stuff around in the sun without any protection?")
        ), () => flags.Bools[Flag.SHOPKEEPER_MENTIONED_MURDER], false);


        ActionGrid[1] = new Process("What kind of name is Maple?", "", () =>
            Game.Instance.Cutscene(false, Event.LongTalk(shopkeep, "/You know, the tree? Maple trees? Maple wood? People make bows out of the stuff./Someone had some high hopes for my archery abilities./Seeing how I'm currently peddling goods off, you can probably guess that I'm not very good at marksmanship./My sister, Willow, is pretty good at shooting./But she uses one of those mechanical bows. So I guess it doesn't count."),
            new Event(() => flags.Ints[Flag.SHOPKEEPER_MENTIONED_SISTER] = 1)));

        ActionGrid[5] = new Process("Ask about Willow", "", () =>
            Game.Instance.Cutscene(false, Event.LongTalk(shopkeep,
            "/Also named after a tree./She's a bounty hunter of sorts. She makes money taking down crooks./Currently going after a rogue Keeper./When she's done with that, let's just say I won't be shopkeeping anymore."),
            new Event(() => flags.Bools[Flag.SHOPKEEPER_MENTIONED_KEEPER] = true)),
            () => flags.Bools[Flag.SHOPKEEPER_MENTIONED_SISTER], false);

        ActionGrid[9] = new Process("Ask about the Keeper", "", () =>
            Game.Instance.Cutscene(false, Event.LongTalk(shopkeep, "/Keepers are very, very, bad and powerful monsters in charge of sowing corruption across the land./Whether that's literally or metaphorically, I don't know./And there's a rogue Keeper on the loose. That probably means that they're doubly very, very bad and powerful.")),
            () => flags.Bools[Flag.SHOPKEEPER_MENTIONED_KEEPER], false);

        ActionGrid[3] = new Process("Why is your Tome of Smite so expensive?", "", () =>
        Game.Instance.Cutscene(false, Event.LongTalk(shopkeep,
        "/I've only got a few of those./They don't grow on trees, ya know./And I have just the feeling that they're high in demand./Some call it price gouging./I call it a good way to make some serious money.")));

        ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "", () => Choose());
    }

    private void Fight() {

    }
}
