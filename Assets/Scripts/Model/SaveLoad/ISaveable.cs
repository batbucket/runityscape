namespace Scripts.Model.SaveLoad {

    /// <summary>
    /// Interface used to facillitate saving and loading of game objects.
    /// To be inherited by the class to be saved in a serializable form.
    /// </summary>
    /// <typeparam name="S">Object to save to</typeparam>
    /// <typeparam name="L">Object to load from</typeparam>
    public interface ISaveable<S> {

        /// <summary>
        /// Gets the save object. A save object contains the neccessary
        /// information to initialize a clean class to its saved state.
        /// A save object is also serializable.
        /// </summary>
        /// <returns></returns>
        S GetSaveObject();

        /// <summary>
        /// Initializes the object from its associated save object.
        /// This lets me encapsulate data. (No need to make any private fields public for setting,
        /// just do it in this method!)
        /// </summary>
        /// <param name="saveObject">The save object.</param>
        void InitFromSaveObject(S saveObject);
    }
}