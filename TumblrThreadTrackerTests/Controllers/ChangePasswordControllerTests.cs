using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NUnit.Framework;
using TumblrThreadTracker.Controllers;

namespace TumblrThreadTrackerTests.Controllers
{
    [TestFixture]
    public class ChangePasswordControllerTests
    {
        [Test]
        public void Change_Password_Controller_Should_Throw_Exception_For_Null_Request()
        {
            // Arrange
            var controller = new ChangePasswordController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            TestDelegate thrower = () => controller.ChangePassword(null);

            //Assert
            Assert.Throws<InvalidOperationException>(thrower);

        }
    }
}
