using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public struct SpellSelect : IButtonable {

    public string ButtonText {
        get {
            StringBuilder s = new StringBuilder();
            s.Append((spell.IsCastable(caster) ? spell.Name : Util.Color(spell.Name, Color.red)) + (spell.Costs.Count == 0 ? "" : " - "));
            List<string> elements = new List<string>();
            foreach (KeyValuePair<ResourceType, int> entry in spell.Costs) {
                if (entry.Key != ResourceType.CHARGE) {
                    Color resourceColor = entry.Key.FillColor;
                    int cost = entry.Value;
                    elements.Add(Util.Color("" + cost, resourceColor));
                }
            }
            s.Append(string.Join("/", elements.ToArray()));
            return s.ToString();
        }
    }

    public bool IsInvokable {
        get {
            return spell.IsCastable(caster);
        }
    }

    public bool IsVisibleOnDisable {
        get {
            return true;
        }
    }

    public string TooltipText {
        get {
            IList<string> s = new List<string>();
            foreach (KeyValuePair<ResourceType, int> pair in spell.Costs) {
                s.Add(
                        string.Format(
                            "{0} {1}",
                            Util.Color("" + pair.Value, (caster.GetResourceCount(pair.Key, false) < pair.Value) ? Color.red : Color.white),
                            Util.Color(pair.Key.Name, pair.Key.FillColor)
                        )
                        );
            }
            string costText = string.Join(", ", s.ToArray());

            return String.Format(
                "{0}{1}\n{2}",
                spell.Name,
                string.IsNullOrEmpty(costText) ? "" : " - " + costText,
                spell.Description
                );
        }
    }

    private Character caster;
    private SpellFactory spell;
    private Action a;

    public SpellSelect(Character caster, SpellFactory spell, Action a) {
        this.caster = caster;
        this.spell = spell;
        this.a = a;
    }

    public void Invoke() {
        a.Invoke();
    }

}
