using Scripts.Game.Defined.Characters;
using Scripts.Game.Undefined.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;

namespace Scripts.Game.Pages {
    public class CreditsPages : PageGroup {
        public CreditsPages(Page previous) : base(new Page("Credits")) {
            CreditsPage(previous);
        }

        private void CreditsPage(Page previous) {
            Page page = Root;

            Root.Actions = new IButtonable[] { PageUtil.GenerateBack(previous) };
            page.Icon = Util.GetSprite("person");

            //// Characters
            //page.AddCharacters(Side.LEFT, new CreditsDummy(Breed.CREATOR, 5, "eternal", "spell-book", "programmer, design, and writing.\nlikes lowercase a little <i>too</i> much."));
            //page.AddCharacters(Side.LEFT, new CreditsDummy(Breed.TESTER, 5, "Duperman", "shiny-apple", "Explorer of nozama."));
            //page.AddCharacters(Side.LEFT, new CreditsDummy(Breed.TESTER, 99, "Rohan", "swap-bag", "Best hunter in the critically acclaimed game\n\'Ace Prunes 3\'"));
            //page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.TESTER, 5, "Vishal", "round-shield", "Hacked the save file to give himself 2,147,483,647 gold in an attempt to buy the tome."));
            //page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.TESTER, 5, "One of Vishal's friends", "hourglass", "Got Vitality nerfed to give 2 health, from 10."));
            //page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.TESTER, 5, "cjdudeman14", "round-shield", "Open beta tester. Bug slayer. Attempted to kill that which is unkillable."));
            //page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.COMMENTER, 5, "UnserZeitMrGlinko", "gladius", "\"more talking!﻿\" ~UZMG"));

            //page.OnEnter += () => {
            //    page.AddText(
            //        "<Tools>\nMade with Unity and the NUnit testing framework.",
            //        "<Music>\nFrom OpenGameArt, Trevor Lentz, cynicmusic.com",
            //        "<Sound Effects>\nSourced from Freesound, SoundBible, and OpenGameArt.",
            //        "<Icons>\nSourced from http://Game-icons.net.",
            //        "<Fonts>\nMain: BPmono by George Triantafyllakos\nTextboxes: Anonymous Pro by Mark Simonson\nHitsplat: n04b by 04\nHotkey: PKMN-Mystery-Dungeon by David Fens"
            //        );
            //};
        }
    }
}