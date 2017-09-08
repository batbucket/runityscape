using Scripts.Game.Defined.Serialized.Items.Misc;
using Scripts.Game.Serialized;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Items;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using Scripts.Model.TextBoxes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Pages {
    public class Shop : PageGroup {
        private static readonly Money MONEY = new Money();

        private Flags flags;
        private Party party;
        private float sellPriceModifier;
        private float buyPriceModifier;
        private ICollection<Talk> talks;
        private ICollection<Buy> buys;
        private Character shopkeeper;
        private Page previous;

        public Shop(Page previous, string name, Flags flags, Party party, float sellPriceModifier, float buyPriceModifier, Character shopkeeper) : base(new Page(name)) {
            this.previous = previous;
            this.flags = flags;
            this.party = party;
            this.sellPriceModifier = sellPriceModifier;
            this.buyPriceModifier = buyPriceModifier;
            this.shopkeeper = shopkeeper;
            this.talks = new List<Talk>();
            this.buys = new List<Buy>();
            Root.AddCharacters(Side.LEFT, party.Collection);
            Root.AddCharacters(Side.RIGHT, shopkeeper);
            SetupMenus();
        }

        public Shop AddOnEnter(Action action) {
            Root.OnEnter += action;
            return this;
        }

        public Shop AddBuys(params Buy[] buys) {
            foreach (Buy buy in buys) {
                this.buys.Add(buy);
            }
            return this;
        }

        public Shop AddTalks(params Talk[] talks) {
            foreach (Talk talk in talks) {
                this.talks.Add(talk);
            }
            return this;
        }

        private bool CanBuy(Item item) {
            return party.Shared.HasItem(MONEY, GetFullBuyPrice(item)) && party.Shared.IsAddable(item);
        }

        private int GetFullSellPrice(Item item) {
            return Mathf.Max((int)(item.BasePrice * sellPriceModifier), 1);
        }

        private void AddToMoney(int amount) {
            if (amount > 0) {
                party.Shared.Add(MONEY, amount);
            } else if (amount < 0) {
                party.Shared.Remove(MONEY, -amount);
            }
        }

        private int GetFullBuyPrice(Item item) {
            return Mathf.Max((int)(item.BasePrice * buyPriceModifier), 1);
        }

        private bool CanSell(Item item) {
            return party.Shared.HasItem(item) && item.BasePrice > 0;
        }

        private int GetPartyMoneyCount() {
            return party.Shared.GetCount(MONEY);
        }

        private void SetupMenus() {
            Page p = Get(ROOT_INDEX);
            Grid main = new Grid("Shop Menu");
            main.OnEnter = () => {
                main.List.Clear();
                main.List.Add(SetupTalkMenu(main));
                main.List.Add(SetupSellMenu(main));
                main.List.Add(SetupBuyMenu(main));
                main.List.Add(null);
                main.List.Add(PageUtil.GenerateItemsGrid(false, p, main, new SpellParams(party.Default, p), PageUtil.GetOutOfBattlePlayableHandler(p)));
                main.List.Add(PageUtil.GenerateEquipmentGrid(main, new SpellParams(party.Default, p), PageUtil.GetOutOfBattlePlayableHandler(p), false));
                main.List.Add(null);
                main.List.Add(PageUtil.GenerateBack(previous));
            };
            p.OnEnter += () => {
                main.Invoke();
            };
        }

        private void PostMoneyAmount() {
            Get(ROOT_INDEX).AddText(string.Format("{0}s: {0}", MONEY.Name, party.Shared.GetCount(MONEY)));
        }

        private Grid SetupShopMenu<T>(IButtonable previous, string name, string spriteLoc, string tooltip, IEnumerable<T> items, Func<T, Action, Process> conversion) {
            Grid grid = new Grid(name);
            grid.Icon = Util.GetSprite(spriteLoc);
            grid.Tooltip = tooltip;
            grid.OnEnter = () => {
                grid.List.Clear();
                grid.List.Add(PageUtil.GenerateBack(previous));
                foreach (T item in items) {
                    grid.List.Add(conversion(item, () => grid.Invoke()));
                };
            };
            return grid;
        }

        private Grid SetupBuyMenu(IButtonable previous) {
            return SetupShopMenu<Buy>(previous, "Buy", "buy-card", string.Format("Purchase items with {0}s.", MONEY.Name), buys, (b, a) => GetBuyProcess(b, a));
        }

        private Process GetBuyProcess(Buy buy, Action postBuy) {
            return new Process(
                string.Format("{0}-{1}", buy.Name, GetFullBuyPrice(buy.GetItem())),
                buy.Sprite,
                string.Format("{0}\nPurchase this item for {1} {2}(s).", buy.Description, GetFullBuyPrice(buy.GetItem()), MONEY.Name),
                () => {
                    Item item = buy.GetItem();
                    AddToMoney(-GetFullBuyPrice(item));
                    party.Shared.Add(item);
                    Root.AddText(string.Format("Purchased {0} for {1} {2}(s).\n\nInventory: ({3})\n{2}s now: {4}",
                        item.Name,
                        GetFullBuyPrice(item).Color(Color.red),
                        MONEY.Name,
                        party.Shared.Fraction,
                        GetPartyMoneyCount()));
                    postBuy();
                },
                () => CanBuy(buy.GetItem())
                );
        }

        private Grid SetupSellMenu(IButtonable previous) {
            return SetupShopMenu<Item>(previous, "Sell", "sell-card", string.Format("Sell items for {0}s.", MONEY.Name), party.Shared, (i, a) => GetSellProcess(i, a));
        }

        private Process GetSellProcess(Item item, Action postSell) {
            return new Process(
                string.Format("{0}",
                    party.Shared.CountedItemName(item),
                    !CanSell(item) ?
                    string.Empty :
                    string.Format("-{0}", GetFullSellPrice(item))),
                item.Icon,
                !CanSell(item) ?
                "This item cannot be sold." :
                string.Format("{0}\nSell this item for {1} {2}(s).", item.Description, GetFullSellPrice(item), MONEY.Name),
                () => {
                    AddToMoney(GetFullSellPrice(item));
                    party.Shared.Remove(item);
                    Root.AddText(string.Format("Sold {0} for {1} {2}(s).\n\nInventory: ({3})\n{2}s now: {4}",
                        item.Name,
                        GetFullSellPrice(item).Color(Color.green),
                        MONEY.Name,
                        party.Shared.Fraction,
                        GetPartyMoneyCount()));
                    postSell();
                },
                () => CanSell(item)
                );
        }

        private Grid SetupTalkMenu(IButtonable previous) {
            return SetupShopMenu<Talk>(previous, "Talk", "talk", "Talk to the shopkeeper about something.", talks, (t, a) => GetTalkProcess(t, a));
        }

        private Process GetTalkProcess(Talk talk, Action postTalk) {
            return new Process(
                talk.Name,
                Util.GetSprite("talk"),
                string.Format("Talk about this topic.", talk.Name),
                () => {
                    ActUtil.SetupScene(
                        ActUtil.LongTalk(
                            Get(ROOT_INDEX),
                            shopkeeper,
                            talk.Big
                        ),
                            () => talk.SetFlags(flags),
                            () => postTalk()
                        );
                },
                () => talk.Condition(flags)
                ).SetVisibleOnDisable(false);
        }

    }
}