namespace Linnworks.IntegrationTests.Tests
{
    public class BaseIntegrationTests
    {
        protected readonly DatabaseOperations.Core.DatabaseOperations DatabaseOperations;

        public BaseIntegrationTests()
        {
            DatabaseOperations = new DatabaseOperations.Core.DatabaseOperations();
        }
    }
}