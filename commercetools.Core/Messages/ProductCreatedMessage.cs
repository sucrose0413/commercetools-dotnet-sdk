﻿using commercetools.ProductProjections;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the create product request.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#productcreated-message"/>
    public class ProductCreatedMessage : Message
    {
        #region Properties

        [JsonProperty(PropertyName = "productProjection")]
        public ProductProjection ProductProjection { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ProductCreatedMessage(dynamic data = null)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.ProductProjection = new ProductProjection(data.productProjection);
        }

        #endregion
    }
}