using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;



namespace MyoSimGuiTest.Test
{
    [TestFixture]
    public class MyoSimTest
    {
        [Test]
        public void testpass()
        {
            Debug.Assert(true);
         
        }

        [Test]
        [Ignore("this test is supposed to fail")]
        public void testfail()
        {
            Debug.Assert(false);
        }

        [Test]
        public void testequal()
        {
            Assert.AreEqual(1, 1);
        }
                
    }
}
