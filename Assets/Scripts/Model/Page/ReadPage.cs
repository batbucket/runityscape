using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Stats.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Pages {

    public class ReadPage : Page {

        public ReadPage(
            string text = "",
            string tooltip = "",
            string location = "",
            bool hasInputField = false,
            IList<Character> left = null,
            IList<Character> right = null,
            PageActions pageActions = new PageActions() { },
            IList<IButtonable> buttonables = null,
            string musicLoc = null
            )
            : base(text, tooltip, location, hasInputField, left, right, pageActions.onFirstEnter, pageActions.onEnter, pageActions.onFirstExit, pageActions.onExit, pageActions.onTick, buttonables, musicLoc) {
        }

        public ReadPage(
        Party party,
        string musicLoc,
        string text,
        string tooltip,
        string location,
        IList<IButtonable> buttonables,
        IList<Character> right = null) : this(party, musicLoc, text, tooltip, location, buttonables, new PageActions() { }, right) { }

        public ReadPage(
        Party party,
        string musicLoc,
        string text,
        string tooltip,
        string location,
        IList<IButtonable> buttonables,
        PageActions pageActions,
        IList<Character> right = null) : this(text, tooltip, location, false, party.Members, right, pageActions, buttonables, musicLoc) {
        }

        public ReadPage(
            Party party,
            string text,
            string location,
            IButtonable oneShot
            ) : this(party, "", text, "", location, new IButtonable[] { oneShot }, null) { }

        public ReadPage(
        Party party,
        string text,
        string location,
        Action a,
        IButtonable oneShot
        ) : this(party, "", text, "", location, new IButtonable[] { oneShot }, new PageActions { onEnter = a }) { }

        protected override void OnAddCharacter(Character c) {
        }

        protected override void OnAnyEnter() {
            GetAll().Where(c => c.HasResource(ResourceType.CHARGE)).ToList().ForEach(c => c.Resources[ResourceType.CHARGE].IsVisible = false);
        }

        protected override void OnAnyExit() {
        }

        protected override void OnFirstEnter() {
        }

        protected override void OnFirstExit() {
        }

        protected override void OnTick() {
        }
    }
}