using Portfolio.Client.Components.Forms;
using Microsoft.AspNetCore.Components.Forms;
using NUnit.Framework;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace Portfolio.Client.Tests.Components.Forms
{
    /// <summary>
    /// Test suite for <see cref="CustomFieldClassProvider"/>
    /// </summary>
    [TestFixture]
    public class CustomFieldClassProviderTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        // Form model
        private TestModel _model;
        // Form edit context
        private EditContext _context;
        // Component under test
        private CustomFieldClassProvider _cut;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        // Valid CSS class
        private const string ValidClass = "valid";
        // Invalid CSS class
        private const string InvalidClass = "invalid";

        #region Nested Types

        private class TestModel
        {
            public int ValidField { get; set; }
            [Range(0, 1)]
            public int InvalidField { get; set; } = 2;
        }

        #endregion

        #region Set-up and Tear-down

        [OneTimeSetUp]
        public void Setup()
        {
            // Create model and edit context
            _model      = new();
            _context    = new(_model);

            // Validate model
            _context.EnableDataAnnotationsValidation();
            _context.Validate();

            // Create component under test
            _cut = new(ValidClass, InvalidClass);
        }

        #endregion

        #region Overridden Function Tests

        /// <summary>
        /// Tests <see cref="CustomFieldClassProvider.GetFieldCssClass(EditContext, in FieldIdentifier)"/>
        /// with a valid field
        /// </summary>
        [Test]
        public void GetFieldCssClass_ValidField()
        {
            Assert.AreEqual(ValidClass, _cut.GetFieldCssClass(_context, 
                _context.Field(nameof(TestModel.ValidField))));
        }

        /// <summary>
        /// Tests <see cref="CustomFieldClassProvider.GetFieldCssClass(EditContext, in FieldIdentifier)"/>
        /// with an invalid field
        /// </summary>
        [Test]
        public void GetFieldCssClass_InvalidField()
        {
            Assert.AreEqual(InvalidClass, _cut.GetFieldCssClass(_context,
                _context.Field(nameof(TestModel.InvalidField))));
        }

        #endregion
    }
}
