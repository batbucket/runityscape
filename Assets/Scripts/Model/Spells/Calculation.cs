using System.Collections.Generic;

public class Calculation {

    IDictionary<AttributeType, PairedInt> _targetAttributes;
    public IDictionary<AttributeType, PairedInt> TargetAttributes { get { return _targetAttributes; } }

    IDictionary<ResourceType, PairedInt> _targetResources;
    public IDictionary<ResourceType, PairedInt> TargetResources { get { return _targetResources; } }

    public IDictionary<AttributeType, PairedInt> _casterAttributes;
    public IDictionary<AttributeType, PairedInt> CasterAttributes { get { return _casterAttributes; } }

    public IDictionary<ResourceType, PairedInt> _casterResources;
    public IDictionary<ResourceType, PairedInt> CasterResources { get { return _casterResources; } }

    public Calculation(IDictionary<AttributeType, PairedInt> targetAttributes = null,
                        IDictionary<ResourceType, PairedInt> targetResources = null,
                        IDictionary<AttributeType, PairedInt> casterAttributes = null,
                        IDictionary<ResourceType, PairedInt> casterResources = null) {
        this._targetAttributes = targetAttributes ?? CreateBlankAttributeDictionary();
        this._targetResources = targetResources ?? CreateBlankResourceDictionary();
        this._casterAttributes = casterAttributes ?? CreateBlankAttributeDictionary();
        this._casterResources = casterResources ?? CreateBlankResourceDictionary();
    }

    public void Clear() {
        this._targetAttributes = CreateBlankAttributeDictionary();
        this._targetResources = CreateBlankResourceDictionary();
        this._casterAttributes = CreateBlankAttributeDictionary();
        this._casterResources = CreateBlankResourceDictionary();
    }

    IDictionary<AttributeType, PairedInt> CreateBlankAttributeDictionary() {
        IDictionary<AttributeType, PairedInt> d = new Dictionary<AttributeType, PairedInt>();
        foreach (AttributeType at in AttributeType.ALL) {
            d.Add(at, new PairedInt(0, 0));
        }
        return d;
    }

    IDictionary<ResourceType, PairedInt> CreateBlankResourceDictionary() {
        IDictionary<ResourceType, PairedInt> d = new Dictionary<ResourceType, PairedInt>();
        foreach (ResourceType rt in ResourceType.ALL) {
            d.Add(rt, new PairedInt(0, 0));
        }
        return d;
    }
}