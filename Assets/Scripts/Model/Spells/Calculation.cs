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

    public Result Result { get; set; }

    public Calculation(IDictionary<AttributeType, PairedInt> targetAttributes = null,
                        IDictionary<ResourceType, PairedInt> targetResources = null,
                        IDictionary<AttributeType, PairedInt> casterAttributes = null,
                        IDictionary<ResourceType, PairedInt> casterResources = null) {
        this._targetAttributes = targetAttributes ?? new Dictionary<AttributeType, PairedInt>();
        this._targetResources = targetResources ?? new Dictionary<ResourceType, PairedInt>();
        this._casterAttributes = casterAttributes ?? new Dictionary<AttributeType, PairedInt>();
        this._casterResources = casterResources ?? new Dictionary<ResourceType, PairedInt>();
    }

    public void Clear() {
        this._targetAttributes = new Dictionary<AttributeType, PairedInt>();
        this._targetResources = new Dictionary<ResourceType, PairedInt>();
        this._casterAttributes = new Dictionary<AttributeType, PairedInt>();
        this._casterResources = new Dictionary<ResourceType, PairedInt>();
    }
}