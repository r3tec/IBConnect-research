﻿/* Copyright (C) 2019 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBBackend.Messages
{
    public class AccountDownloadEndMessage
    {
        private string account;
        
        public AccountDownloadEndMessage(string account)
        {
            Account = account;
        }

        public string Account
        {
            get { return account; }
            set { account = value; }
        }
    }
}
