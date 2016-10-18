﻿using Newtonsoft.Json;

namespace commercetools.Types
{
    /// <summary>
    /// ReferenceType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#referencetype"/>
    public class ReferenceType : FieldType
    {
        #region Properties

        [JsonProperty(PropertyName = "referenceTypeId")]
        public string ReferenceTypeId { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public ReferenceType(dynamic data = null)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.ReferenceTypeId = data.referenceTypeId;
        }

        #endregion
    }
}