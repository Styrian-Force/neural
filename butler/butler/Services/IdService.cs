
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using butler.Interfaces;
using Microsoft.Extensions.Logging;

namespace butler.Services
{
    public class IdService : IIdService
    {
        private ILogger<IdService> _logger;

        public IdService(
            ILogger<IdService> logger
        )
        {            
            this._logger = logger;
        }

        public string GenerateId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}