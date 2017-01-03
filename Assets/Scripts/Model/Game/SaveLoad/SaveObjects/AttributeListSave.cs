using Scripts.Model.Stats.Attributes;
using System.Collections.Generic;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct AttributeListSave : IRestorable<IDictionary<AttributeType, Attribute>> {
        public AttributeSave[] Attributes;

        public AttributeListSave(IList<Attribute> list) {
            Attributes = new AttributeSave[list.Count];

            int index = 0;
            foreach (Attribute a in list) {
                Attributes[index++] = new AttributeSave(a);
            }
        }

        public IDictionary<AttributeType, Attribute> Restore() {
            IDictionary<AttributeType, Attribute> attributes = new SortedDictionary<AttributeType, Attribute>();
            foreach (AttributeSave a in Attributes) {
                Attribute restoredAttribute = a.Restore();
                attributes.Add(restoredAttribute.Type, restoredAttribute);
            }
            return attributes;
        }
    }
}