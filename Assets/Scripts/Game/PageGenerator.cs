using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PageGenerator : IButtonable {
    public IList<Encounter> Encounters {
        get {
            return encounters;
        }
    }

    private string name;
    private string description;
    private IList<Encounter> encounters;

    public PageGenerator(string name,
                         string description,
                         params Encounter[] encounters) {
        this.name = name;
        this.description = description;
        this.encounters = new List<Encounter>();
        this.AddEncounters(encounters);
    }

    public void AddEncounter(Encounter encounter) {
        this.encounters.Add(encounter);
    }

    public void AddEncounters(params Encounter[] encounters) {
        foreach (Encounter e in encounters) {
            this.encounters.Add(e);
        }
    }

    public string ButtonText {
        get {
            return name;
        }
    }

    public bool IsInvokable {
        get {
            return true;
        }
    }

    public bool IsVisibleOnDisable {
        get {
            return true;
        }
    }

    public string TooltipText {
        get {
            return description;
        }
    }

    public void Invoke() {
        IList<Encounter> enabledEncounts = new List<Encounter>();

        // Add enabled encounters into pool
        foreach (Encounter e in encounters) {
            if (!e.IsDisabled) {
                enabledEncounts.Add(e);
            }
        }

        // Sum up weights
        float totalSum = 0;
        foreach (Encounter e in encounters) {

            // If any encounter's override condition is true, set page to that encounter.
            if (e.IsOverride && !e.IsDisabled) {
                Game.Instance.CurrentPage = e.page.Invoke();
                return;
            }

            totalSum += e.weight.Invoke();
        }

        float random = UnityEngine.Random.Range(0, totalSum);

        float cumulSum = 0;
        foreach (Encounter e in encounters) {
            cumulSum += e.weight.Invoke();

            if (random <= cumulSum) {
                Game.Instance.CurrentPage = e.page.Invoke();
                return;
            }
        }
        Util.Assert(false, "Unable to generate an encounter!");
    }
}
