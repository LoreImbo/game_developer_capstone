using System.Collections.Generic;

public class Inventory {
    public List<string> keys;
    public bool HasKey(string id) => keys.Contains(id);
}
