using Scripts.Game.Defined.Serialized.Items;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;

namespace Scripts.Game.Shopkeeper {

    public class InventoryMaster : PageGroup {
        private const int INVENTORY_UPGRADE_AMOUNT = 1;
        private readonly int maxCapacityThatCanBeUpgradedTo;
        private readonly int minCapacityNeededToUpgrade;
        private readonly int pricePerUpgrade;

        public InventoryMaster(
            Page previous,
            Party party,
            Character person,
            int minCapacityNeededToUpgrade,
            int maxCapacityThatCanBeUpgradedTo,
            int pricePerUpgrade)
            : base(new Page("Inventory Master")) {
            this.maxCapacityThatCanBeUpgradedTo = maxCapacityThatCanBeUpgradedTo;
            this.minCapacityNeededToUpgrade = minCapacityNeededToUpgrade;
            this.pricePerUpgrade = pricePerUpgrade;

            Root.AddCharacters(Side.LEFT, party);
            Root.AddCharacters(Side.RIGHT, person);
            Root.Actions = new IButtonable[] {
                PageUtil.GenerateBack(previous),
                GetInventoryExpanderProcess(Root, party.Shared)
            };
            Root.OnEnter = () => {
                Root.AddText(party.Shared.WealthText);
            };
        }

        private Process GetInventoryExpanderProcess(Page current, Inventory inventory) {
            return new Process(
                "Upgrade",
                Util.GetSprite("upgrade"),
                string.Format("Upgrade inventory capacity from {0} to {1}.\nCosts {2} {3}s.\nRequires at least {4} capacity.\nInventory cannot be upgraded past {5} capacity.",
                inventory.Capacity,
                inventory.Capacity + INVENTORY_UPGRADE_AMOUNT,
                pricePerUpgrade,
                Money.NAME,
                minCapacityNeededToUpgrade,
                maxCapacityThatCanBeUpgradedTo),
                () => {
                    inventory.Capacity += INVENTORY_UPGRADE_AMOUNT;
                    current.AddText(string.Format("Inventory capacity upgraded to {0}.\n{1}", inventory.Capacity, inventory.WealthText));
                    if (!Util.IS_DEBUG) {
                        inventory.Remove(new Money(), pricePerUpgrade);
                    }
                },
                () => IsInventoryUpgradable(inventory.Capacity) && (Util.IS_DEBUG || inventory.HasItem(new Money(), pricePerUpgrade))
                );
        }

        private bool IsInventoryUpgradable(int currentInventoryCapacity) {
            return currentInventoryCapacity >= minCapacityNeededToUpgrade
                && (currentInventoryCapacity + INVENTORY_UPGRADE_AMOUNT) <= maxCapacityThatCanBeUpgradedTo;
        }
    }
}