using Scripts.Model.Stats;
using Scripts.Model.Stats.Attributes;
using Scripts.Model.Stats.Resources;
using System.Collections.Generic;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Represents the numeric effects of a spell.
    /// </summary>
    public class Calculation {
        public IDictionary<AttributeType, PairedValue> _casterAttributes;
        public IDictionary<ResourceType, PairedValue> _casterResources;
        private IDictionary<AttributeType, PairedValue> _targetAttributes;
        private IDictionary<ResourceType, PairedValue> _targetResources;

        public Calculation(IDictionary<AttributeType, PairedValue> targetAttributes = null,
                            IDictionary<ResourceType, PairedValue> targetResources = null,
                            IDictionary<AttributeType, PairedValue> casterAttributes = null,
                            IDictionary<ResourceType, PairedValue> casterResources = null) {
            this._targetAttributes = targetAttributes ?? CreateBlankAttributeDictionary();
            this._targetResources = targetResources ?? CreateBlankResourceDictionary();
            this._casterAttributes = casterAttributes ?? CreateBlankAttributeDictionary();
            this._casterResources = casterResources ?? CreateBlankResourceDictionary();
        }

        public IDictionary<AttributeType, PairedValue> CasterAttributes { get { return _casterAttributes; } }
        public IDictionary<ResourceType, PairedValue> CasterResources { get { return _casterResources; } }
        public IDictionary<AttributeType, PairedValue> TargetAttributes { get { return _targetAttributes; } }
        public IDictionary<ResourceType, PairedValue> TargetResources { get { return _targetResources; } }

        public void Clear() {
            this._targetAttributes = CreateBlankAttributeDictionary();
            this._targetResources = CreateBlankResourceDictionary();
            this._casterAttributes = CreateBlankAttributeDictionary();
            this._casterResources = CreateBlankResourceDictionary();
        }

        private IDictionary<AttributeType, PairedValue> CreateBlankAttributeDictionary() {
            IDictionary<AttributeType, PairedValue> d = new Dictionary<AttributeType, PairedValue>();
            foreach (AttributeType at in AttributeType.ALL) {
                d.Add(at, new PairedValue(0, 0));
            }
            return d;
        }

        private IDictionary<ResourceType, PairedValue> CreateBlankResourceDictionary() {
            IDictionary<ResourceType, PairedValue> d = new Dictionary<ResourceType, PairedValue>();
            foreach (ResourceType rt in ResourceType.ALL) {
                d.Add(rt, new PairedValue(0, 0));
            }
            return d;
        }
    }
}