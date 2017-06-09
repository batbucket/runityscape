using Scripts.Model.Characters;
using Scripts.Model.Stats;
using Scripts.View.Portraits;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Presenter {

    public class CharacterPresenter {
        public Character Character { get; private set; }
        public PortraitView PortraitView { get; private set; }

        public CharacterPresenter(Character character, PortraitView portraitView) {
            this.Character = character;
            this.PortraitView = portraitView;
        }

        public void Tick() {
            //Attempt to set ResourceViews
            PortraitView.SetResources(Character.Stats.Dict.Keys.Where(k => StatType.RESOURCES.Contains(k)).ToArray());

            //Update ResourceViews' values
            foreach (KeyValuePair<StatType, Stat> pair in Character.Stats.Dict) {
                if (PortraitView != null && PortraitView.ResourceViews.ContainsKey(pair.Key)) {
                    ResourceView rv = PortraitView.ResourceViews[pair.Key].resourceView;
                    StatType resType = pair.Key;
                    Stat res = pair.Value;
                    rv.SetBarScale(res.Mod, res.Max);
                    rv.Text = "" + res.Mod;
                }
            }

            //TODO Buff stuff
            PortraitView.SetBuffs(Character.Stats.Buffs.Collection
                .Select(b =>
                new PortraitView.BuffParams {
                    id = b.GetHashCode(),
                    name = b.Name,
                    description = b.Description,
                    sprite = b.Sprite,
                    duration = b.IsDefinite ? ("" + b.TurnsRemaining) : "inf"
                })
                .ToArray()
                );

            this.PortraitView.Sprite = Character.Look.Sprite;
        }
    }
}