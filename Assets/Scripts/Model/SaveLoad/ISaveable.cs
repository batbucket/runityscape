namespace Scripts.Model.SaveLoad {

    /// <summary>
    /// Interface used to facillitate saving and loading of game objects.
    /// To be inherited by the class to be saved in a serializable form.
    /// </summary>
    /// <typeparam name="S">Implementing class type</typeparam>
    public interface ISaveable<S> {
        S GetSaveObject();
        void InitFromSaveObject(S saveObject);
    }
}