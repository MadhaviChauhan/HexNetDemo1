using NUnit.Framework;

namespace Test_Solution1.TestFramework
{
    [TestFixture]
    public abstract class SpecFor
    {
        [SetUp]
        public void SetUp()
        {
            Context();
            Because();
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

        public abstract void Context();
        public abstract void Because();

        public virtual void CleanUp()
        {
        }
    }

    [TestFixture]
    public abstract class SpecFor<T>
    {
        [SetUp]
        public void SetUp()
        {
            Context();
            Because();
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }


        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            GlobalContext();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            GlobalCleanUp();
        }

        protected T subject;

        public abstract void Context();
        public abstract void Because();



        public virtual void CleanUp()
        {
        }


        public virtual void GlobalContext()
        {

        }


        public virtual void GlobalCleanUp()
        {

        }
    }
}
