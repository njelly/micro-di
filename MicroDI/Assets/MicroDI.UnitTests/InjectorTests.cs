using MicroDI.Attributes;
using MicroDI.Core;
using MicroDI.Exceptions;
using MicroDI.Interfaces;
using NUnit.Framework;

namespace MicroDI.UnitTests
{
    #region Data
    interface ITest1 { }
    class Test1 : ITest1 { }

    interface ITest2
    {
        ITest1 Test1 { get; set; }
        IContainer Container { get; set; }
    }
    class Test2 : ITest2
    {
        public ITest1 Test1 { get; set; }
        public IContainer Container { get; set; }

        public Test2(ITest1 test1, IContainer container)
        {
            Test1 = test1;
            Container = container;
        }
    }

    interface ITest3
    {
        ITest1 Test1 { get; set; }
        string Text { get; set; }
        bool Boolean { get; set; }
    }
    class Test3 : ITest3
    {
        public ITest1 Test1 { get; set; }
        public string Text { get; set; }
        public bool Boolean { get; set; }

        public Test3(ITest1 test1, string text, bool boolean)
        {
            Test1 = test1;
            Text = text;
            Boolean = boolean;
        }
    }

    interface ITest4A { }
    interface ITest4B { }
    class Test4A : ITest4A
    {
        public Test4A(ITest4B test) { }
    }
    class Test4B : ITest4B
    {
        public Test4B(ITest4A test) { }
    }

    interface ITest5
    {
        ITest1 Test1 { get; set; }
    }
    class Test5 : ITest5
    {
        [Inject] public ITest1 Test1 { get; set; }
    }
    
    interface ITest6 { }
    class Test6A : ITest6 { }
    class Test6B : ITest6 { }

    interface ITest7 { ITest6 Property { get; set; } }
    class Test7 : ITest7
    {
        public Test7(ITest6 test6)
        {
            Property = test6;
        }

        public ITest6 Property { get; set; }
    }
    #endregion

    public class InjectorTests
    {
        [Test]
        public void DoesResolveTransient()
        {
            var container = new Container();
            container.Bind<ITest1, Test1>();

            var instance1 = container.Resolve<ITest1>();
            var instance2 = container.Resolve<ITest1>();

            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);

            Assert.AreNotEqual(instance1, instance2);
        }

        [Test]
        public void DoesResolveSingleton()
        {
            var container = new Container();
            container.Bind<ITest1, Test1>().AsSingleton();
            container.Bind<ITest2, Test2>();

            var instance = container.Resolve<ITest2>();

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Test1);
            Assert.IsNotNull(instance.Container);
        }

        [Test]
        public void DoesResolveWith()
        {
            var container = new Container();
            container.Bind<ITest1, Test1>().AsSingleton();
            container.Bind<ITest2, Test2>();
            container.Bind<ITest3, Test3>();

            var instance = container.ResolveWith<ITest3>(null, "Hello World", true);

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Test1);
            Assert.AreEqual(instance.Text, "Hello World");
            Assert.AreEqual(instance.Boolean, true);
        }

        [Test]
        public void DoesThrowCircularReference()
        {
            var caught = false;
            var container = new Container();
            container.Bind<ITest4A, Test4A>();
            container.Bind<ITest4B, Test4B>();

            try
            {
                var instance = container.Resolve<ITest4A>();
            }
            catch(CircularInjectionException)
            {
                caught = true;
            }

            Assert.AreEqual(caught, true);
        }

        [Test]
        public void DoesInjectMembers()
        {
            var container = new Container();
            container.Bind<ITest1, Test1>().AsSingleton();
            container.Bind<ITest5, Test5>();

            var instance = container.Resolve<ITest5>();

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Test1);
        }

        [Test]
        public void DoesResolveTaggedTypes()
        {
            var container = new Container();
            container.Bind<ITest6, Test6A>().WithTag("a").AsSingleton();
            container.Bind<ITest6, Test6B>().WithTag("b").AsSingleton();

            var instanceA = container.Resolve<ITest6>("a");
            var instanceB = container.Resolve<ITest6>("b");

            Assert.IsNotNull(instanceA);
            Assert.IsNotNull(instanceB);
            Assert.AreNotEqual(instanceA, instanceB);
        }

        [Test]
        public void DoesResolveWithCustomConstructor()
        {
            var container = new Container();
            container.Bind<ITest6, Test6A>().WithFactory(delegate
            {
                return new Test6A();
            });

            var instance = container.Resolve<ITest6>();

            Assert.IsNotNull(instance);
        }

        [Test]
        public void DoesThrowCustomFactoryInjectionException()
        {
            var caught = false;
            var container = new Container();
            container.Bind<ITest6, Test6A>().WithFactory(delegate
            {
                return new Test1();
            });

            try
            {
                var instance = container.Resolve<ITest6>();
            }
            catch(CustomFactoryInjectionException)
            {
                caught = true;
            }

            Assert.AreEqual(true, caught);
        }
    }
}
