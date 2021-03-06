// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Moryx.Model.Tests
{
    public interface ICreateStringParamRepository : IRepository<SomeEntity>
    {
        SomeEntity Create(string name);
    }
}
