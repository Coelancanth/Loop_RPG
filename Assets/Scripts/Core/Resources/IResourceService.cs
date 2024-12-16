public interface IResourceService
{
    int GetResourceCount(ResourceType type);
    bool TryConsumeResource(ResourceType type, int amount);
    void AddResource(ResourceType type, int amount);
    int GetMaxStorage(ResourceType type);
} 