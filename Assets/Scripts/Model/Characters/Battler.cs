/**
 * This interface describes any entity that can
 * participate in a battle
 */
public interface Battler {
    void act(Game game); //Perform an action, typically consuming CT
    void charge();
    void react(UndoableProcess process, Game game);
    void onStart(Game game);
    bool isDefeated(Game game);
    void onDefeat(Game game);
    bool isKilled(Game game);
    void onKill(Game game);
    Attribute getAttribute(AttributeType attribute);
    Resource getResource(ResourceType resource);
}
