using UnityEngine;
using System.Collections;

public abstract class Spell {

    protected int resourceCost; //cost of spell
    protected ResourceType resourceCostType;
    protected string spellName;  //name of spell
    protected SpellType spellType; //category of spell
    protected string textBoxDescription; //textbox will say this when spell is used in battle
    protected Character user; //person who attacked
    protected Character target; //person who recieves attack
    protected int spellDamage; //damage of spell

    //protected Effect spellEffect; //additional Effect of spell 

    public abstract void stubMethod();

    public void setSpellType(SpellType spellType) { this.spellType = spellType; } //add dash and relavant recourses 
    public SpellType getSpellType() { return spellType; }

    public void setSpellName(string spellName) { this.spellName = spellName; } //add name to spell
    public string getSpellName() { return spellName; }

    public void setResourceCost(int resourceCost) { this.resourceCost = resourceCost; } //quantitatively defines resource cost
    public int getResourceCost() { return resourceCost; }

    public void setResourceCostType(ResourceType resourceCostType) { this.resourceCostType = resourceCostType; } //qualitatively defines resource cost
    public ResourceType getResourceCostType() { return resourceCostType; }

    public void setTextBoxDescription(string textBoxDescription) { this.textBoxDescription = textBoxDescription; } //sets a description for the textbox to describe the attack used
    public string getTextBoxDescription() { return textBoxDescription; }

    
    public Character getUser() { return user; } //defines user of spell
    public void setUser(Character user) { this.user = user; }


    public Character getTarget() { return target; }//defines target of spell 
    public void setTarget(Character target) { this.target = target; }

    public void setSpellDamage(int spellDamage) { this.spellDamage = spellDamage; }
    public int getSpellDamage() { return spellDamage; }

    //public void setSpellEffect(string spellEffect) { this.spellEffect = spellEffect; }
    //public Effect getSpellEffect() { return spellEffect; }



}
