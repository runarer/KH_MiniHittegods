using System;

namespace MiniHittegodsApi.Test;

public class DeleteFoundTest
{
    [Fact]
    public void DeleteItem_DeleteItemWithIdItemExcistsAndGotStatusAsAvailable_Return204()
    {

    }

    [Fact]
    public void DeleteItem_DeleteItemWithIdItemDoesntExcist_Return404AndErrorMessage()
    {

    }

    [Fact]
    public void DeleteItem_DeleteItemWithIdItemExcistsButStatusIsNotSetToAvailable_Return409()
    {

    }
}
