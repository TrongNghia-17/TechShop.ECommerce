using TechShop.ECommerce.Application.Contracts.Persistence;

namespace TechShop.ECommerce.Application.UnitTests.Mocks.UnitOfWork;

public static class MockUnitOfWork
{
    public static Mock<IUnitOfWork> GetMock()
    {
        var mockUow = new Mock<IUnitOfWork>();

        mockUow
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        return mockUow;
    }
}
