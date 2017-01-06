using Scripts.Model.Pages;
using System;

namespace Scripts.Model.Pages {

    public class BattleResult {
        public readonly Page Page;
        public readonly Action Action;

        public BattleResult(Page page, Action action = null) {
            this.Page = page;
            this.Action = action ?? (() => { });
        }
    }
}