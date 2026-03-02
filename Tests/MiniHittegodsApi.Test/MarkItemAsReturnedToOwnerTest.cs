using System;

namespace MiniHittegodsApi.Test;

public class MarkItemAsReturnedToOwnerTest
{
    [Fact]
    public void ReturnItem_MarkItemAsReturnedItemExcistsAndGotStatusAsClaimed_Return200WithItemAndSetStatusToReturnedAndTimeItWasClaimed()
    {

    }

    [Fact]
    public void ReturnItem_MarkItemAsReturnedItemDoedntExcists_Return404AndErrorMessage()
    {

    }

    [Fact]
    public void ReturnItem_MarkItemAsReturnedItemExcistsButDoNotGotStatusAsClaimed_Return409AndErrorMessage()
    {

    }
}
