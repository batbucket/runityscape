using Scripts.Model.Stats.Resources;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct ResourceSave : IRestorable<Resource> {
        public PairedValueSave Paired;
        public string Type;

        public ResourceSave(Resource r) {
            Paired = new PairedValueSave(r.True, r.False);
            Type = Util.GetClassName(r);
        }

        public Resource Restore() {
            Resource r = Util.StringToObject<Resource>(Type);
            r.True = Paired.True;
            r.False = Paired.False;
            return r;
        }
    }
}