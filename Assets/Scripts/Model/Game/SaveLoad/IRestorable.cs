namespace Scripts.Model.World.Serialization {

    /// <summary>
    /// This is implemented on every SaveObject, which is a struct representing a class object.
    /// Through this method the struct can reinstantiate the class object it is storing data for.
    /// </summary>
    /// <typeparam name="T">Type of the class object this datatype is storing information for</typeparam>
    public interface IRestorable<T> {

        T Restore();
    }
}