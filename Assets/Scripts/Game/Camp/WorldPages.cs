using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Game.Pages {
    public class WorldPages : PageGroup {
        public WorldPages(Page previous) : base(new Page("World")) {
            Root.Actions = new IButtonable[] { PageUtil.GenerateBack(previous) };
        }
    }
}