namespace Caching
{
    public class CacheRequest
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public int ExpiryInMinutes { get; set; }
        public string DependentKey { get; set; }
        public ExpirationType ExpirationType { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Key: {0}; Value: {1}; ExpiryInMinutes: {2}; ExpirationType: {3}; DependentKey: {4};",
                Key, Value, ExpiryInMinutes, ExpirationType, DependentKey);
        }
    }
}