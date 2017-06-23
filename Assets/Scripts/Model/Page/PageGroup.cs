using System.Collections.Generic;

namespace Scripts.Model.Pages {
    public abstract class PageGroup {
        public const int ROOT_INDEX = 0;

        private Page root;

        private Map<int, Page> map;

        public PageGroup(Page head) {
            this.root = head;
            this.map = new Map<int, Page>();
            Register(ROOT_INDEX, root);
        }

        public Page Root {
            get {
                return root;
            }
        }

        protected void Register(int id, Page page) {
            Util.Assert(!map.Contains(page), "Page already registered.");
            map.Add(id, page);
        }

        protected Page Get(int index) {
            return map.Get(index);
        }
    }
}