using UnityEngine;
using System.Collections;

public interface IReactable {

    /// <summary>
    /// Called when this Character is affected by a Spell.
    /// </summary>
    /// <param name="s">Spell Character hit by</param>
    /// <param name="r">s's Result</param>
    /// <param name="c">s's Calculation</param>
    void React(Spell s, Result r, Calculation c);

    /// <summary>
    /// Called when any Character in the Page is affected by a Spell.
    /// </summary>
    /// <param name="s">Spell Character hit by</param>
    /// <param name="r">s's Result</param>
    /// <param name="c">s's Calculation</param>
    void Witness(Spell s, Result r, Calculation c);
}
