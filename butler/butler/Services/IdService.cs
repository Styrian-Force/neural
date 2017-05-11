
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Butler.Interfaces;
using Microsoft.Extensions.Logging;

namespace Butler.Services
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
            //Guid guid = Guid.NewGuid();
            //return guid.ToString();
            return string.Format("{0:yyyy-MM-dd_HH-mm-ss.fff}", DateTime.Now);
        }
    }
}