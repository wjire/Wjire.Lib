﻿using System;
using System.Collections.Generic;
using System.Text;
using Wjire.Db;

namespace ConsoleTest
{
    public class UserInfoRepository : BaseRepository<UserInfo>
    {
        public UserInfoRepository(string name) : base(name)
        {
        }

        public UserInfoRepository(IUnitOfWork unit) : base(unit)
        {
        }
    }

    public class UserInfo
    {

    }
}