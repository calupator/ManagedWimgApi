﻿using System;
using System.IO;
using NUnit.Framework;
using Shouldly;

namespace Microsoft.Wim.Tests
{
    [TestFixture]
    public class ExportImageTests : TestBase
    {
        [Test]
        public void ExportImageTest()
        {
            var exportWimPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "export.wim");

            using (var imageHandle = WimgApi.LoadImage(TestWimHandle, 1))
            {
                using (var wimHandle = WimgApi.CreateFile(exportWimPath, WimFileAccess.Write, WimCreationDisposition.CreateAlways, WimCreateFileOptions.None, WimCompressionType.Lzx))
                {
                    WimgApi.SetTemporaryPath(wimHandle, TempPath);

                    WimgApi.ExportImage(imageHandle, wimHandle, WimExportImageOptions.None);
                }
            }

            File.Exists(exportWimPath).ShouldBeTrue();

            new FileInfo(exportWimPath).Length.ShouldNotBe(0);
        }

        [Test]
        public void ExportImageTest_ThrowsArgumentNullException_imageHandle()
        {
            ShouldThrow<ArgumentNullException>("imageHandle", () =>
                WimgApi.ExportImage(null, null, WimExportImageOptions.None));
        }

        [Test]
        public void ExportImageTest_ThrowsArgumentNullException_wimHandle()
        {
            using (var imageHandle = WimgApi.LoadImage(TestWimHandle, 1))
            {
                var imageHandleCopy = imageHandle;

                ShouldThrow<ArgumentNullException>("wimHandle", () =>
                    WimgApi.ExportImage(imageHandleCopy, null, WimExportImageOptions.None));
            }
        }
    }
}