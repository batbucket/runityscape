/**
 * This interface describes any entity that can
 * participate in a battle
 */
public interface Battler {
    void act(TextBoxHolderManager textBoxHolderManager); //Perform an action, typically consuming CT
    void react(BattleProcess process);
    void onStart();
    bool isDefeated();
    void onDefeat();
    bool isKilled();
    void onKill();
    int getResource(ResourceType resource);
}
