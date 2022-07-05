﻿using Ordering.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Infrastructure
{
    public interface EmailServices
    {
        Task<bool> SendEmail(Email email);
    }
}
