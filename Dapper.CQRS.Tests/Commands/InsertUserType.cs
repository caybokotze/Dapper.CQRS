﻿using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Commands
{
    public class InsertUserType : Command<int>
    {
        public UserType UserType { get; }

        public InsertUserType(UserType userType)
        {
            UserType = userType;
        }
        
        public override void Execute()
        {
            var sql = new SqlBuilder()
                .Insert<UserType>("user_types")
                .Values()
                .Build();

            Execute(sql);
        }
    }
}