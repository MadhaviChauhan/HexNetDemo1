using System.Configuration;

namespace Caching
{
    public class CacheConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("types", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CacheTypeCollection))]
        public CacheTypeCollection Types
        {
            get { return (CacheTypeCollection)base["types"]; }
        }

        public class CacheMethodCollection : ConfigurationElementCollection
        {
            public CacheMethodElement this[int index]
            {
                get { return (CacheMethodElement)BaseGet(index); }
                set
                {
                    if (BaseGet(index) != null)
                    {
                        BaseRemoveAt(index);
                    }
                    BaseAdd(index, value);
                }
            }

            public new CacheMethodElement this[string type]
            {
                get { return (CacheMethodElement)BaseGet(type); }
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return new CacheMethodElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((CacheMethodElement)element).Name;
            }

            public void Remove(string type)
            {
                BaseRemove(type);
            }

            public void Clear()
            {
                BaseClear();
            }

            public void Add(CacheMethodElement customElement)
            {
                BaseAdd(customElement);
            }

            protected override void BaseAdd(ConfigurationElement element)
            {
                BaseAdd(element, false);
            }
        }

        public class CacheMethodElement : ConfigurationElement
        {
            [ConfigurationProperty("name", IsRequired = true)]
            public string Name
            {
                get { return (string)this["name"]; }
                set { this["name"] = value; }
            }

            [ConfigurationProperty("expirationInMinutes", IsRequired = false, DefaultValue = 15)]
            public int ExpirationValue
            {
                get { return (int)this["expirationInMinutes"]; }
                set { this["expirationInMinutes"] = value; }
            }

            [ConfigurationProperty("expirationType", IsRequired = false, DefaultValue = "Absolute")]
            public string ExpirationType
            {
                get { return this["expirationType"].ToString(); }
                set { this["expirationType"] = value; }
            }
        }

        public class CacheTypeCollection : ConfigurationElementCollection
        {
            public CacheTypeElement this[int index]
            {
                get { return (CacheTypeElement)BaseGet(index); }
                set
                {
                    if (BaseGet(index) != null)
                    {
                        BaseRemoveAt(index);
                    }
                    BaseAdd(index, value);
                }
            }

            public new CacheTypeElement this[string type]
            {
                get { return (CacheTypeElement)BaseGet(type); }
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return new CacheTypeElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((CacheTypeElement)element).Type;
            }

            public void Remove(string type)
            {
                BaseRemove(type);
            }

            public void Clear()
            {
                BaseClear();
            }

            public void Add(CacheTypeElement customElement)
            {
                BaseAdd(customElement);
            }

            protected override void BaseAdd(ConfigurationElement element)
            {
                BaseAdd(element, false);
            }
        }

        public class CacheTypeElement : ConfigurationElement
        {
            [ConfigurationProperty("type", IsRequired = true)]
            public string Type
            {
                get { return (string)this["type"]; }
                set { this["type"] = value; }
            }

            [ConfigurationProperty("methods", IsDefaultCollection = true)]
            [ConfigurationCollection(typeof(CacheMethodCollection))]
            public CacheMethodCollection Methods
            {
                get { return (CacheMethodCollection)base["methods"]; }
            }
        }
    }
}