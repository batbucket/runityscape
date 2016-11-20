using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour {
    public static ObjectPoolManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<ObjectPoolManager>();
            }
            return instance;
        }
    }
    private static ObjectPoolManager instance;

    private IDictionary<string, Stack<PooledBehaviour>> pools;

    public void Start() {
        pools = new Dictionary<string, Stack<PooledBehaviour>>();
    }

    public void Register(PooledBehaviour prefab, int count) {
        //Util.Log(string.Format("Registering {0} of {1}.", count, prefab.gameObject.name));
        if (!pools.ContainsKey(prefab.gameObject.name)) {
            pools.Add(prefab.name, new Stack<PooledBehaviour>());
        }
        for (int i = 0; i < count; i++) {
            PooledBehaviour pb = (Instantiate(prefab.gameObject)).GetComponent<PooledBehaviour>();
            pb.gameObject.name = prefab.gameObject.name;
            pb.gameObject.SetActive(false);
            Util.Parent(pb.gameObject, this.gameObject);
            pools[prefab.name].Push(pb);
        }
    }

    public T Get<T>(T script) where T : PooledBehaviour {
        Util.Assert(
            pools.ContainsKey(script.gameObject.name),
            string.Format("Unable to find {0} in Pool. Did you register the prefab?", script.gameObject.name)
            );
        PooledBehaviour pb = null;
        if (pools[script.gameObject.name].Count == 0) {
            //Util.Log(string.Format("Pool ran out of {0}. Instantiating...", script.gameObject.name));
            pb = (Instantiate(script.gameObject)).GetComponent<PooledBehaviour>();
            pb.gameObject.name = script.gameObject.name;
        } else {
            //Util.Log(string.Format("Getting {0} from a pool of {1}.", script.gameObject.name, pools[script.gameObject.name].Count));
            pb = pools[script.gameObject.name].Pop();
        }
        pb.gameObject.SetActive(true);
        return pb.GetComponent<T>();
    }

    public void Return(PooledBehaviour pb) {
        Util.Assert(
            pools.ContainsKey(pb.gameObject.name),
            string.Format("Unable to find {0} in Pool. Did you use Instantiate?", pb.gameObject.name));
        pb.Reset();
        pb.gameObject.SetActive(false);
        pools[pb.gameObject.name].Push(pb);
        Util.Parent(pb.gameObject, this.gameObject);
    }

    private void Foo() {
        GameObject.Destroy(this.gameObject);
    }
}
