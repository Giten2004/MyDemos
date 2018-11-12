using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class ExtendedDataAnnotationsProvider : CachedDataAnnotationsModelMetadataProvider
    {
        protected override CachedDataAnnotationsModelMetadata CreateMetadataPrototype(IEnumerable<Attribute> attributes,Type containerType, Type modelType, string propertyName)
        {
            CachedDataAnnotationsModelMetadata modelMetadata = base.CreateMetadataPrototype(attributes, containerType, modelType, propertyName);
            if (string.IsNullOrEmpty(modelMetadata.DisplayName))
            {
                DisplayTextAttribute displayTextAttribute = attributes.OfType<DisplayTextAttribute>().FirstOrDefault();
                if (null != displayTextAttribute)
                {
                    displayTextAttribute.SetDisplayName(modelMetadata);
                }
            }
            return modelMetadata;
        }

        protected override CachedDataAnnotationsModelMetadata CreateMetadataFromPrototype(CachedDataAnnotationsModelMetadata prototype, Func<object> modelAccessor)
        {
            CachedDataAnnotationsModelMetadata modelMetadata = base.CreateMetadataFromPrototype(prototype, modelAccessor);
            modelMetadata.DisplayName = prototype.DisplayName;
            return modelMetadata;
        }
    }
}