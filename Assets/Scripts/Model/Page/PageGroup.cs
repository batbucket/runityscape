using Scripts.Model.Interfaces;
using Scripts.Model.Processes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections.ObjectModel;

namespace Scripts.Model.Pages {
    public abstract class PageGroup : IButtonable {
        public const int ROOT_INDEX = 0;

        private Page root;

        private IDictionary<int, Page> dict;

        public PageGroup(Page head) {
            this.root = head;
            this.dict = new Dictionary<int, Page>();
            Register(ROOT_INDEX, root);
        }

        public Page Root {
            get {
                return root;
            }
        }

        public string ButtonText {
            get {
                return root.Location;
            }
        }

        public bool IsInvokable {
            get {
                return true;
            }
        }

        public string TooltipText {
            get {
                return root.TooltipText;
            }
        }

        public Sprite Sprite {
            get {
                return root.Sprite;
            }
        }

        protected ReadOnlyCollection<Page> AllPages {
            get {
                return new ReadOnlyCollection<Page>(dict.Values.ToArray());
            }
        }

        protected void Register(int id, Page page) {
            Util.Assert(!dict.ContainsKey(id), "ID already registered.");
            dict.Add(id, page);
        }

        protected Page Get(int index) {
            Util.Assert(dict.ContainsKey(index));
            return dict[index];
        }

        protected Page BasicPage(int index, int previousIndex, params IButtonable[] buttons) {
            Page page = Get(index);
            IButtonable[] list = new IButtonable[buttons.Length + 1];
            list[0] = Get(previousIndex);
            for (int i = 1; i < list.Length; i++) {
                list[i] = buttons[i - 1];
            }
            page.Actions = list;
            return page;
        }

        public void Invoke() {
            root.Invoke();
        }
    }
}