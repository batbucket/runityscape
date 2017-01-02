using System;
using System.Collections.Generic;

[System.Serializable]
public struct ResourceListSave : IRestorable<IDictionary<ResourceType, Resource>> {
    ResourceSave[] Resources;

    public ResourceListSave(IList<Resource> list) {
        Resources = new ResourceSave[list.Count];
        int index = 0;
        foreach (Resource r in list) {
            Resources[index++] = new ResourceSave(r);
        }
    }

    public IDictionary<ResourceType, Resource> Restore() {
        IDictionary<ResourceType, Resource> resources = new SortedDictionary<ResourceType, Resource>();
        foreach (ResourceSave r in Resources) {
            Resource restoredRes = r.Restore();
            resources.Add(restoredRes.Type, restoredRes);
        }
        return resources;
    }
}
