using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PageGenerator : IButtonable {
    private Action<Page> pageSetFunc;
    private string name;
    private string description;
    private IList<Encounter> encounters;

    public PageGenerator(Action<Page> pageSetFunc,
                         string name,
                         string description,
                         params Encounter[] encounters) {
        this.pageSetFunc = pageSetFunc;
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
        float totalSum = 0;

        // Sum up weights
        foreach (Encounter e in encounters) {

            // If any encounter's override condition is true, set page to that encounter.
            if (e.overrideCondition.Invoke()) {
                pageSetFunc.Invoke(e.page.Invoke());
                return;
            }

            totalSum += e.weight.Invoke();
        }

        float random = UnityEngine.Random.Range(0, totalSum);

        float cumulSum = 0;
        foreach (Encounter e in encounters) {
            cumulSum += e.weight.Invoke();

            if (random <= cumulSum) {
                pageSetFunc.Invoke(e.page.Invoke());
                return;
            }
        }
        Util.Assert(false, "Unable to generate an encounter!");
    }
}
