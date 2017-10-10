using Scripts.Model.Interfaces;
using Scripts.Model.Processes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections.ObjectModel;
using System;

namespace Scripts.Model.Pages {

    /// <summary>
    /// Abstract class for creating groupings of pages.
    /// </summary>
    /// <seealso cref="Scripts.Model.Interfaces.IButtonable" />
    public abstract class PageGroup : IButtonable {
        public const int ROOT_INDEX = 0;

        private Page root;

        private IDictionary<int, Page> dict;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageGroup"/> class.
        /// </summary>
        /// <param name="head">The root page.</param>
        public PageGroup(Page head) {
            this.root = head;
            this.dict = new Dictionary<int, Page>();
            Register(ROOT_INDEX, root);
        }

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public Page Root {
            get {
                return root;
            }
        }

        /// <summary>
        /// Gets the button text.
        /// </summary>
        /// <value>
        /// The button text.
        /// </value>
        public string ButtonText {
            get {
                return root.ButtonText;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is invokable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is invokable; otherwise, <c>false</c>.
        /// </value>
        public bool IsInvokable {
            get {
                return Root.IsInvokable;
            }
        }

        /// <summary>
        /// Gets the tooltip text.
        /// </summary>
        /// <value>
        /// The tooltip text.
        /// </value>
        public string TooltipText {
            get {
                return root.TooltipText;
            }
        }

        /// <summary>
        /// Gets the sprite.
        /// </summary>
        /// <value>
        /// The sprite.
        /// </value>
        public Sprite Sprite {
            get {
                return root.Sprite;
            }
        }

        /// <summary>
        /// Gets all pages.
        /// </summary>
        /// <value>
        /// All pages.
        /// </value>
        protected ReadOnlyCollection<Page> AllPages {
            get {
                return new ReadOnlyCollection<Page>(dict.Values.ToArray());
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is visible on disable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is visible on disable; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisibleOnDisable {
            get {
                return root.IsVisibleOnDisable;
            }
        }

        /// <summary>
        /// Registers the specified identifier with the page.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="page">The page.</param>
        protected void Register(int id, Page page) {
            Util.Assert(!dict.ContainsKey(id), "ID already registered.");
            dict.Add(id, page);
        }

        /// <summary>
        /// Gets the page associated with index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected Page Get(int index) {
            Util.Assert(dict.ContainsKey(index));
            return dict[index];
        }

        /// <summary>
        /// Gets a basic page that can return to a previous page.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="previousIndex">Index of the previous.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns></returns>
        protected Page BasicPage(int index, int previousIndex, params IButtonable[] buttons) {
            Page page = Get(index);
            IButtonable[] list = new IButtonable[buttons.Length + 1];
            list[0] = PageUtil.GenerateBack(Get(previousIndex));
            for (int i = 1; i < list.Length; i++) {
                list[i] = buttons[i - 1];
            }
            page.Actions = list;
            return page;
        }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        public void Invoke() {
            root.Invoke();
        }
    }
}