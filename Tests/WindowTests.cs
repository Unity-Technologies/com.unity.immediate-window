using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.ImmediateWindow.UI;

namespace UnityEditor.ImmediateWindow.UI.Tests
{
    internal class WindowTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ImmediateWindowWorks()
        {
            ImmediateWindow.ShowPackageManagerWindow();
            Assert.That(ImmediateWindow.CurrentWindow != null);
        }
    }
}
