

namespace MiniHittegodsApi.Test;

public class MarkItemAsClaimedTest
{
    [Fact]
    public void ClaimItem_MarkItemAsClaimedItemIsAvailableAndClaimedByGotAValue_Return200WithUpdatedItemAndStatusSetToClaimed()
    {

    }
    [Fact]
    public void ClaimItem_MarkItemAsClaimedIdOfItemNotAvailable_Return404AndErroMessage()
    {

    }
    [Fact]
    public void ClaimItem_MarkItemAsClaimedItemIsNotAvailable_Return409AndErrorMessage()
    {

    }
    [Fact(Skip = "Not Implemented, wait until I'm sure I got time.")]
    public void ClaimItem_MarkItemAsClaimedItemIsAvailableButClaimedByGotNoValue_Return400AndErrorMessage()
    {

    }
}
