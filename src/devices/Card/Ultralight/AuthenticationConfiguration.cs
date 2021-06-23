﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Device.Card.Ultralight
{
    /// <summary>
    /// Contains the Authentication Configuration elements
    /// </summary>
    public class AuthenticationConfiguration
    {
        /// <summary>
        /// Is Read Write Authentication Required
        /// </summary>
        public bool IsReadWriteAuthenticationRequired { get; set; }

        /// <summary>
        /// Gets or sets the authentication page requirement
        /// </summary>
        /// <remarks>If the page is higher than the capacity, it means no authentication required.</remarks>
        public byte AuthenticationPageRequirement { get; set; }

        /// <summary>
        /// Is user configuration permanently locked against write access, except PWD and PACK
        /// </summary>
        public bool IsWrittenLocked { get; set; }

        /// <summary>
        /// Maximum number of possible try, 0 = disabled, 1 to 7 enabled
        /// </summary>
        public byte MaximumNumberOfPossibleTry { get; set; }
    }
}
