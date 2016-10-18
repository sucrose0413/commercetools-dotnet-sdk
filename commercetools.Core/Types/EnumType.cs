﻿using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Types
{
    /// <summary>
    /// EnumType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#enumtype"/>
    public class EnumType : FieldType
    {
        #region Properties

        [JsonProperty(PropertyName = "values")]
        public List<EnumValue> Values { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public EnumType(dynamic data = null)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Values = Helper.GetListFromJsonArray<EnumValue>(data.values);
        }

        #endregion
    }
}