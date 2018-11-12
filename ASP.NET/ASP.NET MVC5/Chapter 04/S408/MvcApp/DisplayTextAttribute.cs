using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class DisplayTextAttribute : Attribute, IMetadataAware
    {
        private static Type staticResourceType;
        public string DisplayName { get; set; }
        public Type ResourceType { get; set; }

        public DisplayTextAttribute()
        {
            this.ResourceType = staticResourceType;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            this.DisplayName = this.DisplayName ??(metadata.PropertyName ?? metadata.ModelType.Name);
            if (null == this.ResourceType)
            {
                metadata.DisplayName = this.DisplayName;
                return;
            }
            PropertyInfo property = this.ResourceType.GetProperty(this.DisplayName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            metadata.DisplayName = property.GetValue(null, null).ToString();
        }

        public static void SetResourceType(Type resourceType)
        {
            staticResourceType = resourceType;
        }
    }
}