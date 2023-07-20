﻿namespace Orc.DataAccess.Tests;

using System;
using System.Data;
using System.Data.Common;

public class TestDbConnection : DbConnection
{
    public bool IsValid { get; set; } = true;

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => null;

    protected override DbCommand CreateDbCommand() => null;

    public override void ChangeDatabase(string databaseName)
    {
        //do nothing
    }

    public override void Close()
    {
        //do nothing
    }

    public override void Open()
    {
        if (!IsValid)
        {
            throw new Exception("Invalid connection");
        }
    }

    public override string ConnectionString { get; set; }
    public override string Database { get; }
    public override System.Data.ConnectionState State { get; }
    public override string DataSource { get; }
    public override string ServerVersion { get; }
}