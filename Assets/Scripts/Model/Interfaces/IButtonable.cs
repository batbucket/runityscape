namespace Scripts.Model.Interfaces {

    /// <summary>
    /// Represents an object that can be converted
    /// into a button.
    /// </summary>
    public interface IButtonable {
        string ButtonText { get; }
        bool IsInvokable { get; }
        bool IsVisibleOnDisable { get; }
        string TooltipText { get; }

        void Invoke();
    }
}