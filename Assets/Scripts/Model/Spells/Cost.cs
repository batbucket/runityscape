public struct Cost {
    public ResourceType resource;
    public int amount;

    public Cost(ResourceType res, int amount) {
        this.resource = res;
        this.amount = amount;
    }
}
