namespace Scripts.Model.Characters {

    /// <summary>
    /// Represents an object
    /// that can show up on the
    /// PortraitHolders.
    /// </summary>
    public abstract class Entity {

        public Entity(string spriteLoc) {
            this.SpriteLoc = spriteLoc;
        }

        public string SpriteLoc { get; set; }
    }
}