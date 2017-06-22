namespace Scripts.Model.SaveLoad {

    /// <summary>
    /// Interface used to facillitate saving and loading of game objects.
    /// To be inherited by the class to be saved in a serializable form.
    /// </summary>
    /// <typeparam name="S">Object to save to</typeparam>
    /// <typeparam name="L">Object to load from</typeparam>
    public interface ISaveable<S, L> {
        S GetSaveObject();
        void InitFromSaveObject(L saveObject);
    }
}